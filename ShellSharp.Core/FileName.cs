// <copyright file="FileName.cs" company="PlanGrid, Inc.">
//     Copyright (c) 2017 PlanGrid, Inc. All rights reserved.
// </copyright>

using System.IO;

namespace ShellSharp.Core
{
    public struct FileName
    {
        public DirectoryName Directory { get; }
        public string Name { get; }
        public string Extension => Path.GetExtension(Name).Substring(1);

        public FileName(string fileName)
        {
            int lastSlashIndex = fileName.LastIndexOf('\\');
            if (lastSlashIndex != -1)
            {
                Directory = new DirectoryName(fileName.Substring(0, lastSlashIndex));
                fileName = fileName.Substring(lastSlashIndex + 1);
            }
            else
            {
                Directory = default(DirectoryName);
            }

            Name = fileName;
        }

        public override string ToString()
        {
            if (!Directory.IsEmpty)
                return $"{Directory}\\{Name}";
            else
                return Name;
        }

        public static implicit operator FileName(string fileName) => new FileName(fileName);
    }
}