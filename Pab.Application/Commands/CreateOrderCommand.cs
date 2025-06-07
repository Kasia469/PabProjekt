using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pab.Application.Commands
{
    public class CreateOrderCommand
    {
        public string UserId { get; set; } = null!;
        public List<(int ProductId, int Quantity)> Items { get; set; } = new();
    }
}
