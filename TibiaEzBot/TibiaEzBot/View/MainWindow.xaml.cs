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
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Reflection;
using System.IO;
using Microsoft.Win32;
using System.Threading;
using TibiaEzBot.Core.Configs;
using TibiaEzBot.Core;

namespace TibiaEzBot.View
{

    public partial class MainWindow : Window
    {
        public bool PreventClose { get; set; }
        public System.Windows.Forms.NotifyIcon NotifyIcon { get; set; }
        private System.Windows.Forms.MenuItem uxHideShowMenuItem;
		//private PathFinder.PathFinderView pathFinder = new PathFinder.PathFinderView();

        public MainWindow()
        {
            ClientChooser clientChooser = new ClientChooser();
            clientChooser.ShowDialog();

            if (clientChooser.SelectedClient != null)
            {
                Core.Kernel.GetInstance().Client = clientChooser.SelectedClient;
                Core.Kernel.GetInstance().Initialize();
            }
            else
            {
                MessageBox.Show("You need to select a valid\ntibia client or open a new one.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
                return;
            }

            InitializeComponent();

            NotifyIcon = new System.Windows.Forms.NotifyIcon();
            NotifyIcon.Icon = Properties.Resources.Icon;
            NotifyIcon.Text = "TibiaEzBot";

            NotifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu();
            uxHideShowMenuItem = new System.Windows.Forms.MenuItem("Hide", new EventHandler(uxHideShowMenuItem_Click));

            NotifyIcon.ContextMenu.MenuItems.Add(uxHideShowMenuItem);
            NotifyIcon.ContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("-"));
            NotifyIcon.ContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Exit", new EventHandler(uxExitMenuItem_Click)));

            NotifyIcon.Visible = true;

            uxWaypointsListView.ItemsSource = Core.Kernel.GetInstance().AutoWalk.Waypoints;
            Core.Kernel.GetInstance().AutoWalk.CurrentWaypointChanged += new EventHandler(AutoWalk_CurrentWaypointChanged);

            Core.Kernel.GetInstance().Client.Exited += new EventHandler(Client_Exited);
			
			//pathFinder.Show();
			
            PreventClose = true;
        }

