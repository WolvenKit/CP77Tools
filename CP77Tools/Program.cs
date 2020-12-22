﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.IoC;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using CP77Tools.Model;
using WolvenKit.Common.Services;
using WolvenKit.Common.Tools.DDS;
using System.Diagnostics;
using CP77.Common.Services;
using CP77.Common.Tools.FNV1A;
using CP77Tools.Commands;
using CP77Tools.Common.Services;
using CP77Tools.Extensions;
using Luna.ConsoleProgressBar;

namespace CP77Tools
{
    class Program
    {
        [STAThread]
        public static async Task Main(string[] args)
        {
            ServiceLocator.Default.RegisterType<ILoggerService, LoggerService>();
            ServiceLocator.Default.RegisterType<IHashService, HashService>();
            ServiceLocator.Default.RegisterType<IMainController, MainController>();

            var logger = ServiceLocator.Default.ResolveType<ILoggerService>();

            logger.OnStringLogged += delegate (object? sender, LogStringEventArgs args)
            {
                switch (args.Logtype)
                {
                    
                    case Logtype.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case Logtype.Important:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case Logtype.Success:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case Logtype.Normal:
                    case Logtype.Wcc:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                Console.WriteLine("[" + args.Logtype + "]" + args.Message);
                Console.ResetColor();
            };

            // get csv data
            logger.LogString("Loading Hashes...", Logtype.Important);
            await Loadhashes();

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

                    var pb = new ConsoleProgressBar()
                    {
                        DisplayBars = false,
                        DisplayPercentComplete = false,
                        DisplayAnimation = false
                    };
                    var parsed = CommandLineExtensions.ParseText(line, ' ', '"');

                    logger.PropertyChanged += OnLoggerOnPropertyChanged;
                    void OnLoggerOnPropertyChanged(object? sender, PropertyChangedEventArgs args)
                    {
                        switch (args.PropertyName)
                        {
                            case "Progress":
                            {
                                if (logger.Progress.Item1 == 0)
                                {
                                    pb = new ConsoleProgressBar() { DisplayBars = true, DisplayAnimation = false };
                                }

                                pb.Report(logger.Progress.Item1);
                                if (logger.Progress.Item1 == 1)
                                {
                                    System.Threading.Thread.Sleep(1000);

                                    Console.WriteLine();
                                    pb.Dispose();
                                    pb = null;
                                }

                                break;
                            }
                            default:
                                break;
                        }
                    }

                    rootCommand.InvokeAsync(parsed.ToArray()).Wait();

                    await WriteLog();
                    logger.PropertyChanged -= OnLoggerOnPropertyChanged;
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


        private static async Task Loadhashes()
        {
            var _maincontroller = ServiceLocator.Default.ResolveType<IMainController>();
            var logger = ServiceLocator.Default.ResolveType<ILoggerService>();
            Stopwatch watch = new Stopwatch();
            watch.Restart();

            var hashDictionary = new ConcurrentDictionary<ulong,string>();

            Parallel.ForEach(File.ReadLines(Constants.ArchiveHashesPath), line =>
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

            logger.LogString($"Loaded {hashDictionary.Count} hashes in {watch.ElapsedMilliseconds}ms.", Logtype.Success);
        }
    }
}
