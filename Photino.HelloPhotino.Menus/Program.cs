using System;
using System.IO;
using System.Text.Json;
using Photino.NET;

namespace Photino.HelloPhotino.Menus;

public static class Program
{
    private static MessageHandler _handler;
    private const string _indexPath = "UserInterface/dist/index.html";

    [STAThread]
    public static int Main()
    {
        var indexHtmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _indexPath);

        if (!File.Exists(indexHtmlPath))
        {
            Console.WriteLine($"Couldn't find '{indexHtmlPath}'. Did you execute 'npm run build'?");
            return -1;
        }

        using var reader = new StreamReader(indexHtmlPath);
        var window = new PhotinoWindow();
        _handler = new MessageHandler(window);

        window
            .RegisterWebMessageReceivedHandler(HandleMessage)
            .LoadRawString(reader.ReadToEnd())
            .WaitForClose();

        return 0;
    }

    private static void HandleMessage(object? sender, string message)
    {
        IMessage? messageObject;

        try
        {
            messageObject = JsonSerializer.Deserialize(message, SerializerContext.Default.IMessage);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed to deserialize message:{0}{1}", Environment.NewLine, exception);
            return;
        }

        if (messageObject == null)
        {
            Console.WriteLine("Received an invalid message.");
        }
        else
        {
            try
            {
                messageObject.Accept(_handler);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Failed to process message:{0}{1}", Environment.NewLine, exception);
            }
        }
    }
}