using Application.DTO;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.Services
{
    public class PondService : IPondService
    {
        private readonly IPondRepository _pondRepo;
        private readonly IFishRepository _fishRepo;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFishPondRepository _fishPondRepo;
        private readonly IFileUploadRepository _file;
        public PondService(IPondRepository pondRepo, IFishRepository fishRepo, ICurrentUser currentUser, IUnitOfWork unitOfWork, IFishPondRepository fishPondRepo, IFileUploadRepository file)
        {
            _fishRepo = fishRepo;
            _pondRepo = pondRepo;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
            _fishPondRepo = fishPondRepo;
            _file = file;
        }
        public Response<PondResponseModel> Create(PondRequestModel model)
        {
            var exists = _pondRepo.Check(model.Name);

            if (exists)
            {
                return new Response<PondResponseModel>
                {
                    Message = $"{model.Name} already exist",
                    Status = false,
                    Value = null
                };
            }

            var dimensions = model.Dimension.Split('x');
            dimensions[0] = dimensions[0].Trim();
            dimensions[1] = dimensions[1].Trim();
            int measure = int.Parse(dimensions[0]);
            int measureDim = int.Parse(dimensions[1]);
            var totalDim = measure * measureDim * 1000;

            var pond = new Pond
            {
                Name = model.Name,
                Description = model.Description,
                PondSize = totalDim,
                Dimension = model.Dimension,
                CreatedBy = _currentUser.GetCurrentUser(),
                PondImage = _file.UploadFile(model.PondImage),
                SpaceRemain = totalDim, 
                
            };
/*
            var fishPond = new FishPond
            {
                Pond = pond,
                PondId = pond.Id,
                 
            };
            _fishPondRepo.Create(fishPond);*/
            _pondRepo.Create(pond);
            _unitOfWork.Save();

            return new Response<PondResponseModel>
            {
                Message = "Pond Created successfully",
                Status = true,
                Value = new PondResponseModel
                {
                    Name = pond.Name,
                    Description = pond.Description,
                    PondSize = pond.PondSize,
                    Dimension = pond.Dimension,
                    SpaceRemain = pond.SpaceRemain,
                    PondImage = pond.PondImage,
                    FishPonds = pond.FishPonds.Select(e => new FishPond { Fish = e.Fish }).ToList()
                }
            };
        }

        public Response<ICollection<PondResponseModel>> GetAll()
        {
            var ponds = _pondRepo.GetAll();
            var listOfPond = ponds.Select(p => new PondResponseModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                PondSize = p.PondSize,
                Dimension = p.Dimension,
                SpaceRemain = p.SpaceRemain,
                CreatedBy = p.CreatedBy,
                PondImage = p.PondImage,
                 FishPonds = p.FishPonds.Select(e => new FishPond
                 {
                     Fish = e.Fish,
                 }).ToList() 

            }).ToList();

            return new Response<ICollection<PondResponseModel>>
            {
                Value = listOfPond,
                Status = true,
                Message = "all ponds"
            };
        }
        public Response<PondResponseModel> GetPondId(Guid id)
        {
            var pond = _pondRepo.GetById(id);
            if (pond == null)
            {
                return new Response<PondResponseModel>
                {
                    Message = "pond does not exist",
                    Status = false,
                };
            }
            return new Response<PondResponseModel>
            {
                Status = true,
                Value = new PondResponseModel
                {
                    Id = pond.Id,
                    Name = pond.Name,
                    Description = pond.Description,
                    PondSize = pond.PondSize,
                    Dimension = pond.Dimension,
                    SpaceRemain = pond.SpaceRemain,
                    CreatedBy = pond.CreatedBy,
                    PondImage = pond.PondImage,
                    FishPonds = pond.FishPonds.Select(e => new FishPond
                    {
                        Fish = e.Fish
                    }).ToList(),
                    DateCreated = pond.DateCreated

                }
            };
        }

        public Response<PondResponseModel> GetPondName(string name)
        {
            var exists = _pondRepo.Check(name);
            if (!exists)
            {
                return new Response<PondResponseModel>
                {
                    Message = $"{name} does not exist",
                    Status = false,
                };
            }
            var pond = _pondRepo.GetName(name);
            return new Response<PondResponseModel>
            {
                Status = true,
                Value = new PondResponseModel
                {
                    Name = pond.Name,
                    Description = pond.Description,
                    PondSize = pond.PondSize,
                    Dimension = pond.Dimension,
                    SpaceRemain = pond.SpaceRemain,
                    CreatedBy = pond.CreatedBy,
                    PondImage = pond.PondImage,
                    FishPonds = pond.FishPonds.Select(e => new FishPond
                    {
                        Fish = e.Fish
                    }).ToList(),
                    DateCreated = pond.DateCreated,
                    
                    

                }
            };
        }

        public Response<PondResponseModel> UpdatePond(Guid id, PondRequestModel model)
        {
            var pond = _pondRepo.GetById(id);
            if (pond == null)
            {
                return new Response<PondResponseModel>
                {
                    Message = "Pond Not Found",
                    Status = false,
                    Value = null
                };
            }
            pond.Name = model.Name;
            pond.Description = model.Description;
            pond.Dimension = model.Dimension;
            pond.PondImage = _file.UploadFile(model.PondImage);
            _pondRepo.Update(pond);
            _unitOfWork.Save();
            return new Response<PondResponseModel>
            {
                Message = "Updated successfully",
                Status = true,
                Value = new PondResponseModel
                {
                    Id = pond.Id,
                    Name = pond.Name,
                    Description = pond.Description,
                    Dimension = pond.Dimension,
                    SpaceRemain = pond.SpaceRemain,
                    PondSize = pond.PondSize,
                    CreatedBy = pond.CreatedBy,
                    DateCreated = pond.DateCreated,
                    PondImage = pond.PondImage,
                    FishPonds = pond.FishPonds.Select(e => new FishPond
                    {
                        Fish = e.Fish
                    }).ToList(),
                }
            };
        }
    }
}
