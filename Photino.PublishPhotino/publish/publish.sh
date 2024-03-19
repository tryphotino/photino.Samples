#!/bin/bash

# Set publish options
APP_NAME="PublishPhotino"
APP_NAME_LC=$(echo $APP_NAME | tr '[:upper:]' '[:lower:]')

APP_URN_ROOT="io.tryphotino"
APP_URN="$APP_URN_ROOT.$APP_NAME_LC"

APP_MAINTAINER="TryPhotino <info@tryphotino.io>"
APP_DESC_SHORT="PublishPhotino Demo"
APP_DESC_LONG="PublishPhotino is a simple, cross-platform demo to show packaging options for Photino applications."

PROJECT_ROOT=$(pwd)/..
APPLICATION_DIR="$PROJECT_ROOT"

PUBLISH_CONFIG="Release"
PUBLISH_PLATFORMS="win-x64 win-arm64 osx-x64 osx-arm64 linux-x64 linux-arm64"

PUBLISH_ROOT="$PROJECT_ROOT/publish"
PUBLISH_TEMPLATES="$PUBLISH_ROOT/templates"
PUBLISH_BUILD="$PUBLISH_ROOT/build"
PUBLISH_OUTPUT="$PUBLISH_ROOT/output"

# Function to generate formatted headers
function header() {
    echo -e "\033[;32m"
    echo "# $1"
    echo -en "\033[0m"
}

# Function to abort the script
function abort() {
    echo -en "\033[0;31m"
    echo "❌ Publishing aborted!"
    exit 1
}

# Function to create packages for Windows
create_win_package() {
    header "Creating package for Windows ($1) ..."

    cd $PUBLISH_BUILD/$APP_NAME.$APP_VERSION.$1

    mv $APP_NAME.exe $APP_NAME.$APP_VERSION.$1.exe

    zip -r $APP_NAME.$APP_VERSION.$1.zip $APP_NAME.$APP_VERSION.$1.exe

    mv $APP_NAME.$APP_VERSION.$1.zip $PUBLISH_OUTPUT

    cd $APPLICATION_DIR
}

# Function to create packages for macOS
create_mac_app_bundle() {
    header "Creating app bundle for macOS ($1) ..."

    cd $PUBLISH_BUILD/$APP_NAME.$APP_VERSION.$1
    
    cp -r $PUBLISH_TEMPLATES/osx/APP_NAME.app "./$APP_NAME.app"
    
    mkdir -p $APP_NAME.app/Contents/MacOS/
    mv $APP_NAME $APP_NAME.app/Contents/MacOS/

    # Replace version placeholder in Info.plist file, overwrite existing file
    cp $APP_NAME.app/Contents/Info.plist $APP_NAME.app/Contents/Info.plist-template
    
    cat $APP_NAME.app/Contents/Info.plist-template\
        | sed "s/APP_NAME/$APP_NAME/"\
        | sed "s/APP_URN/$APP_URN/"\
        | sed "s/APP_VERSION/$APP_VERSION/"\
        > $APP_NAME.app/Contents/Info.plist

    rm $APP_NAME.app/Contents/Info.plist-template

    # Create DMG file
    hdiutil create -volname $APP_NAME -srcfolder $APP_NAME.app -ov -format UDZO $APP_NAME.$APP_VERSION.$1.dmg
    mv $APP_NAME.$APP_VERSION.$1.dmg $PUBLISH_OUTPUT

    cd $APPLICATION_DIR
}

