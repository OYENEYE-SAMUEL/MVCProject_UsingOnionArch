using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IRoleService
    {
        Response<RoleResponseModel> Create(RoleRequestModel model);
        Response<RoleResponseModel> GetRole(string name);
        Response<ICollection<RoleResponseModel>> GetAll();
    }
}
