using System.Text.Json.Serialization;

namespace Photino.HelloPhotino.Menus;

[JsonDerivedType(typeof(OpenMenuMessage), typeDiscriminator: nameof(OpenMenuMessage))]
public interface IMessage
{
    void Accept(IMessageVisitor visitor);
}