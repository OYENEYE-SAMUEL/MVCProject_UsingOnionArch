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
    public class FishRepository : IFishRepository
    {
        private readonly FishContext _fishContext;
        public FishRepository(FishContext fishContext)
        {
            _fishContext = fishContext;
        }
        public bool Check(string name)
        {
            var exist = _fishContext.Fishes.
                Any(f => f.Name == name && f.IsDeleted == false);
            return exist;
        }

        public Fish Create(Fish fish)
        {
            _fishContext.Fishes.Add(fish);
            return fish;
        }

        public ICollection<Fish> GetAll()
        {
            var orders = _fishContext.Fishes
                .Include(v => v.FishPonds)
                .ThenInclude(r => r.Pond)
                .Include(h => h.OrderFishes)
                .ThenInclude(e => e.Order)
                .Where(f => f.IsDeleted == false).ToList();
            return orders;
        }

        public Fish GetById(Guid id)
        {
            var fish = _fishContext.Fishes
                .Include(f => f.FishPonds)
                .ThenInclude(g => g.Pond)
                .Include(h => h.OrderFishes)
                .ThenInclude(e => e.Order)
                .FirstOrDefault(f => f.Id == id && f.IsDeleted == false);
            return fish;
        }

        public Fish GetByName(string name)
        {
            var fish = _fishContext.Fishes
                .Include(f => f.FishPonds)
                .ThenInclude(r => r.Pond)
                .Include(h => h.OrderFishes)
                .ThenInclude(e => e.Order)
                .FirstOrDefault(f => f.Name == name && f.IsDeleted == false);
            return fish;
        }

        public Fish Update(Fish fish)
        {
            var getFish = _fishContext.Fishes.Update(fish);
            return fish;
        }
    }
}
