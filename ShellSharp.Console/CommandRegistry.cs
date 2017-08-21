using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ShellSharp.Console
{
    public class CommandRegistry
    {
        private readonly Dictionary<string, ICommand> commands;

        public CommandRegistry(params Assembly[] assemblies) : this(GetCommandTypes(assemblies).ToArray())
        {
        }

        public CommandRegistry(params Type[] types)
        {
            commands = types
                .Select(x => new
                {
                    Type = x,
                    Attribute = x.GetCustomAttribute<CommandAttribute>()
                })
                .ToDictionary(x => x.Attribute.Command, x => (ICommand)Activator.CreateInstance(x.Type));
        }

        public ICommand this[string command]
        {
            get
            {
                commands.TryGetValue(command, out var result);
                return result;
            }
        }

        private static IEnumerable<Type> GetCommandTypes(Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes().Where(x => typeof(ICommand).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract))
                {
                    var attribute = type.GetCustomAttribute<CommandAttribute>();
                    if (attribute == null)
                        throw new Exception($"A command must be decorated with a [Command] attribute: {type.FullName}");

                    yield return type;
                }
            }
        }
    }
}