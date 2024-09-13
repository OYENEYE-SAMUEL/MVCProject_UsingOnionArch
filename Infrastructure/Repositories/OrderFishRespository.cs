using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderFishRespository : IOrderFishRepository
    {
        private readonly FishContext _fishContext;
        public OrderFishRespository(FishContext fishContext)
        {
            _fishContext = fishContext;
        }

        public OrderFish Create(OrderFish orderFish)
        {
            _fishContext.OrderFishes.Add(orderFish);
            return orderFish;
        }
    }
}
