using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IStaffRepository
    {
        bool Check(string email);
        Staff Create(Staff staff);
        Staff GetById(Guid id);
        Staff GetByEmail(string email);
        Staff Update (Staff staff);
        ICollection<Staff> GetAll();
    }
}