# Functions to create packages for Linux
create_deb_package() {
    if [ -x "$(command -v dpkg)" ]; then
        header "Creating DEB package for Linux ($1) ..."

        cd $PUBLISH_BUILD/$APP_NAME.$APP_VERSION.$1

        # Copy debian folder into release folder
        cp -r $PUBLISH_TEMPLATES/linux/deb ./debian
        # Copy published binaries into debian folder
        mkdir -p ./debian/bin
        mv $APP_NAME ./debian/bin || abort

        # Replace version placeholder in control file, overwrite existing file
        cp ./debian/DEBIAN/control ./debian/DEBIAN/control-template

        cat ./debian/DEBIAN/control-template \
            | sed "s/APP_NAME/$APP_NAME/"\
            | sed "s/APP_MAINTAINER/$APP_MAINTAINER/"\
            | sed "s/APP_VERSION/$APP_VERSION/"\
            | sed "s/APP_DESC_SHORT/$APP_DESC_SHORT/"\
            | sed "s/APP_DESC_LONG/$APP_DESC_LONG/"\
            | sed "s/APP_ARCH/$(echo $1 | sed 's/linux-//')/" > ./debian/DEBIAN/control
        
        rm ./debian/DEBIAN/control-template

        # Move doc folder to correct location
        mv "./debian/usr/share/doc/APP_NAME/" "./debian/usr/share/doc/$APP_NAME_LC/"

        # Write commit messages since previous tag into changelog file
        # Check if current directory is a git repository
        if [ ! -d "$PROJECT_ROOT/.git" ]; then
            echo -en "\033[0;31m"
            echo "! Not a git repository, skipping changelog generation."
            echo -en "\033[0m"
        else
            # Get previous to last git tag
            prev_tag=$(git tag -l 2> /dev/null | sed 's/v//' | sort -uVr | sed -n 2p)
            $(git log --pretty=format:"%ad %an%n%s%n" --date=short --no-merges v$prev_tag..HEAD 2> /dev/null) > ./changelog
        
            gzip --best -n ./changelog
            mv changelog.gz ./debian/usr/share/doc/$APP_NAME_LC/
            rm -f ./changelog
        fi

        # Replace placeholders in desktop config, overwrite existing file
        cat "./debian/usr/share/applications/APP_URN.desktop"\
            | sed "s/APP_NAME/$APP_NAME/"\
            | sed "s/APP_URN/$APP_URN/"\
            | sed "s/APP_DESC_SHORT/$APP_DESC_SHORT/"\
            | sed "s/APP_ARCH/$(echo $1 | sed 's/linux-//')/" > "./debian/usr/share/applications/$APP_URN.desktop"
        
        rm "./debian/usr/share/applications/APP_URN.desktop"

        # Move icon to correct location
        mv "./debian/usr/share/icons/hicolor/512x512/apps/APP_URN.png" "./debian/usr/share/icons/hicolor/512x512/apps/$APP_URN.png"

        # Create DEB package, stop publishing on error
        dpkg-deb --root-owner-group --build debian 2> /dev/null || abort
        mv 'debian.deb' "$PUBLISH_OUTPUT/$APP_NAME.$APP_VERSION.$1.deb"
        rm -rf ./debian

        cd $APPLICATION_DIR
    else
        echo -en "\033[0;31m"
        echo "! For Debian package generation, install 'dpkg' with 'apt' or 'brew'."
        echo -en "\033[0m"
    fi
}

