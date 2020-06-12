using System;
using System.Collections.Generic;
using System.Text;

namespace DbTransformer.Services
{
    public class MessageBox : IMessageBox
    {
        public void ShowMessage(string message)
        {
            System.Windows.MessageBox.Show(message);
        }
    }
}
