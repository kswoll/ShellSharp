using System.Threading.Tasks;
using ShellSharp.Core;

namespace ShellSharp.Console.Commands
{
    [Command("dir")]
    public class ListDirectoryCommand : ICommand
    {
        public Task Execute(Shell shell, string[] arguments)
        {

            throw new System.NotImplementedException();
        }
    }
}