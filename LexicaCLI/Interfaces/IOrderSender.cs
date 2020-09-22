using System.Threading.Tasks;
using Lexica.CLI.Models;

namespace Lexica.CLI.Interfaces
{
    public interface IOrderSender
    {
        Task<string> Send(Order order);
    }
}
