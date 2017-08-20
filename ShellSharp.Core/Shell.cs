// <copyright file="Shell.cs" company="PlanGrid, Inc.">
//     Copyright (c) 2017 PlanGrid, Inc. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;

namespace ShellSharp.Core
{
    public class Shell
    {
        public DirectoryName WorkingDirectory { get; set; }

        public Shell(string workingDirectory = null)
        {
            WorkingDirectory = workingDirectory ?? Environment.CurrentDirectory;
        }

        public Process Start(FileName fileName, string arguments)
        {
            var startInfo = new ProcessStartInfo
            {
                WorkingDirectory = (WorkingDirectory + fileName.Directory).ToString(),
                FileName = fileName.Name,
                Arguments = arguments,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            var process = new Process
            {
                StartInfo = startInfo,
                EnableRaisingEvents = true
            };

            void OnOutputDataReceived(object sender, DataReceivedEventArgs args) => Console.WriteLine(args.Data);
            void OnErrorDataReceived(object sender, DataReceivedEventArgs args)
            {
                if (!process.HasExited)
                    Console.Error.WriteLine(args.Data);
            }

            process.OutputDataReceived += OnOutputDataReceived;
            process.ErrorDataReceived += OnErrorDataReceived;

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            return process;
        }

        public void Run(FileName fileName, string arguments)
        {
            Start(fileName, arguments).WaitForExit();
        }

        public void ChangeDirectory(DirectoryName directory)
        {
            WorkingDirectory = WorkingDirectory + directory;
        }
    }
}