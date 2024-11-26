using System;
using System.IO;
using System.Runtime.Versioning;
using System.Text.Json;
using Photino.NET;

namespace Photino.HelloPhotino.Menus;

[SupportedOSPlatform("windows")]
public static class Program
{
    private static MessageHandler _handler;
    private const string _indexPath = "UserInterface/dist/index.html";

    [STAThread]
    public static int Main()
    {
        if (!File.Exists(_indexPath))
        {
            Console.WriteLine($"Couldn't find '{_indexPath}'. Did you execute 'npm run build'?");
            return -1;
        }

        using var reader = new StreamReader(_indexPath);
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