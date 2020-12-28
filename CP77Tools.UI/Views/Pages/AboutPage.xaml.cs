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
    /// Interaction logic for AboutPage.xaml
    /// </summary>
    public partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();
            ThemeManager.Current.ChangeTheme(this, "Dark.Steel");


        }
       

        private void Githublink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo(Githublink.Text.ToString()) { UseShellExecute = true });

        }

        private void CommunityLink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo(CommunityLink.Text.ToString()) { UseShellExecute = true });

        }

        private void ModdingToolsDiscordLink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo(ModdingToolsDiscordLink.Text.ToString()) { UseShellExecute = true });

        }

        private void Githublink_MouseEnter(object sender, MouseEventArgs e)
        {
            Githublink.TextDecorations = TextDecorations.Underline;
        }

        private void Githublink_MouseLeave(object sender, MouseEventArgs e)
        {
            Githublink.TextDecorations = null;

        }

        private void CommunityLink_MouseEnter(object sender, MouseEventArgs e)
        {
            CommunityLink.TextDecorations = TextDecorations.Underline;

        }

        private void CommunityLink_MouseLeave(object sender, MouseEventArgs e)
        {
            CommunityLink.TextDecorations = null;

        }

        private void ModdingToolsDiscordLink_MouseEnter(object sender, MouseEventArgs e)
        {
            ModdingToolsDiscordLink.TextDecorations = TextDecorations.Underline;

        }

        private void ModdingToolsDiscordLink_MouseLeave(object sender, MouseEventArgs e)
        {
            ModdingToolsDiscordLink.TextDecorations = null;

        }
    }
}
