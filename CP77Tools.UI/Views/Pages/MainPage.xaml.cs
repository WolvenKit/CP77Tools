using ControlzEx.Theming;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            ThemeManager.Current.ChangeTheme(this, SUI.sui.generaldata.ThemeFinder());
        }




        private void Githublink_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo(Githublink.Text.ToString()) { UseShellExecute = true });

        }

        private void ModdingToolsDiscordLink_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo(ModdingToolsDiscordLink.Text.ToString()) { UseShellExecute = true });

        }

        private void CommunityLink_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo(CommunityLink.Text.ToString()) { UseShellExecute = true });

        }
    }
}
