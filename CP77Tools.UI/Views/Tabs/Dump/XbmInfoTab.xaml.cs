using System;
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
using ControlzEx.Theming;

namespace CP77Tools.UI.Views.Tabs.Dump
{
    /// <summary>
    /// Interaction logic for XbmInfoTab.xaml
    /// </summary>
    public partial class XbmInfoTab : UserControl
    {
        public XbmInfoTab()
        {
            InitializeComponent();
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
            ThemeManager.Current.ChangeTheme(this, SUI.sui.generaldata.ThemeFinder());

            SUI.sui.dumpdata.Dump_Info = true;
            SUI.sui.DumpXbmTab = this;

        }
        private void ArchiveSelectOutpathButton_Click(object sender, RoutedEventArgs e)
        {
            OMD OutPathSelector = new OMD(TaskType.Dump, OMD_Type.Single, true);
            OutPathSelector.Title = "Select Outpath";
            OutPathSelector.DumpXbmTab = this;
            OutPathSelector.dumpsubtasktype = DumpData.DumpTaskType.Info;
            OutPathSelector.Show();
        }




        private void ArchiveSelectInputButton_Click(object sender, RoutedEventArgs e)
        {
            OMD InputSelector = new OMD(TaskType.Dump, OMD_Type.Multi, false);
            InputSelector.DumpXbmTab = this;
            InputSelector.dumpsubtasktype = DumpData.DumpTaskType.Info;
            InputSelector.Show();
        }

        private void ArchiveLaunchTaskButton_Click(object sender, RoutedEventArgs e)
        {
            CreateDumpTaskk();



        }

        private void CreateDumpTaskk()
        {
            var sometask = new TaskTemplate(General.TaskType.Dump);



        }
    }
}
