using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Nito.AsyncEx;
using ShellSharp.Core;

namespace ShellSharp.Console
{
    class Program
    {
        private CommandRegistry commands;

        static void Main(string[] args)
        {
            var program = new Program(new[] { typeof(ICommand).Assembly }, args);
            program.Start();
        }

        public Program(Assembly[] assemblies, string[] arguments)
        {
            commands = new CommandRegistry(assemblies);
        }

        public void Start()
        {
            AsyncContext.Run(Run);
        }

        private async Task Run()
        {
            var shell = new Shell();

            while (true)
            {
                var line = System.Console.ReadLine();
                var lineParts = line.Split(' ');
                var commandName = lineParts.First();
                var arguments = lineParts.Skip(1).ToArray();
                var command = commands[commandName];
                await command.Execute(shell, arguments);
            }
        }
    }
}
