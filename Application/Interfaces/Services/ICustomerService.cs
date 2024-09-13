using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface ICustomerService
    {
        Response<CustomerResponseModel> Register(CustomerRequestModel model);
        Response<CustomerResponseModel> GetCustomer(string email);
        Response<CustomerResponseModel> GetCustomerById(Guid id);
        Response<ICollection<CustomerResponseModel>> GetAllCustomers();
        Response<CustomerResponseModel> ViewCustomerWallet(string email);
        Response<CustomerResponseModel> FundWallet(CustomerRequestModel model, decimal amount);
        Response<CustomerResponseModel> Update(Guid id, CustomerRequestModel model);
        Response<CustomerResponseModel> Delete(string email);
        Response<CustomerResponseModel> ViewProfile(string email);
    }
}
