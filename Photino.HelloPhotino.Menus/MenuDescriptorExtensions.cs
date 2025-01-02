using Photino.NET;

namespace Photino.HelloPhotino.Menus;

public static class MenuDescriptorExtensions
{
    public static Menu ToMenu(this MenuDescriptor descriptor, PhotinoWindow window)
    {
        var result = new Menu(window);

        foreach (var child in descriptor.Children)
        {
            switch (child)
            {
                case MenuItemDescriptor menuItemDescriptor:
                {
                    result.Add(ToMenuItem(menuItemDescriptor));
                    break;
                }
                case MenuSeparatorDescriptor menuSeparatorDescriptor:
                {
                    result.Add(ToMenuSeparator(menuSeparatorDescriptor));
                    break;
                }
            }
        }

        return result;
    }

    private static MenuItem ToMenuItem(MenuItemDescriptor descriptor)
    {
        var result = new MenuItem(new MenuItemOptions
        {
            Id = descriptor.Id,
            Label = descriptor.Label
        });

        foreach (var child in descriptor.Children)
        {
            switch (child)
            {
                case MenuItemDescriptor menuItemDescriptor:
                {
                    result.Add(ToMenuItem(menuItemDescriptor));
                    break;
                }
                case MenuSeparatorDescriptor menuSeparatorDescriptor:
                {
                    result.Add(ToMenuSeparator(menuSeparatorDescriptor));
                    break;
                }
            }
        }

        return result;
    }

    private static MenuSeparator ToMenuSeparator(MenuSeparatorDescriptor _)
    {
        return new MenuSeparator();
    }
}