using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace TibiaEzBot.View.Controls
{
    public class NumberTextBox : TextBox
    {
        private bool IsNumeric(string str)
        {
            bool ret = true;

            int l = str.Length;
            for (int i = 0; i < l; i++)
            {
                char ch = str[i];
                ret &= Char.IsDigit(ch);
            }

            return ret;
        }


        protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsNumeric(e.Text);
            base.OnPreviewTextInput(e); 
        }

        public int Number
        {
            get
            {
                int n;
                Int32.TryParse(base.Text, out n);
                return n;
            }
            set
            {
                this.Text = value.ToString();
            }
        }


    }
}
