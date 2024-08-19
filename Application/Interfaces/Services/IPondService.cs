using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IPondService
    {
        Response<PondResponseModel> Create(PondRequestModel model);
        Response<PondResponseModel> GetPondName(string name);
        Response<PondResponseModel> GetPondId(Guid id);
        Response<ICollection<PondResponseModel>> GetAll();
        Response<PondResponseModel> UpdatePond(Guid id, PondRequestModel model);
        //Response<PondResponseModel> DeletePond(string name);
    }
}
