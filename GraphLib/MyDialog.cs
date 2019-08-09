using System.IO;
using GraphLib.Properties;
using Microsoft.WindowsAPICodePack.Dialogs;
// ReSharper disable UnusedMember.Global


namespace GraphLib
{
    public static class MyDialog
    {
        public static string GetFolder(bool useParent)
        {
            using (var dialog = new CommonOpenFileDialog { InitialDirectory = Settings.Default.RunPath, IsFolderPicker = true })
            {
                
                if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                    return null;

                if (!Directory.Exists(dialog.FileName))
                    return null;

                if (useParent)
                {
                    try
                    {
                        var v = Directory.GetParent(dialog.FileName);
                        Settings.Default.RunPath = v.FullName;
                    }
                    catch
                    {
                        Settings.Default.RunPath = dialog.FileName;
                    }
                   
                }
                else
                    Settings.Default.RunPath = dialog.FileName;
                Settings.Default.Save();
                return dialog.FileName;
            }
        }
    }
}
