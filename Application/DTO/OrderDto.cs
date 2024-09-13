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
        public List<OrderItemRequestModel> OrderItems { get; set; }
            = new List<OrderItemRequestModel>();
    }

    public class OrderItemRequestModel
    {
        public string Key { get; set; } = default!;
        public int Value { get; set; } = default!;
    }

    public class OrderItemResponseModel
    {
        public Guid Id { get; set; }
        public string Key { get; set; } = default!;
        public int Value { get; set; } = default!;
        public Guid OrderId { get; set; }
    }

    public class OrderResponseModel
    {
        public Guid Id { get; set; }
        public Status OrderStatus { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateOrder { get; set; }
        public Guid CustomerId { get; set; }
        public string? CreatedBy { get; set; }
         public Staff? Staff { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<OrderItemResponseModel>? OrderItems { get; set; }
            
        public ICollection<OrderFish> OrderFishes { get; set; } = new HashSet<OrderFish>();
    }

   
}
    

