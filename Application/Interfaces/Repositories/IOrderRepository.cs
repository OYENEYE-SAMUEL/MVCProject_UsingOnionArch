using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Order MakeOrder(Order order);
        Order GetOrder(Guid id);
        Order Update(Order order);
        Order GetOrderByCustomer(Guid customerId);
        ICollection<Order> AllOrders();
    }
}
