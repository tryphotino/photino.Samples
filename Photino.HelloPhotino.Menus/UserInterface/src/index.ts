interface MenuDescriptorChild {
    $type: 'MenuItemDescriptor' | 'MenuSeparatorDescriptor'
}

interface MenuDescriptor {
    $type: 'MenuDescriptor';
    children?: MenuItemDescriptor[];
}

interface MenuItemDescriptor extends MenuDescriptorChild {
    $type: 'MenuItemDescriptor';
    children?: MenuItemDescriptor[];
    id: number;
    label?: string;
}

interface MenuSeparatorDescriptor extends MenuDescriptorChild {
    $type: 'MenuSeparatorDescriptor'
}

interface OpenMenuMessage {
    $type: 'OpenMenuMessage';
    menuDescriptor: MenuDescriptor;
    x: number;
    y: number;
}

interface MenuSelectedMessage {
    $type: 'MenuSelectedMessage';
    id: number;
}

enum Color {
    Red = 1,
    Green = 2,
    Blue = 3,
    Yellow = 4,
    Cyan = 5,
    Magenta = 6,
    Orange = 7,
    Chartreuse = 8,
    SpringGreen = 9,
    Azure = 10,
    Violet = 11,
    Rose = 12
}

// See: https://en.wikipedia.org/wiki/Secondary_color#RGB_and_CMYK
const colors = {
    [Color.Red]: '#ff0000',
    [Color.Green]: '#00ff00',
    [Color.Blue]: '#0000ff',
    [Color.Yellow]: '#ffff00',
    [Color.Cyan]: '#00ffff',
    [Color.Magenta]: '#ff00ff',
    [Color.Orange]: '#ff8000',
    [Color.Chartreuse]: '#80ff00',
    [Color.SpringGreen]: '#00ff80',
    [Color.Azure]: '#0080ff',
    [Color.Violet]: '#8000ff',
    [Color.Rose]: '#ff0080'
};

window.addEventListener('contextmenu', event => {
    event.preventDefault();

    const message: OpenMenuMessage = {
        $type: 'OpenMenuMessage',
        menuDescriptor: {
            $type: 'MenuDescriptor',
            children: [
                {
                    $type: 'MenuItemDescriptor',
                    id: 0,
                    label: 'Primary',
                    children: [
                        { $type: 'MenuItemDescriptor', id: Color.Red, label: 'Red' },
                        { $type: 'MenuItemDescriptor', id: Color.Green, label: 'Green' },
                        { $type: 'MenuItemDescriptor', id: Color.Blue, label: 'Blue' }
                    ]
                },
                {
                    $type: 'MenuItemDescriptor',
                    id: 0,
                    label: 'Secondary',
                    children: [
                        { $type: 'MenuItemDescriptor', id: Color.Yellow, label: 'Yellow' },
                        { $type: 'MenuItemDescriptor', id: Color.Cyan, label: 'Cyan' },
                        { $type: 'MenuItemDescriptor', id: Color.Magenta, label: 'Magenta' }
                    ]
                },
                {
                    $type: 'MenuItemDescriptor',
                    id: 0,
                    label: 'Tertiary',
                    children: [
                        { $type: 'MenuItemDescriptor', id: Color.Orange, label: 'Orange' },
                        { $type: 'MenuItemDescriptor', id: Color.Chartreuse, label: 'Chartreuse' },
                        { $type: 'MenuItemDescriptor', id: Color.SpringGreen, label: 'Spring Green' },
                        { $type: 'MenuItemDescriptor', id: Color.Azure, label: 'Azure' },
                        { $type: 'MenuItemDescriptor', id: Color.Violet, label: 'Violet' },
                        { $type: 'MenuItemDescriptor', id: Color.Rose, label: 'Rose' }
                    ]
                },
            ]
        },
        x: event.clientX,
        y: event.clientY
    };

    (window as any).external.sendMessage(JSON.stringify(message));
});

interface MenuOpenResponse {
    id: number;
}

(window as any).external.receiveMessage((message: string) => {
    const messageObj = JSON.parse(message) as MenuOpenResponse;
    const x = document.querySelector('body') as HTMLElement;
    x.style.backgroundColor = colors[messageObj.id as Color];
});