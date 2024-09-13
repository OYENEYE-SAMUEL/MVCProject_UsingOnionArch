using Domain.Entities;

namespace FishApp.ViewModel
{
    public class PondUpdateViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string Dimension { get; set; } = default!;
        public string? PondImage { get; set; }
        public ICollection<FishPond> FishPonds { get; set; } = new HashSet<FishPond>();
    }
}
