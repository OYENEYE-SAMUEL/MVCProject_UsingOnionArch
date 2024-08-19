using Application.DTO;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
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
        public FishService(IFishRepository fishRepo, IPondRepository pondRepo, ICurrentUser currentUser, IUnitOfWork unitOfWork)
        {
            _fishRepo = fishRepo;
            _pondRepo = pondRepo;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }
        public Response<FishReponseModel> CreateFish(FishRequestModel model)
        {
            var pond = _pondRepo.GetById(model.FishPonds.First().PondId);
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
                CreatedBy = _currentUser.GetCurrentUser(),
                FishPonds = model.FishPonds,
            };
            _fishRepo.Create(fish);
            _unitOfWork.Save();

            return new Response<FishReponseModel>
            {
                Message = "Created successfully",
                Status = true,
                Value = new FishReponseModel
                {
                    Name = fish.Name,
                    Period = fish.Period,
                    Price = fish.Price,
                    Quantity = fish.Quantity,
                    CreatedBy = fish.CreatedBy,
                    DateCreated = fish.DateCreated,
                    FishPonds = fish.FishPonds.Select(r => new FishPond { Pond = r.Pond}).ToList(),

                }
            };
        }

        public Response<ICollection<FishReponseModel>> GetAllFish()
        {
            var categories = _fishRepo.GetAll();
            var listOfCategories = categories.Select(c => new FishReponseModel
            {
                Name = c.Name,
                Period = c.Period,
                Price = c.Price,
                Quantity = c.Quantity,
                CreatedBy= c.CreatedBy,
                DateCreated = c.DateCreated,
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
                    Name = fish.Name,
                    Period = fish.Period,
                    Price = fish.Price,
                    Quantity = fish.Quantity,
                    CreatedBy = fish.CreatedBy,
                    DateCreated = fish.DateCreated,
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
                    Name = fish.Name,
                    Period = fish.Period,
                    Price = fish.Price,
                    Quantity = fish.Quantity,
                    CategoryType = fish.CategoryType,
                     CreatedBy = fish.CreatedBy,
                     DateCreated = fish.DateCreated,
                     FishPonds = fish.FishPonds.Select(e => new FishPond { Pond = e.Pond}).ToList(),
                }
            };
        }

        public Response<FishReponseModel> GetFishById(Guid id)
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
        }

        public Response<FishReponseModel> UpdateCategory(Guid id, FishRequestModel model)
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
            _fishRepo.Update(fish);
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
                }
            };
        }
    }
}
