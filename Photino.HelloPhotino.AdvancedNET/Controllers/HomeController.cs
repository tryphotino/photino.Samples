using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management.Automation;
using System.Runtime.InteropServices;

namespace HelloWorld.AdvancedNET.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private List<string> _names = new List<string>();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            using (var psCommands = PowerShell.Create())
            {
                psCommands.AddCommand("Set-ExecutionPolicy")
                    .AddParameter("-ExecutionPolicy", "Bypass")
                    .Invoke();
            }
        }


        [HttpGet]
        public List<string> GetPrinters()
        {
            try
            {
                _names.Clear();
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    using (var psCommands = PowerShell.Create())
                    {
                        psCommands.AddCommand("Get-Printer");

                        var printers = psCommands.Invoke();

                        if (psCommands.HadErrors)
                        {
                            foreach (var error in psCommands.Streams.Error)
                                _names.Add(error.ErrorDetails.Message);

                            return _names;
                        }

                        foreach (var printer in printers)
                            _names.Add(printer.Members["Name"].Value.ToString());
                    }
                }
                else
                {
                    using (var p = new Process())
                    {
                        p.StartInfo = new ProcessStartInfo
                        {
                            FileName = "/bin/bash",
                            Arguments = "-c \"lpstat -a | cut -f1 -d ' '\"",
                            CreateNoWindow = true,
                            //ErrorDialog = false,
                            //RedirectStandardError = true,
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                        };
                        //p.ErrorDataReceived += (s, e) => { if (!string.IsNullOrEmpty(e.Data)) _names.Add(e.Data); };
                        p.OutputDataReceived += (s, e) => { if (!string.IsNullOrEmpty(e.Data)) _names.Add(e.Data); };
                        p.Start();
                        //p.BeginErrorReadLine();
                        p.BeginOutputReadLine();
                        p.WaitForExit(500);     //timeout is recommended by M$ and required on Linux for some reason
                        p.Close();
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogCritical(ex.Message);
                _names.Add(ex.Message);
            }

            return _names;
        }

        [HttpGet]
        public List<string> GetPowerShellCmdlets()
        {
            //Can use PowerShell across platforms to perform certain tasks 
            try
            {
                using (var psCommands = PowerShell.Create()
                    .AddCommand("Get-Command")
                    .AddParameter("Type", "Cmdlet"))
                {

                    var commands = psCommands.Invoke();

                    if (psCommands.HadErrors)
                    {
                        foreach (var error in psCommands.Streams.Error)
                            _names.Add(error.ErrorDetails.Message);

                        return _names;
                    }

                    foreach (var command in commands)
                        _names.Add(command.Members["Name"].Value.ToString());
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogCritical(ex.Message);
                _names.Add(ex.Message);
            }

            return _names;
        }

        [HttpPost]
        public List<string> RunPowerShellCmdlet([FromBody] dynamic data)
        {
            //Can use PowerShell across platforms to perform certain tasks 
            try
            {
                var parameters = JsonConvert.DeserializeObject(data.ToString());
                var command = parameters.command.Value;

                using (var psCommands = PowerShell.Create()
                    .AddCommand(command))
                {

                    var responses = psCommands.Invoke();

                    if (psCommands.HadErrors)
                    {
                        foreach (var error in psCommands.Streams.Error)
                            _names.Add(error.ErrorDetails.Message);

                        return _names;
                    }

                    foreach (var response in responses)
                        _names.Add(response.ToString());
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogCritical(ex.Message);
                _names.Add(ex.Message);
            }

            return _names;
        }

        [HttpGet]
        public string GetFile()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var fileDlg = new WindowsDialogs.OpenFileDialog
                {
                    Title = "Opening a file from Photino...",
                    Filter = "Text Files (*.txt)\0*.txt\0Log Files (*.log)\0*.log\0All Files (*.*)\0*.*\0",
                    InitialDirectory = null, //AppDomain.CurrentDomain.BaseDirectory,
                    DefaultExtension = "txt"
                };

                if (fileDlg.ShowDialog() == WindowsDialogs.DialogResult.OK)
                {
                    return fileDlg.File;
                }
                else
                    return "Didn't work";
            }
            else
                return "Only available on Windows at this time.";
        }

        [HttpGet]
        public string SaveFile()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var fileDlg = new WindowsDialogs.SaveFileDialog
                {
                    Title = "Saving a file from Photino...",
                    Filter = "Text Files (*.txt)\0*.txt\0Log Files (*.log)\0*.log\0All Files (*.*)\0*.*\0",
                    InitialDirectory = null, //AppDomain.CurrentDomain.BaseDirectory,
                    DefaultExtension = "txt"
                };

                if (fileDlg.ShowDialog() == WindowsDialogs.DialogResult.OK)
                {
                    return fileDlg.File;
                }
                else
                    return "Didn't work";
            }
            else
                return "Only available on Windows at this time.";
        }

        [HttpGet]
        public string GetColor()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var fileDlg = new WindowsDialogs.ColorPickerDialog();

                if (fileDlg.ShowDialog() == WindowsDialogs.DialogResult.OK)
                {
                    return fileDlg.Color.ToString();
                }
                else
                    return "Didn't work";
            }
            else
                return "Only available on Windows at this time.";
        }
    }
}
