using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TibiaEzBot.View
{
    /// <summary>
    /// Interaction logic for DelayWindow.xaml
    /// </summary>
    public partial class DelayWindow : Window
    {
        public DelayWindow()
        {
            InitializeComponent();
        }

        private void uxAddButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            uxDelayNumericTextBox.PreviewKeyDown += new KeyEventHandler(uxDelayNumericTextBox_PreviewKeyDown);
            uxDelayNumericTextBox.Focus();
        }

        void uxDelayNumericTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                uxAddButton_Click(this, new RoutedEventArgs());
                e.Handled = true;
            }
        }
    }
}
