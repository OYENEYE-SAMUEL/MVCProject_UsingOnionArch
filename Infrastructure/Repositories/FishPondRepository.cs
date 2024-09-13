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
    public class FishPondRepository : IFishPondRepository
    {
        private readonly FishContext _fishContext;
        public FishPondRepository(FishContext fishContext)
        {
            _fishContext = fishContext; 
        }

        public FishPond Create(FishPond entity)
        {
            _fishContext.FishPonds.Add(entity);
            return entity;
        }
    }
}
