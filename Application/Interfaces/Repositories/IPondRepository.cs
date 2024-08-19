using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IPondRepository
    {
        bool Check(string name);
        Pond Create(Pond pond);
        Pond GetName(string name);
        Pond GetById(Guid id);
        Pond Update(Pond pond);
        bool Delete(string name);
        List<Pond> GetAll();
    }
}
