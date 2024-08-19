using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class RoleRequestModel
    {
        public string Name { get; set; } = default!;
    }

    public class RoleResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime DateCreated { get; set; }
    }
}
