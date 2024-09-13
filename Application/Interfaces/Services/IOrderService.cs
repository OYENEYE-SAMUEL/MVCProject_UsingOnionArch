using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IOrderService
    {
        Response<OrderResponseModel> MakeOrder(OrderRequestModel model);
        Response<ICollection<OrderResponseModel>> GetOrderByCustomer(Guid customerId);
        Response<OrderResponseModel> GetOrderById(Guid id);
        Response<OrderResponseModel> ApproveOrder(Guid orderId);
        Response<OrderResponseModel> DeliveredOrder(Guid orderId);
        Response<ICollection<OrderResponseModel>> GetAllOrder();
    }
}
