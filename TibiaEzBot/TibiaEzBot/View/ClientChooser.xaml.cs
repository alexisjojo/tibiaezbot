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
using System.Threading;
using TibiaEzBot.Core.Entities;
using System.IO;

namespace TibiaEzBot.View
{
    /// <summary>
    /// Interaction logic for ClientChooser.xaml
    /// </summary>
    public partial class ClientChooser : Window
    {
        public Client SelectedClient { get; set; }

        public ClientChooser()
        {
            InitializeComponent();
        }

        public void LoadClients()
        {
            uxClients.Items.Clear();
            List<Client> clients = Client.GetClients();

            foreach (Client client in clients)
            {
                client.Exited += new EventHandler(client_Exited);
                this.uxClients.Items.Add(client);
            }


            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Tibia\tibia.exe")))
            {
                this.uxClients.Items.Add("New default client...");
            }

            this.uxClients.SelectedIndex = 0;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadClients();
        }

        private void client_Exited(object sender, EventArgs e)
        {
            if (Dispatcher.Thread != Thread.CurrentThread)
            {
                Dispatcher.BeginInvoke(new Action<object, EventArgs>(client_Exited), new Object[] { sender, e });
                return;
            }

            uxClients.Items.Remove(sender);
			this.uxClients.SelectedIndex = 0;
        }

        private void uxChoose_Click(object sender, RoutedEventArgs e)
        {
            if (this.uxClients.SelectedItem is Client)
                this.SelectedClient = (Client)uxClients.SelectedItem;
            else
            {
                if (this.uxClients.Items.Count > 1)
                    this.SelectedClient = Client.OpenMC();
                else
                    this.SelectedClient = Client.Open();
            }

            this.Close();
        }

        private void uxClients_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                LoadClients();
            }
        }
    }
}
