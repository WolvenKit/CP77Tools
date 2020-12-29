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
            InitializeComponent();
            CreateNewLogItem("Archive",2);
            CreateNewLogItem("Error", 1);
            CreateNewLogItem("Archive", 2);
            CreateNewLogItem("Error", 1); 
            CreateNewLogItem("Archive", 2);
            CreateNewLogItem("Error", 1); 
            CreateNewLogItem("Archive", 7);
            CreateNewLogItem("Error", 7); 
            CreateNewLogItem("Archive", 2);
            CreateNewLogItem("Error", 1);


        }


        public void CreateNewLogItem(string Sender, int type)
        {
            LogItem logItem = new LogItem(type);
            logItem.SenderLabel.Content = Sender;
            logItem.SenderText.Text = "TESTING : Error Found during Task\n Try launching the task again.";

            LogItemWrapPanel.Children.Add(logItem);
        }
    }
}
