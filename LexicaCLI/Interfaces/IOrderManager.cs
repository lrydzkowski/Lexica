using System.Threading.Tasks;
using Lexica.CLI.Models;

namespace Lexica.CLI.Interfaces
{
    public interface IOrderManager
    {
        public Task<string> Transmit(Order order);
    }
}