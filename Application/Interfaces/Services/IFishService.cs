using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IFishService
    {
        Response<FishReponseModel> CreateFish(FishRequestModel model);
        Response<FishReponseModel> GetFish(string name);
       /* Response<FishReponseModel> GetFishById(Guid id);*/
        Response<FishReponseModel> GetById(Guid id);
        Response<ICollection<FishReponseModel>> GetAllFish();
        Response<FishReponseModel> UpdateFish(Guid id, FishRequestModel model);
        //Response<FishReponseModel> DeleteCategory(Guid id);
    }
}
