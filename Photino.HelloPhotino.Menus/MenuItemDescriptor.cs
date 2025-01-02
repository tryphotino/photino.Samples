using System.Collections.Generic;

namespace Photino.HelloPhotino.Menus;

public sealed class MenuItemDescriptor : IMenuChildDescriptor
{
    public IEnumerable<IMenuChildDescriptor> Children { get; set; } = [];
    public int Id { get; set; }
    public string? Label { get; set; }
}