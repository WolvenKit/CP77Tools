using ControlzEx.Theming;
using CP77Tools.UI.Data;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CP77Tools.UI.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        public string localaccent;
        public SettingsPage()
        {
            InitializeComponent();
            ThemeManager.Current.ChangeTheme(this, SUI.sui.generaldata.ThemeFinder());
            AccentBox.ItemsSource = SUI.sui.generaldata.Colors2;

            SUI.sui.generaldata.SettingsInstance = this;
        }


        private void ThemeDarkLightToggle_Toggled(object sender, RoutedEventArgs e)
        {
            string[] LightODark = SUI.sui.generaldata.CurrentTheme.Split('.');
            localaccent = LightODark[1];
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    ThemeManager.Current.ChangeTheme(SUI.sui, SUI.sui.generaldata.ThemeHelper("Light." + localaccent));
                    Properties.Settings.Default.Theme = "Light";
                    Properties.Settings.Default.ThemeAccent = localaccent;
                    Properties.Settings.Default.Save();////////////////
                    SUI.sui.tools.RefreshTheme();
                    ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
                    ThemeManager.Current.SyncTheme();
                }
                else
                {
                    ThemeManager.Current.ChangeTheme(SUI.sui, SUI.sui.generaldata.ThemeHelper("Dark." + localaccent));
                    Properties.Settings.Default.Theme = "Dark";
                    Properties.Settings.Default.ThemeAccent = localaccent;
                    Properties.Settings.Default.Save();////////////////
                    SUI.sui.tools.RefreshTheme();
                    ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
                    ThemeManager.Current.SyncTheme();
                }
            }
        }

        private void AccentBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] LightODark = SUI.sui.generaldata.CurrentTheme.Split('.');
            var localtheme = LightODark[0];
            ComboBox toggleSwitch = sender as ComboBox;
            if (toggleSwitch != null)
            {
                ThemeManager.Current.ChangeTheme(SUI.sui, SUI.sui.generaldata.ThemeHelper(localtheme + "." + AccentBox.SelectedItem));
                Properties.Settings.Default.Theme = localtheme;
                Properties.Settings.Default.ThemeAccent = (string)AccentBox.SelectedItem;
                ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
                ThemeManager.Current.SyncTheme();
            }
        }

        private void OutputDirToggle_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true) { Properties.Settings.Default.OpenOutDir = true; Properties.Settings.Default.Save(); }
                else { }
            }
        }

        private void DesktopPopupsToggle_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true) { Properties.Settings.Default.EnablePopUps = true; Properties.Settings.Default.Save(); }
                else { }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            OutputDirToggle.IsOn = Properties.Settings.Default.OpenOutDir;
            DesktopPopupsToggle.IsOn = Properties.Settings.Default.EnablePopUps;
            Defaultoutpathsettings.Text = Properties.Settings.Default.DefaultOutPath;
            AccentBox.SelectedItem = Properties.Settings.Default.ThemeAccent;
            if (Properties.Settings.Default.Theme == "Dark")
            { ThemeDarkLightToggle.IsOn = false; }
            else if (Properties.Settings.Default.Theme == "Light")
            { ThemeDarkLightToggle.IsOn = true; }
        }

        private void Defaultoutpathsettings_TextChanged(object sender, TextChangedEventArgs e) { if (Directory.Exists(Defaultoutpathsettings.Text)) { Properties.Settings.Default.DefaultOutPath = Defaultoutpathsettings.Text; } }
    }
}
