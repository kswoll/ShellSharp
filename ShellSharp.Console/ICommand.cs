using System.Threading.Tasks;
using ShellSharp.Core;

namespace ShellSharp.Console
{
    public interface ICommand
    {
        Task Execute(Shell shell, string[] arguments);
    }
}