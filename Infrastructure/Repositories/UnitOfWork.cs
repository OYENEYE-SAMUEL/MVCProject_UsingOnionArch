using Application.Interfaces.Repositories;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FishContext _fishContext;
        public UnitOfWork(FishContext fishContext)
        {
            _fishContext = fishContext;
        }
        public int Save()
        {
            var save = _fishContext.SaveChanges();
            return save;

        }
    }
}
