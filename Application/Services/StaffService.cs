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
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepo;
        private readonly IUserRespository _userRespo;
        private readonly IRoleRepository _roleRepo;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        public StaffService(IStaffRepository staffRepo, IUserRespository userRespo, IRoleRepository roleRepo, ICurrentUser currentUser, IUnitOfWork unitOfWork)
        {
            _staffRepo = staffRepo;
            _userRespo = userRespo;
            _roleRepo = roleRepo;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }
        public Response<StaffReponseModel> RegisterStaff(StaffRequestModel model)
        {

            var userExist = _userRespo.Check(model.Email);
            if (userExist)
            {
                return new Response<StaffReponseModel>
                {
                    Message = "User already exist",
                    Status = false,

                };
            }

            var user = new User()
            {
                Email = model.Email,
                Password = model.Password,
                ConfirmedPassword = model.ConfirmedPassword,
                FullName = $"{model.FirstName} {model.LastName}",
            };

            /*var role = _roleRepo.Get("Manager");
            if (role == null)
            {*/
                Role newRole = new Role()
                {
                    Name = "Manager",
                    CreatedBy = model.Email,

                };

                user.UserRoles.Add(new UserRole
                {
                    User = user,
                    RoleId = newRole.Id,
                    Role = newRole,
                    UserId = user.Id
                });
                
           // }

            var exist = _staffRepo.Check(model.Email);
            if (exist)
            {
                return new Response<StaffReponseModel>
                {
                    Message = $"Staff with {model.Email} already exist",
                    Status = false,
                    Value = null
                };
            }

            var staff = new Staff()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Gender = model.Gender,
                Qualification = model.Qualification,
                YearOfExperience = model.YearOfExperience,
                CreatedBy = _currentUser.GetCurrentUser(),
            };
            _userRespo.Create(user);
            _roleRepo.Create(newRole);
            _staffRepo.Create(staff);
            _unitOfWork.Save();

            return new Response<StaffReponseModel>
            {
                Message = "Staff Created Successfully",
                Status = true,
                Value = new StaffReponseModel
                {
                    FirstName = staff.FirstName,
                    LastName = staff.LastName,
                    Address = staff.Address,
                    PhoneNumber = staff.PhoneNumber,
                    Email = staff.Email,
                    Gender = staff.Gender,
                    Qualification = staff.Qualification,
                    YearOfExperience = staff.YearOfExperience,
                    CreatedBy = staff.CreatedBy,
                    DateCreated = staff.DateCreated
                }
            };
        }
        public Response<ICollection<StaffReponseModel>> GetAllStaffs()
        {
            var staffs = _staffRepo.GetAll();

            var listOfStaffs = staffs.Select(x => new StaffReponseModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Gender = x.Gender,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                Address = x.Address,
                CreatedBy = x.CreatedBy,
                DateCreated = x.DateCreated,
                Qualification = x.Qualification,
                YearOfExperience = x.YearOfExperience,

            }).ToList();
            return new Response<ICollection<StaffReponseModel>>
            {
                Message = "All staffs",
                Status = true,
                Value = listOfStaffs
            };
        }

        public Response<StaffReponseModel> GetStaff(string email)
        {
            var staff = _staffRepo.GetByEmail(email);
            if (staff == null)
            {
                return new Response<StaffReponseModel>
                {
                    Message = "staff not found",
                    Status = false,
                    Value = null,
                };
            }

            return new Response<StaffReponseModel>
            {
                Status = true,
                Value = new StaffReponseModel
                {
                    FirstName = staff.FirstName,
                    LastName = staff.LastName,
                    Email = staff.Email,
                    Gender = staff.Gender,
                    PhoneNumber = staff.PhoneNumber,
                    Address = staff.Address,
                    CreatedBy = staff.CreatedBy,
                    DateCreated = staff.DateCreated,
                    Qualification = staff.Qualification,
                    YearOfExperience = staff.YearOfExperience
                }
            };
        }

        public Response<StaffReponseModel> GetStaffById(Guid id)
        {
            var staff = _staffRepo.GetById(id);
            if (staff == null)
            {
                return new Response<StaffReponseModel>
                {
                    Message = "staff not found",
                    Status = false,
                    Value = null,
                };
            }

            return new Response<StaffReponseModel>
            {
                Status = true,
                Value = new StaffReponseModel
                {
                    FirstName = staff.FirstName,
                    LastName = staff.LastName,
                    Email = staff.Email,
                    Gender = staff.Gender,
                    PhoneNumber = staff.PhoneNumber,
                    Address = staff.Address,
                    CreatedBy = staff.CreatedBy,
                    DateCreated = staff.DateCreated,
                    Qualification = staff.Qualification,
                    YearOfExperience = staff.YearOfExperience
                }
            };
        }
    }

}

