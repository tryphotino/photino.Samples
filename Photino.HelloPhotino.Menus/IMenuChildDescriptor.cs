using System.Text.Json.Serialization;

namespace Photino.HelloPhotino.Menus;

[JsonDerivedType(typeof(MenuItemDescriptor), typeDiscriminator: nameof(MenuItemDescriptor))]
[JsonDerivedType(typeof(MenuSeparatorDescriptor), typeDiscriminator: nameof(MenuSeparatorDescriptor))]
public interface IMenuChildDescriptor;