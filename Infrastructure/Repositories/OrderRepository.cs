using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly FishContext _fishContext;
        public OrderRepository(FishContext fishContext)
        {
            _fishContext = fishContext;
        }

        public Order MakeOrder(Order order)
        {
            _fishContext.Orders.Add(order);
            return order;
        }
        public ICollection<Order> AllOrders()
        {
            var orders = _fishContext.Orders
                .Include(e => e.OrderFishes)
                .ThenInclude(f => f.Fish)
                .Include(r => r.OrderItems)
                .Where(p => p.IsDeleted == false).ToList();
            return orders;
        }

        public Order GetOrder(Guid id)
        {
            var order = _fishContext.Orders
                .Include(x => x.Staff)
                .Include(t => t.OrderFishes)
                .ThenInclude(e => e.Fish)
                .Include(e => e.OrderItems)
                .FirstOrDefault(o => o.Id == id && o.IsDeleted == false);
            return order;
        }

        public ICollection<Order> GetOrderByCustomer(Guid customerId)
        {
            var order = _fishContext.Orders
                .Include(e => e.OrderItems)
                .Where(o => o.CustomerId == customerId && o.IsDeleted == false).ToList();
            return order;
        }

        public Order Update(Order order)
        {
            var getOrder = _fishContext.Orders.Update(order);
            return order;
        }
    }
}
