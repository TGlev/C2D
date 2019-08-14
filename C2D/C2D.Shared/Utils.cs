using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace C2D.Shared
{
    public static class Utils
    {
        public static bool HasText(TextBox textBox)
        {
            if (textBox.Text == "" || textBox.Text == null)
                return false;
            return true;
        }

        public static bool HasText(PasswordBox textBox)
        {
            if (textBox.Password == "" || textBox.Password == null)
                return false;
            return true;
        }
    }
}
