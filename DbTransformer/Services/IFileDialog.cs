using System;
using System.Collections.Generic;
using System.Text;

namespace DbTransformer.Services
{
    public interface IFileDialog
    {
        string OpenFileDialog();
        string SaveFileDialog();
    }
}
