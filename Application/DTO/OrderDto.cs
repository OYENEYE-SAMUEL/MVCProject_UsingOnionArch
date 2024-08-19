using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class OrderRequestModel
    {
        public Status OrderStatus { get; set; }
        public ICollection<OrderFishRequestModel> OrderFishItems { get; set; }
            = new HashSet<OrderFishRequestModel>();
    }

    public class OrderFishRequestModel
    {
        public string Key { get; set; } = default!;
        public int Value { get; set; } = default!;
    }

    public class OrderResponseModel
    {
        public Guid Id { get; set; }
        public Status OrderStatus { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateOrder { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = default!;
        public string? CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<OrderFishResponseModel> OrderFishItems { get; set; }
            = new HashSet<OrderFishResponseModel>();
    }

    public class OrderFishResponseModel
    {
        public Guid Id { get; set; }
        public string Key { get; set; } = default!;
        public int Value { get; set; } = default!;
        public Guid OrderId { get; set; }
    }
}
    

