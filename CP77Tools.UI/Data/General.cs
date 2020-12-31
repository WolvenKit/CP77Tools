using ControlzEx.Theming;
using CP77Tools.UI.Data.Tasks;
using CP77Tools.UI.Functionality;
using CP77Tools.UI.Views.Pages;
using CP77Tools.UI.Views.Tabs.Logs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Controls;
using WolvenKit.Common.Tools.DDS;

namespace CP77Tools.UI.Data
{

    public class QueuedTask
    {
        public int taskID { get; set; }
        public Task task { get; set; }
        public General.TaskType tasktype { get; set; }
        public ArchiveData.TaskType subtypearchive { get; set; }

        public QueuedTask(int i, Task t, General.TaskType tt)
        {
            taskID = i;
            task = t;
            tasktype = tt;
        }
    }
    public class General
    {

        public static List<int> TaskIDList = new List<int>();
       // public static ConcurrentQueue<Task> cq = new ConcurrentQueue<Task>();
        public Dictionary<int, Task> TaskTodoDict = new Dictionary<int, Task>();





        public  General()
        {

        }

      

        public async Task AddTaskToQueueAsync(QueuedTask queuedTask)
        {

        }

        public async void JustWaiting()
        {
            
        //    await foreach (QueuedTask i)
        //    {
              
      //      }
        }

        public int TaskIDGen()
        {
            var a = TaskIDList.Count + 1;
            TaskIDList.Add(a);
            return a;

        }

        public enum ColorShade
        {
            
            Dark,
            Light
        }

        public string[] Colors2 = new string[]
        {
            "Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald", "Teal", "Cyan", "Cobalt", "Indigo", "Violet", "Pink", "Magenta", "Crimson", "Amber", "Yellow", "Brown", "Olive", "Steel", "Mauve", "Taupe", "Sienna"
        };

        public string CurrentTheme = "Dark.Steel";

        public string ThemeFinder()
        {
            return CurrentTheme;
        }
        public string ThemeHelper(string Theme)
        {

            CurrentTheme = Theme;

            return Theme;
            
        }


        public class Preset
        {
            internal string name;
            internal Dictionary<string,string> Pdata;
            public General.TaskType Type { get; set; }
            public string Name { get; set; }
            public Dictionary<string, string> PresetData { get; set; }

            public Preset(General.TaskType taskType, string _name, Dictionary<string, string> data)
            {
                Type = taskType;
                Name = _name;
                name = _name;
                PresetData = data;
                Pdata = data;


            }

        }


  

        public enum TaskType { Archive, Dump, CR2W, Hash, Oodle, Repack, }

        // OTHER
        public static string[] OMD_Output;


        public enum OMD_Type        {            Single,            Multi        }


        public bool InterceptLog = false;


        public Tools ToolsInstance { get; set; }
        public SettingsPage SettingsInstance { get; set; }

        public ToolLogFlyout toolLogFlyoutInstance { get; set; }




    }

   


}
