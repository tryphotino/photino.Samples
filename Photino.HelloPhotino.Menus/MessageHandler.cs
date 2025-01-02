using System;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Threading.Tasks;
using Photino.NET;

namespace Photino.HelloPhotino.Menus;

public sealed class MessageHandler : IMessageVisitor
{
    private readonly PhotinoWindow _window;

    public MessageHandler(PhotinoWindow window)
    {
        _window = window;
    }

    public void Visit(OpenMenuMessage message)
    {
        _ = OpenMenu(message);
    }

    public void Visit(OpenMenuResponse message)
    {
        Console.WriteLine("Received unexpected message.");
    }

    private async Task OpenMenu(OpenMenuMessage message)
    {
        try
        {
            using var menu = message.MenuDescriptor?.ToMenu(_window);

            if (menu == null)
            {
                Console.WriteLine("Failed to build menu.");
                return;
            }

            var selectedItem = await menu.Show(message.X, message.Y);

            var response = JsonSerializer.Serialize(new OpenMenuResponse
            {
                Id = selectedItem?.Id
            }, SerializerContext.Default.OpenMenuResponse);

            await _window.SendWebMessageAsync(response);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed to open menu:{0}{1}", Environment.NewLine, exception);
        }
    }
}