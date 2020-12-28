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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            ThemeManager.Current.ChangeTheme(this, "Dark.Steel");
            MainUpdateInfoTextBlock.Text = "Update: Version 0.5.4 - Beta\n" +
                "\n" +
                "- WIP.\n" +
                "- " +
                "Todo : \n" +
                "Preset Preparation.\n" +
                "- \n" +
                "\n" +
                "\n" +
                "\n" +
                "\n" +
                "\n";
        }
    }
}
