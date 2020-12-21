using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WolvenKit.Common.Services;
using CP77Tools.UI;
using System.Text.RegularExpressions;

namespace CP77Tools.UI.Functionality
{
    public class Logging
    {

        
        private MainWindow app;
        public Logging(MainWindow mainWindow) { this.app = mainWindow; }


        public void UI_Logger_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            Trace.Write(e.PropertyName);
        }

        public void UI_Logger_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is LoggerService _logger)
            {
                switch (e.PropertyName)
                {
                    case "Progress":
                        {
                            UIProgressCounter(_logger); break;
                        }
                    default:
                        break;
                }
            }
            else
            {
                Trace.Write(e.PropertyName);
            }
        }

        public void UI_Logger_OnStringLogged(object sender, LogStringEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                if (app.data.InterceptLog)
                {
                    app.Hash_Output_UIElement_TextBox.Text = e.Message.ToString();
                    app.data.InterceptLog = false;
                }
                if (!app.data.InterceptLog)
                {
                    Logtype TYPE = e.Logtype;
                    string OUTPUTSTRING = "[" + TYPE.ToString() + "]" + e.Message.ToString(); ;
                    if (e.Message.Contains("File  loaded"))
                    {
                        OUTPUTSTRING = Regex.Replace(OUTPUTSTRING, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

                    }

                    app.Main_ProgressOutput_UIElement_TextBlock.Text = OUTPUTSTRING;
                    app.Main_OutputBox_UIElement_ComboBox.Items.Add(OUTPUTSTRING);
                }
      
       


   

            }));

        }





        // Progress Reporting.
        private int TaskCounter = 0;
        public void UIProgressCounter(LoggerService _logger)
        {
            TaskCounter += 1;
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                Logtype TYPE = _logger.Logtype;
                var CURRENTTASK = TaskCounter;
                string OUTPUTSTRING = "[" + TYPE.ToString() + "]" + " - Working on Task : " + CURRENTTASK ;
                app.Main_ProgressBar_UIElement_ProgressBar.Value += _logger.Progress.Item1;
                app.Main_ProgressOutput_UIElement_TextBlock.Text = OUTPUTSTRING;


            }));
        }

        // Reporting when finished. Should soon be unneeded
        public void TaskFinished(MainWindow.TaskType CurrentTaskType)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                app.Main_ProgressBar_UIElement_ProgressBar.Value = 0;
                app.Main_ProgressOutput_UIElement_TextBlock.Text = "[Normal] - Finished : " + CurrentTaskType.ToString();
                TaskCounter = 0;
            }));
        }
    }
}
