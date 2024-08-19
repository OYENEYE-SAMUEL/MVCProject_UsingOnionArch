using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        bool Check(string email);
        Customer Create(Customer customer);
        bool Delete(string email);
        Customer Update(Customer customer);
        Customer GetByEmail(string email);
        Customer GetById(Guid id);
       ICollection<Customer> AllCustomers();

    }
}
