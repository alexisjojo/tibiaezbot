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
    /// Interaction logic for SayWindow.xaml
    /// </summary>
    public partial class SayWindow : Window
    {
        public SayWindow()
        {
            InitializeComponent();
        }

        private void uxAddButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
