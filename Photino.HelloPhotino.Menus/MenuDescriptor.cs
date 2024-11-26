using System.Collections.Generic;

namespace Photino.HelloPhotino.Menus;

public sealed class MenuDescriptor
{
    public IEnumerable<IMenuChildDescriptor> Children { get; set; } = [];
}