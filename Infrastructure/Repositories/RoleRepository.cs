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
    public class RoleRepository : IRoleRepository
    {
        private readonly FishContext _fishContext;
        public RoleRepository(FishContext fishContext)
        {
            _fishContext = fishContext;
        }
        public bool Check(string name)
        {
            var exist = _fishContext.Roles.Any(x => x.Name == name && x.IsDeleted == false);
            return exist;
        }

        public Role Create(Role role)
        {
            _fishContext.Roles.Add(role);
            return role;
        }

        public Role Get(string name)
        {
            var role = _fishContext.Roles.FirstOrDefault
               (d => d.Name == name && d.IsDeleted == false);
            return role;
        }

        public ICollection<Role> GetAll()
        {
            var roles = _fishContext.Roles
                .Where(r => r.IsDeleted == false).ToList();
            return roles;
        }
    }
}
