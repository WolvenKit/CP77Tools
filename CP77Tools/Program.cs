using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Catel.IoC;
using System.CommandLine;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using CP77Tools.Services;
using WolvenKit.Common.FNV1A;
using WolvenKit.Common.Services;
using System.Diagnostics;
using CP77Tools.Commands;
using Luna.ConsoleProgressBar;

namespace CP77Tools
{
    class Program
    {
        [STAThread]
        public static async Task Main(string[] args)
        {
            ServiceLocator.Default.RegisterType<ILoggerService, LoggerService>();
            ServiceLocator.Default.RegisterType<IMainController, MainController>();
            var logger = ServiceLocator.Default.ResolveType<ILoggerService>();

            // get csv data
            Console.WriteLine("Loading Hashes...");
            await Loadhashes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/archivehashes.csv"));
            
            var rootCommand = new RootCommand();
            rootCommand.Add(new ArchiveCommand());
            rootCommand.Add(new DumpCommand());
            rootCommand.Add(new CR2WCommand());
            rootCommand.Add(new HashCommand());
            rootCommand.Add(new OodleCommand());

            // Run
            if (args == null || args.Length == 0)
            {
                // write welcome message
                rootCommand.InvokeAsync("-h").Wait();

                while (true)
                {
                    string line = System.Console.ReadLine();

                    if (line == "q()")
                        return;

                    var parsed = CommandLineExtensions.ParseText(line, ' ', '"');

                    using var pb = new ConsoleProgressBar()
                    {
                        DisplayBars = true,
                        DisplayAnimation = false
                    };


                    logger.PropertyChanged += delegate (object? sender, PropertyChangedEventArgs args)
                    {
                        if (sender is LoggerService _logger)
                        {
                            switch (args.PropertyName)
                            {
                                case "Progress":
                                {
                                    pb.Report(_logger.Progress.Item1);
                                    break;
                                }
                                default:
                                    break;
                            }
                        }
                    };


                    rootCommand.InvokeAsync(parsed.ToArray()).Wait();

                    await WriteLog();
                }

            }
            else
            {
                rootCommand.InvokeAsync(args).Wait();

                await WriteLog();
            }


            async Task WriteLog()
            {
                if (string.IsNullOrEmpty(logger.ErrorLogStr))
                    return;

                var t = DateTime.Now.ToString("yyyyMMddHHmmss");
                var fi = new FileInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    $"errorlogs/log_{t}.txt"));
                if (fi.Directory != null)
                {
                    Directory.CreateDirectory(fi.Directory.FullName);
                    var log = logger.ErrorLogStr;
                    await File.WriteAllTextAsync(fi.FullName, log);
                }
                else
                {
                    
                }
            }

        }


        private static async Task Loadhashes(string path)
        {
            var _maincontroller = ServiceLocator.Default.ResolveType<IMainController>();

            Stopwatch watch = new Stopwatch();
            watch.Restart();

            var hashDictionary = new ConcurrentDictionary<ulong,string>();

            Parallel.ForEach(File.ReadLines(path), line =>
            {
                // check line
                line = line.Split(',', StringSplitOptions.RemoveEmptyEntries).First();
                if (!string.IsNullOrEmpty(line))
                {
                    ulong hash = FNV1A64HashAlgorithm.HashString(line);
                    hashDictionary.AddOrUpdate(hash, line, (key, val) => val);
                }                
            });

            _maincontroller.Hashdict = hashDictionary.ToDictionary(
                entry => entry.Key,
                entry => entry.Value);

            watch.Stop();

            Console.WriteLine($"Loaded {hashDictionary.Count} hashes in {watch.ElapsedMilliseconds}ms.");
        }
    }
}
