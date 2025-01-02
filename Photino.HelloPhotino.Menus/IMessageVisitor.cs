namespace Photino.HelloPhotino.Menus;

public interface IMessageVisitor
{
    void Visit(OpenMenuMessage message);
    void Visit(OpenMenuResponse message);
}