using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly FishContext _fishContext;
        public CustomerRepository(FishContext fishContext)
        {
            _fishContext = fishContext;
        }

        public bool Check(string email)
        {
            var exist = _fishContext.Customers.Any(x => x.Email == email && x.IsDeleted == false);
            return exist;
        }
        public ICollection<Customer> AllCustomers()
        {
           var customers = _fishContext.Customers
                .Where(c => c.IsDeleted == false).ToList();
            return customers;
        }

     

        public Customer Create(Customer customer)
        {
            _fishContext.Customers.Add(customer);
            return customer;
        }

        public bool Delete(string email)
        {
            throw new NotImplementedException();
        }

        public Customer GetByEmail(string email)
        {
            var customer = _fishContext.Customers
                .FirstOrDefault(x => x.Email == email && x.IsDeleted == false);
            return customer;
        }

        public Customer GetById(Guid id)
        {
            var customer = _fishContext.Customers
                .FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            return customer;
        }

        public Customer Update(Customer customer)
        {
            var getCustomer = _fishContext.Customers.Update(customer);
            return customer;
        }
    }
}
