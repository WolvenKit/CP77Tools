using CP77.Common.Services;
using CP77Tools.UI.Data;
using CP77Tools.UI.Data.Tasks;
using CP77Tools.UI.Views.Tabs.Logs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace CP77Tools.UI.Views.Tasks
{
    /// <summary>
    /// Interaction logic for TaskTemplate.xaml
    /// </summary>
    public partial class TaskTemplate : UserControl
    {
        public General.TaskType localtasktype;
        public TaskTemplate(General.TaskType maintype)
        {
            localtasktype = maintype;
            InitializeComponent();
            SUI.sui.UI_Logger.PropertyChanged += UI_Logger_PropertyChanged;
            SUI.sui.UI_Logger.OnStringLogged += UI_Logger_OnStringLogged;
            SUI.sui.UI_Logger.PropertyChanging += UI_Logger_PropertyChanging;

            switch (localtasktype)
            {
                case General.TaskType.Archive:
                    StartArchiveTask();
                    break;
                case General.TaskType.CR2W:
                    StartCR2WTask();
                    break;
                case General.TaskType.Dump:
                    StartDumpTask();
                    break;
                case General.TaskType.Repack:
                    StartRepackTask();
                    break;
                case General.TaskType.Oodle:
                    StartOodleTask();
                    break;
                case General.TaskType.Hash:
                    StartHashTask();
                    break;

            }
        }

        private void UI_Logger_PropertyChanging(object sender, System.ComponentModel.PropertyChangingEventArgs e)
        {
        }

        private void UI_Logger_OnStringLogged(object sender, CP77.Common.Services.LogStringEventArgs e)
        {
            var a = new StaTaskScheduler(2);

            
           Task.Factory.StartNew(() =>
            {
                AddToTaskLog(e.Logtype, e.Message);
            }, CancellationToken.None, TaskCreationOptions.None, a);
           
            
            
            //Task newtask = new Task(() => AddToTaskLog(e.Logtype, e.Message));


            //Task.Factory.StartNew(() => AddToTaskLog(e.Logtype, e.Message)).ContinueWith(r => AddToTaskLog(e.Logtype, e.Message), scheduler);




       



        }

        private void AddToTaskLog(Logtype logtype, string message)
        {
            //    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            //    {
            //    var logtype = e.Logtype;
            //   var message = e.Message;
            if (logtype.ToString() == "Error")
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    LogItem TaskLogItem = new LogItem(1);
                    TaskLogItem.SenderLabel.Content = localtasktype.ToString();
                    TaskLogItem.SenderText.Text = "[" + logtype + "] : " + message;
                    TaskLogItem.HorizontalAlignment = HorizontalAlignment.Stretch;

                    TaskLogWrap.Children.Add(TaskLogItem);
                }));
            }
            else if (logtype.ToString() == "Success")
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    LogItem TaskLogItem = new LogItem(2);
                    TaskLogItem.SenderLabel.Content = localtasktype.ToString();
                    TaskLogItem.SenderText.Text = "[" + logtype + "] : " + message;
                    TaskLogItem.HorizontalAlignment = HorizontalAlignment.Stretch;


                    TaskLogWrap.Children.Add(TaskLogItem);
                }));
            }
            else if (logtype.ToString() == "Normal")
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    LogItem TaskLogItem = new LogItem(7);
                    TaskLogItem.SenderLabel.Content = localtasktype.ToString();
                    TaskLogItem.SenderText.Text = "[" + logtype + "] : " + message;
                    TaskLogItem.HorizontalAlignment = HorizontalAlignment.Stretch;

                    TaskLogWrap.Children.Add(TaskLogItem);
                }));
            }
            //   }));
        }

        private void UI_Logger_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        private void StartHashTask()
        {
            SUI.sui.ui.ThreadedTaskSender( General.TaskType.Hash);
        }

        private void StartOodleTask()
        {
            SUI.sui.ui.ThreadedTaskSender(General.TaskType.Oodle);
        }

        private void StartRepackTask()
        {
            SUI.sui.ui.ThreadedTaskSender(General.TaskType.Repack);
        }

        private void StartDumpTask()
        {
            SUI.sui.ui.ThreadedTaskSender(General.TaskType.Dump);
        }

        private void StartCR2WTask()
        {
            SUI.sui.ui.ThreadedTaskSender(General.TaskType.CR2W);
        }

        public void StartArchiveTask()
        {
            SUI.sui.ui.ThreadedTaskSender(General.TaskType.Archive);
        }

        private void TaskLogWrap_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }





    }
    /// <summary>Provides a scheduler that uses STA threads.</summary>
    public sealed class StaTaskScheduler : TaskScheduler, IDisposable
    {
        /// <summary>Stores the queued tasks to be executed by our pool of STA threads.</summary>
        private BlockingCollection<Task> _tasks;
        /// <summary>The STA threads used by the scheduler.</summary>
        private readonly List<Thread> _threads;

        /// <summary>Initializes a new instance of the StaTaskScheduler class with the specified concurrency level.</summary>
        /// <param name="numberOfThreads">The number of threads that should be created and used by this scheduler.</param>
        public StaTaskScheduler(int numberOfThreads)
        {
            // Validate arguments
            if (numberOfThreads < 1) throw new ArgumentOutOfRangeException(nameof(numberOfThreads));

            // Initialize the tasks collection
            _tasks = new BlockingCollection<Task>();

            // Create the threads to be used by this scheduler
            _threads = Enumerable.Range(0, numberOfThreads).Select(i =>
            {
                var thread = new Thread(() =>
                {
                    // Continually get the next task and try to execute it.
                    // This will continue until the scheduler is disposed and no more tasks remain.
                    foreach (var t in _tasks.GetConsumingEnumerable())
                    {
                        TryExecuteTask(t);
                    }
                })
                {
                    IsBackground = true
                };
                thread.SetApartmentState(ApartmentState.STA);
                return thread;
            }).ToList();

            // Start all of the threads
            _threads.ForEach(t => t.Start());
        }

        /// <summary>Queues a Task to be executed by this scheduler.</summary>
        /// <param name="task">The task to be executed.</param>
        protected override void QueueTask(Task task) =>
            // Push it into the blocking collection of tasks
            _tasks.Add(task);

        /// <summary>Provides a list of the scheduled tasks for the debugger to consume.</summary>
        /// <returns>An enumerable of all tasks currently scheduled.</returns>
        protected override IEnumerable<Task> GetScheduledTasks() =>
            // Serialize the contents of the blocking collection of tasks for the debugger
            _tasks.ToArray();

        /// <summary>Determines whether a Task may be inlined.</summary>
        /// <param name="task">The task to be executed.</param>
        /// <param name="taskWasPreviouslyQueued">Whether the task was previously queued.</param>
        /// <returns>true if the task was successfully inlined; otherwise, false.</returns>
        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) =>
            // Try to inline if the current thread is STA
            Thread.CurrentThread.GetApartmentState() == ApartmentState.STA &&
                TryExecuteTask(task);

        /// <summary>Gets the maximum concurrency level supported by this scheduler.</summary>
        public override int MaximumConcurrencyLevel => _threads.Count;

        /// <summary>
        /// Cleans up the scheduler by indicating that no more tasks will be queued.
        /// This method blocks until all threads successfully shutdown.
        /// </summary>
        public void Dispose()
        {
            if (_tasks != null)
            {
                // Indicate that no new tasks will be coming in
                _tasks.CompleteAdding();

                // Wait for all threads to finish processing tasks
                foreach (var thread in _threads) thread.Join();

                // Cleanup
                _tasks.Dispose();
                _tasks = null;
            }
        }
    }
}
