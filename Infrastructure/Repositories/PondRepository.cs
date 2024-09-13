using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PondRepository : IPondRepository
    {
        private readonly FishContext _fishContext;
        public PondRepository(FishContext fishContext)
        {
            _fishContext = fishContext; 
        }
        public bool Check(string name)
        {
            var exist = _fishContext.Ponds.Any(p => p.Name == name && p.IsDeleted == false);
            return exist;
        }

        public Pond Create(Pond pond)
        {
            _fishContext.Ponds.Add(pond);
            return pond;
        }

        public bool Delete(string name)
        {
            throw new NotImplementedException();
        }

        public List<Pond> GetAll()
        {
            var ponds = _fishContext.Ponds
                .Include(e => e.FishPonds)
                .ThenInclude(t => t.Fish)
                .Where(p => p.IsDeleted == false).ToList();
            return ponds;
        }  
      

        public Pond GetById(Guid id)
        {
            var pond = _fishContext.Ponds
                .Include(t => t.FishPonds)
                .ThenInclude(e => e.Fish)
                .FirstOrDefault(p => p.Id == id && p.IsDeleted == false);
            return pond;
        }

        public Pond GetName(string name)
        {
            var pond = _fishContext.Ponds
                .Include(g => g.FishPonds)
                .ThenInclude(h => h.Fish.Name)
                .FirstOrDefault(p => p.Name == name && p.IsDeleted == false);
            return pond;
        }

        public Pond Update(Pond pond)
        {
            var getPond = _fishContext.Ponds.Update(pond);
            return pond;
        }
    }
}
