using Application.Interfaces.Repositories;
using Domain.Entities;
using Google.Protobuf.WellKnownTypes;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly FishContext _fishContext;
        public StaffRepository(FishContext fishContext)
        {
            _fishContext = fishContext;
        }
        public bool Check(string email)
        {
            var exist = _fishContext.Staffs.Any(s => s.Email == email && s.IsDeleted == false);
            return exist;
        }

        public Staff Create(Staff staff)
        {
            _fishContext.Staffs.Add(staff);
            return staff;
        }

        public ICollection<Staff> GetAll()
        {
            var staffs = _fishContext.Staffs
                .Where(s => s.IsDeleted == false).ToList();
            return staffs;
        }

        public Staff GetBy(Expression<Func<Staff, bool>> expression)
        {
            var staff = _fishContext.Staffs.FirstOrDefault
               (s => s.Email == s.Email && s.IsDeleted == false);
            return staff;
        }

        public Staff GetByEmail(string email)
        {
            var staff  = _fishContext.Staffs.FirstOrDefault
                (s => s.Email == email && s.IsDeleted == false);
            return staff;
        } 

        public Staff GetById(Guid id)
        {
            var staff = _fishContext.Staffs
                .FirstOrDefault(e => e.Id == id && e.IsDeleted == false);
                
            return staff;

        }

        public Staff Update(Staff staff)
        {
            var getStaff = _fishContext.Staffs.Update(staff);
            return staff;
        }
    }
}