create_flatpack_package() {
    if [ -x "$(command -v flatpak-builder)" ]; then
        header "Creating Flatpak package for Linux ($1) ..."

        cd $PUBLISH_BUILD/$APP_NAME.$APP_VERSION.$1

        # Copy flatpak folder into release folder
        cp -r $PUBLISH_TEMPLATES/linux/flatpak ./flatpak

        # Copy published binaries into debian folder
        mkdir -p ./flatpak/bin
        mv $APP_NAME ./flatpak/bin || abort

        # Replace version placeholder in manifest file, overwrite existing file
        cp ./flatpak/$APP_URN.yml ./flatpak/$APP_URN.yml-template

        cat ./flatpak/$APP_URN.yml-template\
            | sed "s/APP_NAME/$APP_NAME/"\
            | sed "s/APP_URN/$APP_URN/"\
            | sed "s/APP_ARCH/$1/" > ./flatpak/$APP_URN.yml

        rm ./flatpak/$APP_URN.yml-template

        # Move doc folder to correct location
        mv "./debian/usr/share/doc/APP_NAME/" "./debian/usr/share/doc/$APP_NAME_LC/"

        # Write commit messages since previous tag into changelog file
        # Check if current directory is a git repository
        if [ ! -d "$PROJECT_ROOT/.git" ]; then
            echo -en "\033[0;31m"
            echo "! Not a git repository, skipping changelog generation."
            echo -en "\033[0m"
        else
            # Get previous to last git tag
            prev_tag=$(git tag -l 2> /dev/null | sed 's/v//' | sort -uVr | sed -n 2p)
            $(git log --pretty=format:"%ad %an%n%s%n" --date=short --no-merges v$prev_tag..HEAD 2> /dev/null) > ./changelog
        fi

        gzip --best -n ./changelog
        mv changelog.gz ./flatpak/app/share/doc/$APP_NAME_LC/
        rm -f ./changelog

        # Replace placeholders in desktop config, overwrite existing file
        cat ./debian/usr/share/applications/APP_URN.desktop \
            | sed "s/APP_NAME/$APP_NAME/"\
            | sed "s/APP_URN/$APP_URN/"\
            | sed "s/APP_DESC_SHORT/$APP_DESC_SHORT/"\
            | sed "s/APP_ARCH/$(echo $1 | sed 's/linux-//')/" > ./debian/usr/share/applications/$APP_URN.desktop
        
        rm ./debian/usr/share/applications/APP_URN.desktop

        # Move icon to correct location
        mv "./debian/usr/share/icons/hicolor/512x512/apps/APP_URN.png" "./debian/usr/share/icons/hicolor/512x512/apps/$APP_URN.png"

        # Create Flatpak package, stop publishing on error
        flatpak-builder --repo=./flatpak/.repo --force-clean ./flatpak/.build ./flatpak/$APP_URN.yml 2> /dev/null || abort
        flatpak build-bundle ./flatpak/.repo $PUBLISH_OUTPUT/$APP_NAME.$APP_VERSION.$1.flatpak $APP_URN || abort

        rm -rf ./flatpak ./flatpak-builder

        cd $APPLICATION_DIR
    else
        echo -en "\033[0;31m"
        echo "! For Flatpak package generation, install 'flatpak-builder' with 'apt'."
        echo -en "\033[0m"
    fi
}

# Platforms to target
if [ -z "$1" ]; then
    PLATFORMS=$PUBLISH_PLATFORMS
else
    PLATFORMS=$1
fi

# Set version from first argument
# or get latest tag from git and remove the "v" prefix
if [ -z "$2" ]; then
    APP_VERSION=$((git describe --tags --abbrev=0 2> /dev/null || echo "0.0.1") | sed 's/v//')
else
    APP_VERSION=$2
fi

# Output version to console
echo -n "Publishing $APP_NAME "
echo -en "\033[0;32m"
echo $APP_VERSION
echo -en "\033[0m"

echo -n "Selected platforms: $PLATFORMS"
echo -en "\033[0m"

echo "" # New line
echo "" # New line
echo -n "continue? (y/n [y]) "

# Read user input without needing to press enter
read -n 1 -s input

echo "" # New line

# If user input is not "Y", "y" or "" exit
if [[ ! $input =~ ^[Yy]$ ]] && [ ! -z "$input" ]; then
    abort
fi

# CD into application directory
cd $APPLICATION_DIR

# Check if we are in the right directory
if [ ! -f "$APP_NAME.csproj" ]; then
    echo -en "\033[0;31m"
    echo "❌ $APP_NAME.csproj not found!"
    abort
fi

# Iterate through each platform and publish
for PLATFORM in $PLATFORMS; do
    # Clearing previous builds
    header "Clearing previous builds..."
    dotnet clean 1> /dev/null || abort
    rm -rf ./bin

    header "Publishing for $PLATFORM..."
    dotnet publish -r $PLATFORM -c $PUBLISH_CONFIG -p:Version=$APP_VERSION 1> /dev/null || abort

    # Create zip files for Windows
    if [[ $PLATFORM == win-* ]]; then
        create_win_package $PLATFORM
    fi

    # Additional steps for macOS to create a .app bundle
    if [[ $PLATFORM == osx-* ]]; then
        create_mac_app_bundle $PLATFORM
    fi

    # Copy and prepare elements for Linux (Debian Package and Flatpak)
    if [[ $PLATFORM == linux-* ]]; then
        create_deb_package $PLATFORM
        create_flatpack_package $PLATFORM
    fi

    rm -rf $PUBLISH_BUILD/$APP_NAME.$APP_VERSION.$PLATFORM
done

header "Build and packaging complete."
