using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class PondRequestModel
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string Dimension { get; set; } = default!;
    }

    public class PondResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int PondSize { get; set; } = default!;
        public string Dimension { get; set; } = default!;
        public int SpaceRemain { get; set; }
        public ICollection<FishPond> FishPonds { get; set; } = new HashSet<FishPond>();
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
    }
}
