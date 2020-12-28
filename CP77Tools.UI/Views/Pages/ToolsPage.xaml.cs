using ControlzEx.Theming;
using CP77Tools.UI.Data;
using CP77Tools.UI.Data.Tasks;
using CP77Tools.UI.Functionality.Customs;
using CP77Tools.UI.Functionality.ToolsWindow;
using CP77Tools.UI.Views.Tabs.Archive;
using CP77Tools.UI.Views.Tasks;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WolvenKit.Common.Tools.DDS;
using static CP77Tools.UI.Data.General;

namespace CP77Tools.UI.Views.Pages
{
    /// <summary>
    /// Interaction logic for Tools.xaml
    /// </summary>
    public partial class Tools : Page
    {
        private SUI sui;
        private ArchiveData.TaskType selectedTaskType;

        public Tools()
        {
            InitializeComponent();

            ThemeManager.Current.ChangeTheme(this, "Dark.Steel");

        }

        public static CustomTab _CustomTab;


        private void ArhiveBottomTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not MetroTabControl)
                return;
            if (sender == ArhiveBottomTab)
            {
                TabItem Current = (TabItem)ArhiveBottomTab.SelectedItem;

                switch (Current.Header)
                {
                    case "New Task":
                       //  _CustomTab = new CustomTab();
                      //  Current.Content = _CustomTab;
                        selectedTaskType = ArchiveData.TaskType.Custom;

                        break;
                    case "Extract":
                  //      Current.Content = new ExtractTab();
//
                     selectedTaskType = ArchiveData.TaskType.Extract;

                        break;
                    case "Uncook":
                   ///     Current.Content = new UncookTab();
                   //
                       selectedTaskType = ArchiveData.TaskType.Uncook;

                        break;
                    case "Dump":
                     //   Current.Content = new DumpTab();

                        selectedTaskType = ArchiveData.TaskType.Dump;

                        break;
                    case "List":
                      //  Current.Content = new ListTab();

                       selectedTaskType = ArchiveData.TaskType.List;

                        break;
                    case "Extract Single":
                     //   Current.Content = new ExtractSingleTab();

                        selectedTaskType = ArchiveData.TaskType.Single;

                        break;
                }
                Trace.Write(selectedTaskType);
            }
          

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
      
        }

        private void PackIconFeatherIcons_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ToolsLogFlyOut.IsOpen = true;
        }

        private void ToolsLogFlyOut_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            if (ToolsLogFlyOut.IsOpen)
            {
                LogsLabel.Visibility = Visibility.Hidden;

            }
            else
            {
                LogsLabel.Visibility = Visibility.Visible;

            }
        }
    }









}
