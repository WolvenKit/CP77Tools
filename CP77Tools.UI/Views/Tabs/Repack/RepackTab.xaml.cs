using ControlzEx.Theming;
using CP77Tools.UI.Data;
using CP77Tools.UI.Data.Tasks;
using CP77Tools.UI.Functionality.Customs;
using CP77Tools.UI.Views.Tasks;
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
using static CP77Tools.UI.Data.General;

namespace CP77Tools.UI.Views.Tabs.Repack
{
    /// <summary>
    /// Interaction logic for RepackTab.xaml
    /// </summary>
    public partial class RepackTab : UserControl
    {
        public RepackTab()
        {
            InitializeComponent();
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
            ThemeManager.Current.ChangeTheme(this, SUI.sui.generaldata.ThemeFinder());
            SUI.sui.RepackRepackTab = this;

        }
        private void ArchiveSelectOutpathButton_Click(object sender, RoutedEventArgs e)
        {
            OMD OutPathSelector = new OMD(TaskType.Repack, OMD_Type.Single, true);
            OutPathSelector.Title = "Select Outpath";
            OutPathSelector.RepackRepackTab = this;
            OutPathSelector.repacksubtasktype = RepackData.RepackTaskType.Repack;
            OutPathSelector.Show();
        }




        private void ArchiveSelectInputButton_Click(object sender, RoutedEventArgs e)
        {
            OMD InputSelector = new OMD(TaskType.Repack, OMD_Type.Multi, false);
            InputSelector.RepackRepackTab = this;
            InputSelector.repacksubtasktype = RepackData.RepackTaskType.Repack;
            InputSelector.Show();
        }

        private void ArchiveLaunchTaskButton_Click(object sender, RoutedEventArgs e)
        {
            CreateDumpTaskk();



        }

        private void CreateDumpTaskk()
        {
       
            var sometask = new TaskTemplate(General.TaskType.Repack);
    


        }
    }
}
