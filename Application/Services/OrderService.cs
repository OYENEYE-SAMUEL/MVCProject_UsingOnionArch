using Application.DTO;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IFishRepository _fishRepo;
        private readonly ICurrentUser _currentUser;
        private readonly ICustomerRepository _customerRepo;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IOrderRepository orderRepo, IFishRepository fishRepo, ICurrentUser currentUser, ICustomerRepository customerRepo, IUnitOfWork unitOfWork)
        {
            _orderRepo = orderRepo;
            _fishRepo = fishRepo;
            _currentUser = currentUser;
            _customerRepo = customerRepo;
            _unitOfWork = unitOfWork;
        }
        Response<OrderResponseModel> IOrderService.MakeOrder(OrderRequestModel model)
        {
            var customer = _customerRepo.GetByEmail(_currentUser.GetCurrentUser());
            decimal newPrice = CalculateTotalCost(model);

            if (customer.Wallet == 0 || customer.Wallet < newPrice)
            {
                return new Response<OrderResponseModel>()
                {
                    Message = "Insufficient balance! kindly fund your wallet",
                    Status = false,
                    Value = new OrderResponseModel
                    {
                        OrderStatus = Status.Failed
                    }
                };
            }

            foreach (var item in model.OrderFishItems)
            {
                var fishQuan = _fishRepo.GetByName(item.Key);
                if ((fishQuan.Quantity < item.Value || fishQuan.Quantity == 0) && fishQuan == null)
                {
                    /*return new Response<OrderResponseModel>
                    {
                        Message = "Fish not available presently",
                        Status = false,*/
                    return new Response<OrderResponseModel>()
                    {
                        Message = $"The remaining quantity of {item.Key} is {fishQuan.Quantity} which is less than the quantity ordered /{item.Key}",
                        Status = false,
                    Value = null
                    };
                }

                fishQuan.Quantity -= item.Value; 
                _fishRepo.Update(fishQuan);

                customer.Wallet -= newPrice;
                _customerRepo.Update(customer);

            }
             var order = new Order()
                {
                    OrderStatus = Status.Pending,
                    OrderFishItems = model.OrderFishItems.Select(f => new OrderFish()
                    {
                        Key = f.Key,
                        Value = f.Value,
                    }).ToList(),
                    TotalPrice = newPrice,
                    CreatedBy = customer.Email,
                };
                _orderRepo.MakeOrder(order);
            _unitOfWork.Save();

            return new Response<OrderResponseModel>()
            {
                Message = "Ordered successfully",
                Status = true,
                Value = new OrderResponseModel
                {
                    OrderStatus = order.OrderStatus,
                    OrderFishItems = order.OrderFishItems.Select(f => new OrderFishResponseModel()
                    {
                        Key = f.Key,
                        Value = f.Value,
                    }).ToList(),
                    DateOrder = order.DateOrder,
                    TotalPrice = order.TotalPrice,
                    CreatedBy = order.CreatedBy,

                }
            };
        }

        private decimal CalculateTotalCost(OrderRequestModel model)
        {
            decimal total = 0;
            foreach (var item in model.OrderFishItems)
            {
                var fish = _fishRepo.GetByName(item.Key);
                total += fish.Price * item.Value;
            }
            return total;
        }
        public Response<ICollection<OrderResponseModel>> GetAllOrder()
        {
            var orders = _orderRepo.AllOrders();
            var listOfOrder = orders.Select(o => new OrderResponseModel
            {
                DateOrder = o.DateOrder,
                CreatedBy = o.CreatedBy,
                TotalPrice = o.TotalPrice,
                OrderStatus = o.OrderStatus,
                OrderFishItems = o.OrderFishItems.Select(f => new OrderFishResponseModel()
                {
                    Key = f.Key,
                    Value = f.Value
                }).ToList(),
            }).ToList();
            return new Response<ICollection<OrderResponseModel>>
            {
                Value = listOfOrder,
                Status = true
            };
        }

        public Response<OrderResponseModel> GetOrderByCustomer(Guid customerId)
        {
            var order = _orderRepo.GetOrderByCustomer(customerId);
            if (order == null)
            {
                return new Response<OrderResponseModel>
                {
                    Message = "Order not Found",
                    Status = false,
                    Value = null
                };
            }

            return new Response<OrderResponseModel>
            {
                Status = true,
                Value = new OrderResponseModel
                {
                    DateOrder = order.DateOrder,
                    TotalPrice = order.TotalPrice,
                    OrderFishItems = order.OrderFishItems.Select(f => new OrderFishResponseModel()
                    {
                        Key = f.Key,
                        Value = f.Value
                    }).ToList(),
                   OrderStatus = order.OrderStatus,
                }
            };
        }

        public Response<OrderResponseModel> GetOrderById(Guid id)
        {
            var order = _orderRepo.GetOrder(id);
            if (order == null)
            {
                return new Response<OrderResponseModel>
                {
                    Message = "Order not Found",
                    Status = false,
                    Value = null
                };
            }

            return new Response<OrderResponseModel>
            {
                Status = true,
                Value = new OrderResponseModel
                {
                    DateOrder = order.DateOrder,
                    TotalPrice = order.TotalPrice,
                    OrderFishItems = order.OrderFishItems.Select(f => new OrderFishResponseModel()
                    {
                        Key = f.Key,
                        Value = f.Value
                    }).ToList(),
                    CreatedBy = order.CreatedBy,
                    OrderStatus = order.OrderStatus,
                }
            };
        }

        public Response<OrderResponseModel> ApproveOrder(Guid orderId)
        {
            var order = _orderRepo.GetOrder(orderId);
            if (order == null || order.OrderStatus != Status.Pending)
            {
                return new Response<OrderResponseModel>
                {
                    Message = "Order cannot be approved.",
                    Status = false,
                };
            }
            if (order.Staff == null || order.Staff.Email != order.CreatedBy)
            {
                return new Response<OrderResponseModel>
                {
                    Message = "Only managers can approve orders.",
                    Status = false,
                };
            }
            order.OrderStatus = Status.Approved;
            _orderRepo.Update(order);
            return new Response<OrderResponseModel>
            {
                Message = "Order approved successfully",
                Status = true,
                Value = new OrderResponseModel
                {
                    CreatedBy = order.CreatedBy,
                    OrderStatus = order.OrderStatus,
                    DateOrder = order.DateOrder,
                    TotalPrice = order.TotalPrice,
                    OrderFishItems = order.OrderFishItems.Select(f => new OrderFishResponseModel()
                    {
                        Key = f.Key,
                        Value = f.Value
                    }).ToList(),
                }
            };
        }

        public Response<OrderResponseModel> DeliveredOrder(Guid orderId)
        {
            var order = _orderRepo.GetOrder(orderId);
            if (order == null || order.OrderStatus != Status.Approved)
            {
                return new Response<OrderResponseModel>
                {
                    Message = "Order cannot be delivered.",
                    Status = false,
                };
            }
            
            order.OrderStatus = Status.Delivered;
            _orderRepo.Update(order);
            return new Response<OrderResponseModel>
            {
                Message = "Order approved successfully",
                Status = true,
                Value = new OrderResponseModel
                {
                    CreatedBy = order.CreatedBy,
                    OrderStatus = order.OrderStatus,
                    DateOrder = order.DateOrder,
                    TotalPrice = order.TotalPrice,
                    OrderFishItems = order.OrderFishItems.Select(f => new OrderFishResponseModel()
                    {
                        Key = f.Key,
                        Value = f.Value
                    }).ToList(),
                }
            };
        }
    }
}
