using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRespository
    {
        private readonly FishContext _fishContext;
        public UserRepository(FishContext fishContext)
        {
            _fishContext = fishContext;
        }
        public bool Check(string email)
        {
            var exist = _fishContext.Users.Any(x => x.Email == email && x.IsDeleted == false);
            return exist;
        }

        public User Create(User user)
        {
           var userMan = _fishContext.Users.Add(user);
            return user;
        }

        public User Get(string email)
        {
            var user = _fishContext.Users
                .Include(r => r.UserRoles)
                .ThenInclude(e => e.Role)
                .FirstOrDefault(s => s.Email == email && s.IsDeleted == false);
            return user;
        }

        public ICollection<User> GetAll()
        {
            var users = _fishContext.Users
                .Include(a => a.UserRoles)
                .ThenInclude(a => a.Role)
                .Where(a => a.IsDeleted == false).ToList();
            return users;
        }
    }
}
