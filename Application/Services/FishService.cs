using Application.DTO;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.Services
{
    public class FishService : IFishService
    {
        private readonly IFishRepository _fishRepo;
        private readonly IPondRepository _pondRepo;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFishPondRepository _fishPondRepo;
        private readonly IFileUploadRepository _file;
        public FishService(IFishRepository fishRepo, IPondRepository pondRepo, ICurrentUser currentUser, IUnitOfWork unitOfWork, IFishPondRepository fishPondRepo, IFileUploadRepository file)
        {
            _fishRepo = fishRepo;
            _pondRepo = pondRepo;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
            _fishPondRepo = fishPondRepo;
            _file = file;
        }
        public Response<FishReponseModel> CreateFish(FishRequestModel model)
        {
           
            var pond = _pondRepo.GetById(model.PondId);
            if (pond == null)
            {
                return new Response<FishReponseModel>
                {
                    Message = "Pond not Available",
                    Status = false,
                    Value = null
                };
            }

            var exists = _fishRepo.Check(model.Name);
            if (exists)
            {
                return new Response<FishReponseModel>
                {
                    Message = "fish already exist",
                    Status = false
                };
            }
            if (pond.SpaceRemain < model.Quantity || pond.SpaceRemain == 0)
            {
                return new Response<FishReponseModel>
                {
                    Message = "The space is not enough to contain the fish",
                    Status = false
                };
            }
            pond.SpaceRemain -= model.Quantity;
            _pondRepo.Update(pond);
            var fish = new Fish
            {
                Name = model.Name,
                Period = model.Period,
                Price = model.Price,
                Quantity = model.Quantity,
                CategoryType = model.CategoryType,
                FishImage = _file.UploadFile(model.FishImage),
                CreatedBy = _currentUser.GetCurrentUser(),
                
            };

            var fishPond = new FishPond
            {
                Fish = fish,
                FishId = fish.Id,
                Pond = pond,
                PondId = pond.Id,
            };
            _fishPondRepo.Create(fishPond);
            /*FishPond fishPond = new FishPond
            {
                Fish = fish,
                FishId = fish.Id,
                Pond = pond,
                PondId = pond.Id,
            };
            fish.FishPonds.Add(fishPond);*/

            _fishRepo.Create(fish);
            _unitOfWork.Save();

            return new Response<FishReponseModel>
            {
                Message = "Fish Created Successfully",
                Status = true,
                Value = new FishReponseModel
                {
                    Name = fish.Name,
                    Period = fish.Period,
                    Price = fish.Price,
                    Quantity = fish.Quantity,
                    CreatedBy = fish.CreatedBy,
                    DateCreated = fish.DateCreated,
                    CategoryType = fish.CategoryType,
                    FishImage = fish.FishImage,
                    FishPonds = fish.FishPonds.Select(r => new FishPond { Pond = r.Pond}).ToList(),

                }
            };
        }

        public Response<ICollection<FishReponseModel>> GetAllFish()
        {
            var categories = _fishRepo.GetAll();
            var listOfCategories = categories.Select(c => new FishReponseModel
            {
                Id = c.Id,
                Name = c.Name,
                Period = c.Period,
                Price = c.Price,
                Quantity = c.Quantity,
                CreatedBy= c.CreatedBy,
                DateCreated = c.DateCreated,
                FishImage = c.FishImage,
                FishPonds = c.FishPonds.Select(t => new FishPond { Pond = t.Pond}).ToList(),
                CategoryType = c.CategoryType,
            }).ToList();

            return new Response<ICollection<FishReponseModel>>
            {
                Message = "All fish",
                Status = true,
                Value = listOfCategories
            };
        }

        public Response<FishReponseModel> GetById(Guid id)
        {
            var fish = _fishRepo.GetById(id);
            if (fish == null)
            {
                return new Response<FishReponseModel>
                {
                    Message = "fish not found",
                    Status = false
                };
            }
            return new Response<FishReponseModel>
            {
                Status = true,
                Value = new FishReponseModel
                {
                    Id = fish.Id,
                    Name = fish.Name,
                    Period = fish.Period,
                    Price = fish.Price,
                    Quantity = fish.Quantity,
                    CreatedBy = fish.CreatedBy,
                    DateCreated = fish.DateCreated,
                    FishImage = fish.FishImage,
                    FishPonds = fish.FishPonds.Select(e => new FishPond { Pond = e.Pond}).ToList(),
                    CategoryType = fish.CategoryType,
                   
                }
            };
        }

        public Response<FishReponseModel> GetFish(string name)
        {
            var exists = _fishRepo.Check(name);
            if (!exists)
            {
                return new Response<FishReponseModel>
                {
                    Message = "fish not found",
                    Status = false
                };
            }
            var fish = _fishRepo.GetByName(name);
            return new Response<FishReponseModel>
            {
                Status = true,
                Value = new FishReponseModel
                {
                    Id = fish.Id,
                    Name = fish.Name,
                    Period = fish.Period,
                    Price = fish.Price,
                    Quantity = fish.Quantity,
                    CategoryType = fish.CategoryType,
                     CreatedBy = fish.CreatedBy,
                     DateCreated = fish.DateCreated,
                     FishImage = fish.FishImage,
                     FishPonds = fish.FishPonds.Select(e => new FishPond { Pond = e.Pond}).ToList(),
                }
            };
        }

        /*public Response<FishReponseModel> GetFishById(Guid id)
        {
            var fish = _fishRepo.GetById(id);
            if (fish == null)
            {
                return new Response<FishReponseModel>
                {
                    Message = "fish not found",
                    Status = false
                };
            }
            return new Response<FishReponseModel>
            {
                Status = true,
                Value = new FishReponseModel
                {
                    Name = fish.Name,
                    Period = fish.Period,
                    Price = fish.Price,
                    Quantity = fish.Quantity,
                    CategoryType = fish.CategoryType,
                    CreatedBy = fish.CreatedBy,
                    DateCreated = fish.DateCreated,
                    FishPonds = fish.FishPonds.Select(e => new FishPond { Pond = e.Pond }).ToList(),
                }
            };
        }*/

        public Response<FishReponseModel> UpdateFish(Guid id, FishRequestModel model)
        {
            var fish = _fishRepo.GetById(id);
            if (fish == null)
            {
                return new Response<FishReponseModel>
                {
                    Message = "Fish Not Found",
                    Status = false
                };
            }
            fish.Name = model.Name;
            fish.Period = model.Period;
            fish.Price = model.Price;
            fish.Quantity = model.Quantity;
            fish.CategoryType = model.CategoryType;
            fish.FishImage = _file.UploadFile(model.FishImage);

            foreach (var item in fish.FishPonds)
            {
                item.PondId = model.PondId;
            }
            _fishRepo.Update(fish);
            _unitOfWork.Save();
            return new Response<FishReponseModel>
            {
                Message = "Updated Successfully",
                Status = true,
                Value = new FishReponseModel
                {
                    Name = fish.Name,
                    Period = fish.Period,
                    Price = fish.Price,
                    Quantity = fish.Quantity,
                    DateCreated = fish.DateCreated,
                    CategoryType = fish.CategoryType,
                    CreatedBy = fish.CreatedBy,
                    FishPonds = fish.FishPonds,
                    FishImage = fish.FishImage,
                }
            };
        }
    }
}
