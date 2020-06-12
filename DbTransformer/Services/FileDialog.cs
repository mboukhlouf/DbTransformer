using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbTransformer.Services
{
    public class FileDialog : IFileDialog
    {
        public string OpenFileDialog()
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result != true)
                return null;
            return dialog.FileName;
        }

        public string SaveFileDialog()
        {
            var dialog = new SaveFileDialog();
            var result = dialog.ShowDialog();
            if (result != true)
                return null;
            return dialog.FileName;
        }
    }
}
