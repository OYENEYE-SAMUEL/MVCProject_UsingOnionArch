using Application.DTO;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRespository _userRepo;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUserRespository userRepo, IUnitOfWork unitOfWork)
        {
            _userRepo = userRepo;  
            _unitOfWork = unitOfWork;
        }

        public Response<UserResponseModel> Login(UserRequestModel model)
        {
            var user = _userRepo.Get(model.Email);
            if (user == null)
            {
                return new Response<UserResponseModel>
                {
                    Message = "User Not Found",
                    Status = false,
                };
            }
          /*  var salt = BCrypt.Net.BCrypt.GenerateSalt(20);*/
            var hashPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);
            if(!hashPassword)
            {
                return new Response<UserResponseModel>
                {
                    Message = "Invalid Crediential",
                    Status = false
                };
            }

            return new Response<UserResponseModel>
            {
                Message = "Login Successfully",
                Status = true,
                Value = new UserResponseModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    UserRoles = user.UserRoles.Select(r => new  Role
                    {
                        Id = r.Id,
                        Name = r.Role.Name,
                    }).ToList()

                    //UserRoles = user.UserRoles.Select(t => new UserRole
                    //{
                    //     Id = t.Id,
                    //    Role = t.Role,
                    //}).ToList()

                }
            };
        }
    }
}
