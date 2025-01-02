using System.Text.Json.Serialization;

namespace Photino.HelloPhotino.Menus;

[JsonSerializable(typeof(IMenuChildDescriptor))]
[JsonSerializable(typeof(IMessage))]
[JsonSerializable(typeof(MenuDescriptor))]
[JsonSerializable(typeof(MenuItemDescriptor))]
[JsonSerializable(typeof(MenuSeparatorDescriptor))]
[JsonSerializable(typeof(OpenMenuMessage))]
[JsonSerializable(typeof(OpenMenuResponse))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public sealed partial class SerializerContext : JsonSerializerContext;