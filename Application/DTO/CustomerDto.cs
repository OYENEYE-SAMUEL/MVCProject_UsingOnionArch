using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class CustomerRequestModel
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Address { get; set; }
        public string PhoneNumber { get; set; } = default!;
        public Gender Gender { get; set; }
        public string Password { get; set; } = default!;
        public string ConfirmedPassword { get; set; } = default!;
        public decimal Wallet { get; set; }
    }


    public class CustomerResponseModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Address { get; set; }
        public string PhoneNumber { get; set; } = default!;
        public Gender Gender { get; set; }
        public decimal Wallet { get; set; }
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
