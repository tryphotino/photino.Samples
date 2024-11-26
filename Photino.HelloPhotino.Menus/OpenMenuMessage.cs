namespace Photino.HelloPhotino.Menus;

public sealed class OpenMenuMessage : IMessage
{
    public MenuDescriptor? MenuDescriptor { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public void Accept(IMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}