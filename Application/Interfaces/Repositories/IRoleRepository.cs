using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        bool Check(string name);
        Role Create(Role role);
        Role Get(string name);
        ICollection<Role> GetAll();
    }
}
