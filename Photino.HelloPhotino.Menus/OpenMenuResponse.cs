namespace Photino.HelloPhotino.Menus;

public sealed class OpenMenuResponse : IMessage
{
    public int? Id { get; set; }

    public void Accept(IMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}