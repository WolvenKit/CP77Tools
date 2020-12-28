using ControlzEx.Theming;
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
    /// Interaction logic for AboutPage.xaml
    /// </summary>
    public partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();
            ThemeManager.Current.ChangeTheme(this, "Dark.Steel");
            AboutTextBlock.Text = "CP77Tools Tool by Rfuzzo. \n " + "\n" + "CP77Tools UI by R503B(Offline)" + "\n" + "\n" + "\n" + "Modding tools for the cyberpunk 2077 game. \n" + "\n" + " - requires NET5.0 \n" + "\n" + "The cp77 tools require the oo2ext_7_win64.dll to work.Copy and paste the dll into the cp77Tools folder.\n" + "\n" + "If you are building from source, the dll needs to be in the same folder as the build.exe, e.g.C:\\cpmod\\CP77Tools\\CP77Tools\\bin\\Debug\\net5.0\\oo2ext_7_win64.dll \n" + "\n" + "It can be found here: Cyberpunk 2077\\bin\\x64\\oo2ext_7_win64.dll \n" + "\n" +
                 "\n";
        }
    }
}
