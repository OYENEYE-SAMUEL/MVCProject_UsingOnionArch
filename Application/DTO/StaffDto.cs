using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class StaffRequestModel
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Address { get; set; }
        public string PhoneNumber { get; set; } = default!;
        public string Qualification { get; set; } = default!;
        public int YearOfExperience { get; set; }
        public Gender Gender { get; set; }
        public string Password { get; set; } = default!;
        public string ConfirmedPassword { get; set; } = default!;
    }
    public class StaffReponseModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Address { get; set; }
        public string PhoneNumber { get; set; } = default!;
        public string Qualification { get; set; } = default!;
        public int YearOfExperience { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }

    }

}


