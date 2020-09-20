using System.Threading.Tasks;
using Lexica.CLI.Interfaces;
using Lexica.CLI.Models;

namespace Lexica.CLI.Managers
{
    public class OrderManager : IOrderManager
    {
        private readonly IOrderSender orderSender;

        public OrderManager(IOrderSender sender)
        {
            orderSender = sender;
        }

        public async Task<string> Transmit(Order order)
        {
            return await orderSender.Send(order);
        }
    }
}