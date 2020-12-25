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

namespace CP77Tools.UI.Views
{
    /// <summary>
    /// Interaction logic for Tools.xaml
    /// </summary>
    public partial class Tools : Page
    {
        public Tools()
        {
            InitializeComponent();
            ThemeManager.Current.ChangeTheme(this, "Dark.Steel");
            LoadCollectionData();
            ArchiveTaskConceptGrid.ItemsSource = JustTesting;



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TabItem NewTask = new TabItem();
            NewTask.Header = "Concept Task";
            SubTabs.Items.Add(NewTask);

            TabItem Current = (TabItem)SubTabs.Items[0];
            Current.Header = "Next";


            ArchiveTaskConceptGrid.CanUserAddRows = true;


        }

        public Dictionary<string, string> JustTesting = new Dictionary<string, string>();

        private void LoadCollectionData()
        {
            JustTesting.Add("Selected Outpath", "None");
            JustTesting.Add("Extract", "Disabled");
            JustTesting.Add("List", "Disabled");
            JustTesting.Add("Dump", "Disabled");
            JustTesting.Add("Uncook", "Disabled");
            JustTesting.Add("Uncook File Extension", "Default (TGA)");
            JustTesting.Add("Selected", "None");



        }
    }







}
