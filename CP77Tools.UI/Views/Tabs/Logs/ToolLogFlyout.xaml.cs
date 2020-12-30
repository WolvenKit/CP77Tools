using CP77Tools.UI.Data;
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

namespace CP77Tools.UI.Views.Tabs.Logs
{
    /// <summary>
    /// Interaction logic for ToolLogFlyout.xaml
    /// </summary>
    public partial class ToolLogFlyout : UserControl
    {
        public ToolLogFlyout()
        {
            InitializeComponent(); if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            SUI.sui.generaldata.toolLogFlyoutInstance = this;
             SUI.sui.log.flyoutInstance = SUI.sui.generaldata.toolLogFlyoutInstance;

        }

        public void CreateNewLogItem(General.TaskType taskType)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                LogItem logItem = new LogItem(2);
            logItem.SenderLabel.Content = taskType.ToString();
            logItem.SenderText.Text = "A task with type : " + taskType.ToString() + " has finished.";
            LogItemWrapPanel.Children.Add(logItem);
            }));

        }

        private void LogItemWrapPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
    }
}
