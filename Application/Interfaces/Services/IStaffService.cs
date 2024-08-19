using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IStaffService
    {
        Response<StaffReponseModel> RegisterStaff(StaffRequestModel model);
        Response<StaffReponseModel> GetStaff(string email);
        Response<StaffReponseModel> GetStaffById(Guid id);
        Response<ICollection<StaffReponseModel>> GetAllStaffs();
    }
}
