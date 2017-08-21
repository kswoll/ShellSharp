using System;

namespace ShellSharp.Console
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public string Command { get; }

        public CommandAttribute(string command)
        {
            Command = command;
        }
    }
}