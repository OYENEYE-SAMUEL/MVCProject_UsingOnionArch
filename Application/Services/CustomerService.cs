
using Application.DTO;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IUserRespository _userRespo;
        private readonly IRoleRepository _roleRepo;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        public CustomerService(ICustomerRepository customerRepo, IUserRespository userRespo, IRoleRepository roleRepo, ICurrentUser currentUser, IUnitOfWork unitOfWork)
        {
            _customerRepo = customerRepo;
            _userRespo = userRespo;
            _roleRepo = roleRepo;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }
        public Response<CustomerResponseModel> Register(CustomerRequestModel model)
        {
            var userExist = _userRespo.Check(model.Email);
            if (userExist)
            {
                return new Response<CustomerResponseModel>
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

          /*  var role = _roleRepo.Get("Customer");*/
          /*  if (role == null)
            {*/
                Role newRole = new Role()
                {
                    Name = "Customer",
                    CreatedBy = model.Email,
                };
                user.UserRoles.Add(new UserRole
                {
                    User = user,
                    UserId = newRole.Id,
                    RoleId = newRole.Id,
                    Role = newRole

                });
                
            //}

            var manExist = _customerRepo.Check(model.Email);
            if (manExist)
            {
                return new Response<CustomerResponseModel>
                {
                    Message = "Customer already exist",
                    Status = false
                };
            }

            var customer = new Customer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Address = model.Address,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                CreatedBy = _currentUser.GetCurrentUser(),
                Wallet = model.Wallet

            };

            _userRespo.Create(user);
            _roleRepo.Create(newRole);
            _customerRepo.Create(customer);
            _unitOfWork.Save();

            return new Response<CustomerResponseModel>
            {
                Message = "Created successfully",
                Status = true,
                Value = new CustomerResponseModel
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    Address = customer.Address,
                    Gender = customer.Gender,
                    Wallet = customer.Wallet,
                    PhoneNumber = customer.PhoneNumber,
                }
            };
        }

        public Response<CustomerResponseModel> GetCustomer(string email)
        {
            var exist = _customerRepo.Check(email);
            if (!exist)
            {
                return new Response<CustomerResponseModel>
                {
                    Message = "Customer not found",
                    Status = false,
                    Value = null,
                };
            }
            var customer = _customerRepo.GetByEmail(email);
            return new Response<CustomerResponseModel>
            {
                Status = true,
                Value = new CustomerResponseModel
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    Gender = customer.Gender,
                    PhoneNumber = customer.PhoneNumber,
                    Address = customer.Address,
                    Orders = customer.Orders,
                }
            };
        }

        public Response<ICollection<CustomerResponseModel>> GetAllCustomers()
        {
            var customers = _customerRepo.AllCustomers();

            var listOfCustomers = customers.Select(x => new CustomerResponseModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Gender = x.Gender,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                Address = x.Address,
                Orders = x.Orders,
                    
            }).ToList();
            return new Response<ICollection<CustomerResponseModel>>
            {
                Message = "All customers",
                Status = true,
                Value = listOfCustomers
            };
        }
        public Response<CustomerResponseModel> Delete(string email)
        {
            throw new NotImplementedException();
        }

        public Response<CustomerResponseModel> GetCustomerById(Guid id)
        {
            var customer = _customerRepo.GetById(id);
            if (customer == null)
            {
                return new Response<CustomerResponseModel>
                {
                    Message = "Customer not found",
                    Status = false,
                    Value = null,
                };
            }

            return new Response<CustomerResponseModel>
            {
                Status = true,
                Value = new CustomerResponseModel
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    Gender = customer.Gender,
                    PhoneNumber = customer.PhoneNumber,
                    Address = customer.Address,
                    Orders = customer.Orders,
                }
            };
        }

        

        public Response<CustomerResponseModel> Update(CustomerRequestModel model)
        {
            var customer = _customerRepo.GetByEmail(model.Email);
            if (customer == null)
            {
                return new Response<CustomerResponseModel>
                {
                    Message = "Customer not Found",
                    Status = false,

                };
            }
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Email = model.Email;
            customer.Gender = model.Gender;
            customer.PhoneNumber = model.PhoneNumber;
            customer.Address = model.Address;
            customer.Wallet = model.Wallet;
            return new Response<CustomerResponseModel>
            {
                Message = "Updated successfully",
                Status = true,
                Value = new CustomerResponseModel
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    Gender = customer.Gender,
                    PhoneNumber = customer.PhoneNumber,
                    Address = customer.Address,
                    Wallet = customer.Wallet
                }
            }; 

        }

        public Response<CustomerResponseModel> ViewProfile(string email)
        {
            throw new NotImplementedException();
        }

        public Response<CustomerResponseModel> FundWallet(CustomerRequestModel model, decimal amount)
        {
            var customer = _customerRepo.GetByEmail(model.Email);
            if (customer == null)
            {
                return new Response<CustomerResponseModel>
                {
                    Message = "Customer Not Found",
                    Status = false,
                    Value = null
                };
            }
            customer.Wallet += amount;
            _customerRepo.Update(customer);
            return new Response<CustomerResponseModel>
            {
                Message = "Wallet funded successfully",
                Status = true,
                Value = new CustomerResponseModel
                {
                    Wallet = customer.Wallet
                }
            };
        }
    }
}
