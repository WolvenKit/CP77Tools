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
using System.Windows.Threading;

namespace CP77Tools.UI.Views.Tabs.Logs
{
    /// <summary>
    /// Interaction logic for LogItem.xaml
    /// </summary>
    public partial class LogItem : UserControl
    {
        public LogItem(int type)
        {
            InitializeComponent();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                CurrentTimeStamp.Content = DateTime.Now;
                SetColor(type);

            }));
       
        }

        private void SetColor(int type)
        {

            switch (type)
            {
                case 1:
                    BackGround.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7C0000")); // Error
                    break;
                case 2:
                    BackGround.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF1F4D1A")); // Succes

                    break;
            }
        }
    }
}
