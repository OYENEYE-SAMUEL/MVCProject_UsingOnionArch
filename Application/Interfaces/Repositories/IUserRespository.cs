using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IUserRespository
    {
        bool Check(string email);
        User Create(User user);
        User Get(string email);
        ICollection<User> GetAll();
    }
}