        private void uxHideShowMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                Hide();
                uxHideShowMenuItem.Text = "Show";
            }
            else
            {
                Show();
                uxHideShowMenuItem.Text = "Hide";
            }
        }

        private void uxExitMenuItem_Click(object sender, EventArgs e)
        {
            if (Core.Kernel.GetInstance().Client.LoggedIn)
            {
                if (System.Windows.Forms.MessageBox.Show("If you close the TibiaEzBot your character will be kicked.\nDo you really want close?",
                    "Warning", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question,
                    System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            PreventClose = false;
            Close();
        }

        #region Event Handlers

        private void Client_Exited(object sender, EventArgs e)
        {
            if (Dispatcher.Thread != Thread.CurrentThread)
            {
                Dispatcher.BeginInvoke(new Action<object, EventArgs>(Client_Exited), new object[] { sender, e });
                return;
            }

            PreventClose = false;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.HideAllExpanders();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (PreventClose)
            {
                e.Cancel = true;
                uxHideShowMenuItem.Text = "Show";
                Hide();
            }
            else
            {
                NotifyIcon.Visible = false;
                Core.Kernel.GetInstance().Shutdown();
            }
        }

        #endregion

        #region Load/Save

        private void uxLoadMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files|*.xml";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog(this) == true)
            {
                TibiaEzBot.Core.Configs.ConfigManager.LoadConfig(this, openFileDialog.FileName);
            }
        }

        private void uxSaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML Files|*.xml";

            if (saveFileDialog.ShowDialog(this) == true)
            {
                TibiaEzBot.Core.Configs.ConfigManager.SaveConfig(this, saveFileDialog.FileName);
            }
        }

        #endregion

        #region Expanders

        private void HideAllExpanders()
        {
            uxAutoLoginExpander.Visibility = Visibility.Collapsed;
            uxCaveHuntingExpander.Visibility = Visibility.Collapsed;
            uxManaRestoreExpander.Visibility = Visibility.Collapsed;
            uxManaTrainingExpander.Visibility = Visibility.Collapsed;
            uxRuneMakerExpander.Visibility = Visibility.Collapsed;
            uxSelfHealingExpander.Visibility = Visibility.Collapsed;
        }

        public void ShowExpander(Expander expander)
        {
            uxMainStackPanel.Children.Remove(expander);
            uxMainStackPanel.Children.Add(expander);
            expander.Visibility = Visibility.Visible;
        }

        public void HideExpander(Expander expander)
        {
            expander.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region AutoHeal

        private void uxSelfHealingMenuItem_Checked(object sender, RoutedEventArgs e)
        {
            ShowExpander(uxSelfHealingExpander);
        }

        private void uxSelfHealingMenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
            HideExpander(uxSelfHealingExpander);
        }

        private void uxHealingSpellsEnableCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            uxHealingSpellsMinimumTextBox.IsEnabled = false;
            uxHealingSpellsManaTextBox.IsEnabled = false;
            uxHealingSpellTextBox.IsEnabled = false;

            Core.Kernel.GetInstance().AutoHeal.MagicMinimumHealth = uxHealingSpellsMinimumTextBox.Number;
            Core.Kernel.GetInstance().AutoHeal.MagicMinimumMana = uxHealingSpellsManaTextBox.Number;
            Core.Kernel.GetInstance().AutoHeal.MagicWords = uxHealingSpellTextBox.Text;

            Core.Kernel.GetInstance().AutoHeal.MagicEnable = true;
        }

        private void uxHealingSpellsEnableCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoHeal.MagicEnable = false;

            uxHealingSpellsMinimumTextBox.IsEnabled = true;
            uxHealingSpellsManaTextBox.IsEnabled = true;
            uxHealingSpellTextBox.IsEnabled = true;
        }

        private void uxHealingPotionsEnableCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            uxMinimumHealthPotionTextBox.IsEnabled = false;
            uxTypePotionComboBox.IsEnabled = false;

            Core.Kernel.GetInstance().AutoHeal.PotionItemNumber = Int32.Parse((String)((ComboBoxItem)uxTypePotionComboBox.SelectedItem).Tag);
            Core.Kernel.GetInstance().AutoHeal.PotionMinimumHealth = uxMinimumHealthPotionTextBox.Number;
            Core.Kernel.GetInstance().AutoHeal.PotionEnable = true;
        }

        private void uxHealingPotionsEnableCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoHeal.PotionEnable = false;

            uxMinimumHealthPotionTextBox.IsEnabled = true;
            uxTypePotionComboBox.IsEnabled = true;
        }

        #endregion

        #region Restore Mana

        private void uxManaRestoreMenuItem_Checked(object sender, RoutedEventArgs e)
        {
            ShowExpander(uxManaRestoreExpander);
        }

        private void uxManaRestoreMenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
            HideExpander(uxManaRestoreExpander);
        }

        private void uxManaRestoreEnableCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            uxMinimumManaRestoreTextBox.IsEnabled = false;
            uxItemManaRestoreCheckBox.IsEnabled = false;

            Core.Kernel.GetInstance().AutoManaRestore.MinimumMana = uxMinimumManaRestoreTextBox.Number;
            Core.Kernel.GetInstance().AutoManaRestore.ItemId = Int32.Parse((String)((ComboBoxItem)uxItemManaRestoreCheckBox.SelectedItem).Tag);

            Core.Kernel.GetInstance().AutoManaRestore.Enable = true;
        }

        private void uxManaRestoreEnableCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoManaRestore.Enable = false;

            uxMinimumManaRestoreTextBox.IsEnabled = true;
            uxItemManaRestoreCheckBox.IsEnabled = true;
        }

        #endregion

        #region AutoLogin

        private void uxAutoLoginMenuItem_Checked(object sender, RoutedEventArgs e)
        {
            ShowExpander(uxAutoLoginExpander);
        }

        private void uxAutoLoginMenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
            HideExpander(uxAutoLoginExpander);
        }

        private void uxAutoLoginEnableCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            uxLoginTextBox.IsEnabled = false;
            uxPasswordTextBox.IsEnabled = false;
            uxCharacterNameTextBox.IsEnabled = false;

            Core.Kernel.GetInstance().AutoLogin.Account = uxLoginTextBox.Text;
            Core.Kernel.GetInstance().AutoLogin.Password = uxPasswordTextBox.Text;
            Core.Kernel.GetInstance().AutoLogin.CharacterName = uxCharacterNameTextBox.Text;

            Core.Kernel.GetInstance().AutoLogin.Enable = true;
        }

        private void uxAutoLoginEnableCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoLogin.Enable = false;

            uxLoginTextBox.IsEnabled = true;
            uxPasswordTextBox.IsEnabled = true;
            uxCharacterNameTextBox.IsEnabled = true;
        }

        #endregion

        #region AutoAttack

        private void uxCaveHuntingMenuItem_Checked(object sender, RoutedEventArgs e)
        {
            ShowExpander(uxCaveHuntingExpander);
        }

        private void uxCaveHuntingMenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
            HideExpander(uxCaveHuntingExpander);
        }

        private void uxAttackMonstersCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoAttack.Enable = true;
            uxTargetAllCheckBox.IsEnabled = true;
        }

        private void uxAttackMonstersCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            uxTargetAllCheckBox.IsChecked = false;
            uxTargetAllCheckBox.IsEnabled = false;
            Core.Kernel.GetInstance().AutoAttack.Enable = false;
        }

        private void uxTargetAllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoAttack.TargetAll = true;
        }

        private void uxTargetAllCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoAttack.TargetAll = false;
        }

        #endregion

        #region AutoWalk
        private void uxWaypointsGroundContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoWalk.AddWaypoint(TibiaEzBot.Core.Entities.WaypointType.WAYPOINT_GROUND);
        }

        private void uxFollowWaypointsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            uxWaypointsListView.ContextMenu = null;
            Core.Kernel.GetInstance().AutoWalk.Enable = true;
        }

        private void uxFollowWaypointsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            uxWaypointsListView.ContextMenu = uxWaypointsContextMenu;
            Core.Kernel.GetInstance().AutoWalk.Enable = false;
        }

        private void uxWaypointsHoleContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoWalk.AddWaypoint(TibiaEzBot.Core.Entities.WaypointType.WAYPOINT_HOLE);
        }

        private void uxWaypointsRopeContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoWalk.AddWaypoint(TibiaEzBot.Core.Entities.WaypointType.WAYPOINT_ROPE);
        }

        private void uxWaypointsLadderContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoWalk.AddWaypoint(TibiaEzBot.Core.Entities.WaypointType.WAYPOINT_LADDER);
        }

        private void uxWaypointsUpStairsContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoWalk.AddWaypoint(TibiaEzBot.Core.Entities.WaypointType.WAYPOINT_STAIR_UP);
        }

        private void uxWaypointsDownStairsContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoWalk.AddWaypoint(TibiaEzBot.Core.Entities.WaypointType.WAYPOINT_START_DOWN);
        }

        private void uxWaypointsSayContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SayWindow sayWindow = new SayWindow();
            if (sayWindow.ShowDialog() == true)
            {
                Core.Kernel.GetInstance().AutoWalk.AddWaypoint(sayWindow.uxWordsTextBox.Text, 
                    (TibiaEzBot.Core.Entities.SayType)Byte.Parse((String)((ComboBoxItem)sayWindow.uxSayTypeComboBox.SelectedItem).Tag));
            }
        }

        private void uxWaypointsDelayContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DelayWindow delayWindow = new DelayWindow();
            if (delayWindow.ShowDialog() == true)
            {
                Core.Kernel.GetInstance().AutoWalk.AddWaypoint(delayWindow.uxDelayNumericTextBox.Number);
            }
        }


        private void uxWaypointsMoveUpContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoWalk.MoveUp(uxWaypointsListView.SelectedIndex);
        }

        private void uxWaypointsMoveDownContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoWalk.MoveDown(uxWaypointsListView.SelectedIndex);
        }

        private void uxWaypointsUpContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoWalk.AddWaypoint(TibiaEzBot.Core.Constants.Direction.Up);
        }

        private void uxWaypointsDownContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoWalk.AddWaypoint(TibiaEzBot.Core.Constants.Direction.Down);
        }

        private void uxWaypointsLeftContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoWalk.AddWaypoint(TibiaEzBot.Core.Constants.Direction.Left);
        }

        private void uxWaypointsRightContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoWalk.AddWaypoint(TibiaEzBot.Core.Constants.Direction.Right);
        }

        private void uxUseLightShovelCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoWalk.UseLightShovel = true;
        }

        private void uxUseLightShovelCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoWalk.UseLightShovel = false;
        }

        private void uxUseElvenhairRopeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoWalk.UseElvenhairRope = true;
        }

        private void uxUseElvenhairRopeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoWalk.UseElvenhairRope = false;            
        }

        private void AutoWalk_CurrentWaypointChanged(object sender, EventArgs e)
        {
            if (Dispatcher.Thread != Thread.CurrentThread)
            {
                Dispatcher.BeginInvoke(new Action<object, EventArgs>(AutoWalk_CurrentWaypointChanged), new object[] { sender, e });
                return;
            }

            uxWaypointsListView.SelectedIndex = Core.Kernel.GetInstance().AutoWalk.CurrentWaypoint;
        }

        private void uxWaypointsClearContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoWalk.Clear();
        }

        private void uxWaypointsRemoveContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Core.Kernel.GetInstance().AutoWalk.Remove(uxWaypointsListView.SelectedIndex);
        }

        private void uxWaypointsSaveContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML Files|*.xml";

            if (saveFileDialog.ShowDialog(this) == true)
            {
                TibiaEzBot.Core.Configs.ConfigManager.SaveWaypoints(saveFileDialog.FileName);
            }
        }

        private void uxWaypointsLoadContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files|*.xml";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog(this) == true)
            {
                TibiaEzBot.Core.Configs.ConfigManager.LoadWaypoints(openFileDialog.FileName);
            }
        }
        #endregion
    }
}
