using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class FishRequestModel
    {

        public string Name { get; set; } = default!;
        public string Period { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Guid PondId { get; set; }
        public IFormFile FishImage { get; set; } = default!;
        public CategoryType CategoryType { get; set; }
    }

    public class FishReponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Period { get; set; } = default!;
        public decimal Price { get; set; }
        public double Quantity { get; set; }
        public CategoryType CategoryType { get; set; }
        public string FishImage { get; set; } = default!;
        public ICollection<FishPond> FishPonds { get; set; } = new HashSet<FishPond>();
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
    }
}
