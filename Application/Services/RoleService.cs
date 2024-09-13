using Application.DTO;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepo;
        private readonly IUserRespository _userRespo;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        public RoleService(IRoleRepository roleRepo, IUserRespository userRespo, ICurrentUser currentUser, IUnitOfWork unitOfWork)
        {
            _roleRepo = roleRepo;
            _userRespo = userRespo;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }
        public Response<RoleResponseModel> Create(RoleRequestModel model)
        {
            var exist = _roleRepo.Check(model.Name);
            if (exist)
            {
                return new Response<RoleResponseModel>
                {
                    Message = $"{model.Name} already exist",
                    Status = false,
                    Value = null
                };
            }
            var role = new Role
            {
                Name = model.Name,
                CreatedBy = _currentUser.GetCurrentUser(),
            };
            /*var user = _userRespo.Get(model.Name);*/
            /*user.UserRoles.First().Role.Id = role.Id;*/
            _roleRepo.Create(role);
            _unitOfWork.Save();

            return new Response<RoleResponseModel>
            {
                Message = "Role created successfully",
                Status = true,
                Value = new RoleResponseModel
                {
                    Id = role.Id,
                    Name = role.Name,
                    DateCreated = role.DateCreated,
                }
            };
        }

        public Response<ICollection<RoleResponseModel>> GetAll()
        {
            var roles = _roleRepo.GetAll();
            var listOfRole = roles.Select(x => new RoleResponseModel
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            return new Response<ICollection<RoleResponseModel>>
            {
                Value = listOfRole,
                Status = true,
            };
        }

        public Response<RoleResponseModel> GetRole(string name)
        {
            var exists = _roleRepo.Check(name);
            if (!exists)
            {
                return new Response<RoleResponseModel>
                {
                    Message = $"Role {name} does not exist",
                    Status = false,
                };
            }
            var role = _roleRepo.Get(name);
            return new Response<RoleResponseModel>
            {
                Message = "role found",
                Status = true,
                Value = new RoleResponseModel
                {
                    Id = role.Id,
                    Name = role.Name
                }
            };
        }
    }
}
