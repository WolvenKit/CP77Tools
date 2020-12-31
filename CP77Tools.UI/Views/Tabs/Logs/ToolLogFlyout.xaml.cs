using ControlzEx.Theming;
using CP77Tools.UI.Data;
using CP77Tools.UI.Functionality;
using CP77Tools.UI.Views.Notifications;
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
            ThemeManager.Current.ChangeTheme(this, SUI.sui.generaldata.ThemeFinder());

            SUI.sui.generaldata.toolLogFlyoutInstance = this;
             SUI.sui.log.flyoutInstance = SUI.sui.generaldata.toolLogFlyoutInstance;

        }

        public void CreateNewLogItem(General.TaskType taskType, int TaskID)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                LogItem logItem = new LogItem(2);
            logItem.SenderLabel.Content = taskType.ToString();
                logItem.SenderText.Text = "[" + taskType.ToString() + "] - " + TaskID.ToString() + " has finished.";
            LogItemWrapPanel.Children.Add(logItem);



           if(Properties.Settings.Default.EnablePopUps)
            {
                    ToastNotification notification = new ToastNotification(taskType.ToString() + " Task Finished", "The " + taskType.ToString() + " has finished" + "\n" + "You can see the results on the task page.", 1);
                    notification.Show();
                }
             


                UserInterfaceLogic.controller.CloseAsync();

            }));

        }

        private void LogItemWrapPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
    }
}
