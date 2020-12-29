using ControlzEx.Theming;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
    public partial class SettingsPage : Page
    {
        public string localaccent;
        public SettingsPage()
        {
            InitializeComponent();
            ThemeManager.Current.ChangeTheme(this, SUI.sui.generaldata.ThemeFinder());
            AccentBox.ItemsSource = SUI.sui.generaldata.Colors2;

           
             



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
                    ThemeManager.Current.ChangeTheme(SUI.sui, SUI.sui.generaldata.ThemeHelper("Light."+ localaccent));

                    ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
                    ThemeManager.Current.SyncTheme();

                 
                }
                else
                {
                    ThemeManager.Current.ChangeTheme(SUI.sui, SUI.sui.generaldata.ThemeHelper("Dark." + localaccent));

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

                ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
                ThemeManager.Current.SyncTheme();
            }
        }
    }
}
