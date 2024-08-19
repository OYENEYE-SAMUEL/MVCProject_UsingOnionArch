using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IFishRepository
    {
        bool Check(string name);
        Fish Create(Fish fish);
        Fish GetById(Guid id);
        Fish GetByName(string name);
        Fish Update(Fish fish);
        ICollection<Fish> GetAll();

    }
}
