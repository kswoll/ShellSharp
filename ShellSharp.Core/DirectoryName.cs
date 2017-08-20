using System;
using System.Collections.Generic;
using System.Text;

namespace ShellSharp.Core
{
    public struct DirectoryName
    {
        public string Drive { get; }
        public string Name { get; }
        public bool IsAbsolute { get; }
        public bool IsEmpty => Name == null;

        public DirectoryName(string drive, string name, bool isAbsolute)
        {
            Drive = drive;
            Name = name;
            IsAbsolute = isAbsolute;
        }

        public DirectoryName(string directoryName)
        {
            if (directoryName.Length > 1 && directoryName[1] == ':')
            {
                Drive = directoryName.Substring(0, 1);
                directoryName = directoryName.Substring(2);
            }
            else
            {
                Drive = null;
            }

            if (directoryName.StartsWith("\\"))
            {
                IsAbsolute = true;
                directoryName = directoryName.Substring(1);
            }
            else
            {
                IsAbsolute = false;
            }

            Name = Simplify(directoryName, IsAbsolute);
        }

        private static string Simplify(string directoryName, bool isAbsolute)
        {
            var parts = directoryName.Split('\\');
            var names = new List<string>();
            foreach (var part in parts)
            {
                if (part == ".")
                    continue;
                else if (part == "..")
                {
                    if (names.Count > 0)
                        names.RemoveAt(names.Count - 1);
                    else if (isAbsolute)
                        throw new ArgumentException("Name contained parent folder references (i.e. `..`)  that carried the path beyond the absolute root.", nameof(directoryName));
                }
                else
                    names.Add(part);
            }
            return string.Join("\\", names);
        }

        public override string ToString()
        {
            if (IsEmpty)
                return "";

            var builder = new StringBuilder();
            if (Drive != null)
                builder.Append(Drive).Append(':');
            if (IsAbsolute)
                builder.Append('\\');
            if (Name != null)
                builder.Append(Name);

            return builder.ToString();
        }

        public static DirectoryName operator +(DirectoryName directory1, DirectoryName directory2)
        {
            if (directory2.Drive != null)
                return directory2;
            else if (directory2.IsAbsolute)
                return new DirectoryName(directory1.Drive, directory2.Name, true);
            else
                return new DirectoryName(directory1.Drive, $"{directory1.Name}\\{directory2.Name}", directory1.IsAbsolute);
        }
        
        public static implicit operator DirectoryName(string directoryName) => new DirectoryName(directoryName);
//        public static implicit operator string(DirectoryName directoryName) => directoryName.ToString();
    }
}