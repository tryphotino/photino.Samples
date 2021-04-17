using Microsoft.Windows.Sdk;
using System;
using System.Runtime.InteropServices;

namespace HelloPhotino.AdvancedNET
{
    public class CsWin32
    {
        public static bool OpenOrCreateFile(string fullPath)
        {
            var sH = PInvoke.CreateFile(fullPath, 
                FILE_ACCESS_FLAGS.FILE_GENERIC_READ | FILE_ACCESS_FLAGS.FILE_GENERIC_WRITE,
                FILE_SHARE_FLAGS.FILE_SHARE_NONE,
                null,
                FILE_CREATE_FLAGS.OPEN_EXISTING,
                FILE_FLAGS_AND_ATTRIBUTES.FILE_ATTRIBUTE_NORMAL,
                null);

            return !sH.IsInvalid;
        }

        public static string MyGetOpenFileName(string initialDirectory = null, string filter = null, string defaultExtension = null)
        {
            var fileName = string.Empty;

            unsafe
            {
                fixed (char* file = new char[100])
                {
                    var request = new OPENFILENAMEW
                    {
                        //lStructSize = (uint)sizeof(OPENFILENAMEW),

                        lpstrInitialDir = null,
                        lpstrFilter = null,
                        lpstrDefExt = null,

                        lpstrFile = file,   //
                        nMaxFile = 100,     //

                        hwndOwner = new HWND(),
                        hInstance = new HINSTANCE(),
                        Flags = 0,
                        FlagsEx = 0,
                        lpstrCustomFilter = null,
                        lpstrTitle = null,
                        lCustData = new LPARAM(),
                        lpfnHook = null,    //delegate*
                        lpstrFileTitle = null,
                        lpTemplateName = null,
                        nFileExtension = 0,
                        nFileOffset = 0,
                        nFilterIndex = 0,
                        nMaxCustFilter = 0,
                        nMaxFileTitle = 0,
                        dwReserved = 0,
                        pvReserved = (void*)IntPtr.Zero,  //void*
                    };

                    request.lStructSize = (uint)Marshal.SizeOf<OPENFILENAMEW>();

                    if (PInvoke.GetOpenFileName(ref request))
                        fileName = new string(request.lpstrFile);
                    else
                        fileName = $"Dlg Error: {PInvoke.CommDlgExtendedError()}";
                }
            }

            return fileName;
        }




        public static int GetTickCount()
        {
            return (int)PInvoke.GetTickCount();
        }
    }
}
