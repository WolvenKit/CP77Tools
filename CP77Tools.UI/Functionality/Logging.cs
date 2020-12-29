using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CP77Tools.UI;
using System.Text.RegularExpressions;
using CP77Tools.UI.Data;
using CP77.Common.Services;

namespace CP77Tools.UI.Functionality
{
    public class Logging
    {

        
        private SUI app;
        public Logging(SUI _SUI) { this.app = _SUI; }


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
                if (app.generaldata.InterceptLog)
                {
                  //  app.Hash_Output_UIElement_TextBox.Text = e.Message.ToString();
                    app.generaldata.InterceptLog = false;
                }
                if (!app.generaldata.InterceptLog)
                {
                    Logtype TYPE = e.Logtype;
                    string OUTPUTSTRING = "[" + TYPE.ToString() + "]" + e.Message.ToString(); ;
                    if (e.Message.Contains("File  loaded"))
                    {
                        OUTPUTSTRING = Regex.Replace(OUTPUTSTRING, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

                    }

                 //   app.Main_ProgressOutput_UIElement_TextBlock.Text = OUTPUTSTRING;
                 //   app.Main_OutputBox_UIElement_ComboBox.Items.Add(OUTPUTSTRING);
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
              //  if (app.Main_ProgressBar_UIElement_ProgressBar.Value >= 1) { app.Main_ProgressBar_UIElement_ProgressBar.Value = 0; }

                Logtype TYPE = _logger.Logtype;
                var CURRENTTASK = TaskCounter;
                string OUTPUTSTRING = "[" + TYPE.ToString() + "]" + " - Working on Task : " + CURRENTTASK ;
              //  app.Main_ProgressBar_UIElement_ProgressBar.Value += _logger.Progress.Item1;
               // app.Main_ProgressOutput_UIElement_TextBlock.Text = OUTPUTSTRING;

            }));
        }

        // Reporting when finished. Should soon be unneeded  (Maybe keep this for opening output after it finishes.)
        public void TaskFinished(General.TaskType CurrentTaskType)
        {
            switch (CurrentTaskType)
            {
                case General.TaskType.Archive:
                 //   Process.Start("explorer.exe", app.generaldata.Archive_OutPath);
                    break;
                case General.TaskType.CR2W:
                    Process.Start("explorer.exe", app.cr2wdata.CR2W_OutPath);
                    break;
                case General.TaskType.Dump:
                    Process.Start("explorer.exe", app.dumpdata.Dump_OutPath);
                    break;
                case General.TaskType.Hash:
                    break;
                case General.TaskType.Oodle:
                    Process.Start("explorer.exe", app.oodledata.Oodle_OutPath);
                    break;
                case General.TaskType.Repack:
                    Process.Start("explorer.exe", app.repackdata.Repack_OutPath);
                    break;
            }
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
           //     app.Main_ProgressBar_UIElement_ProgressBar.Value = 0;
              //  app.Main_ProgressOutput_UIElement_TextBlock.Text = "[Succes] - Finished : " + CurrentTaskType.ToString();
            //    app.Main_OutputBox_UIElement_ComboBox.Items.Add("[Succes] - Finished : " + CurrentTaskType.ToString());
                TaskCounter = 0;
            }));
        }
    }
}
