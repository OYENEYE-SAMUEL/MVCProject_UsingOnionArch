using Application.DTO;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enum;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IFishRepository _fishRepo;
        private readonly ICurrentUser _currentUser;
        private readonly ICustomerRepository _customerRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderFishRepository _orderFishRepo;
        private readonly IStaffRepository _staffRepo;
        public OrderService(IOrderRepository orderRepo, IFishRepository fishRepo, ICurrentUser currentUser, ICustomerRepository customerRepo, IUnitOfWork unitOfWork, IOrderFishRepository orderFishRepo, IStaffRepository staffRepo)
        {
            _orderRepo = orderRepo;
            _fishRepo = fishRepo;
            _currentUser = currentUser;
            _customerRepo = customerRepo;
            _unitOfWork = unitOfWork;
            _orderFishRepo = orderFishRepo;
            _staffRepo = staffRepo;
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
            /* var fish = _fishRepo.GetByName(model.OrderFishes.Select(f => new OrderFish { Fish = f.Fish}).ToString());*/
            foreach (var item in model.OrderItems)
            {
                var fish = _fishRepo.GetByName(item.Key);
                if (fish == null)
                {
                    return new Response<OrderResponseModel>
                    {
                        Message = "Fish not available presently",
                        Status = false,
                    };

                }
                if (fish.Quantity < item.Value || fish.Quantity == 0)
                {     
                    return new Response<OrderResponseModel>()
                    {
                        Message = $"The remaining quantity of {item.Key} is {fish.Quantity} which is less than the quantity ordered /{item.Key}",
                        Status = false,
                    Value = null
                    };
                }

                fish.Quantity -= item.Value; 
                customer.Wallet -= newPrice;
                _fishRepo.Update(fish);
                _customerRepo.Update(customer);

            }
            var manager = _staffRepo.GetByEmail("sam@gmail.com");
             var order = new Order()
                {
                    OrderStatus = Status.Pending,
                    OrderItems = model.OrderItems.Select(f => new OrderItem()
                    {
                        Key = f.Key,
                        Value = f.Value,
                    }).ToList(),
                    TotalPrice = newPrice,
                    CreatedBy = customer.Email,
                    CustomerId = customer.Id,
                    Staff = manager,
                    
                };

            foreach (var item in model.OrderItems)
            {
                var fish = _fishRepo.GetByName(item.Key);
                OrderFish orderFish = new OrderFish
                {
                    Order = order,
                    OrderId = order.Id,
                    Fish = fish,
                    FishId = fish.Id
                };
                _orderFishRepo.Create(orderFish);
            } 

             _orderRepo.MakeOrder(order);
            _unitOfWork.Save();

            return new Response<OrderResponseModel>()
            {
                Message = "Ordered successfully",
                Status = true,
                Value = new OrderResponseModel
                {
                    OrderStatus = order.OrderStatus,
                    OrderItems = order.OrderItems.Select(f => new OrderItemResponseModel()
                    {
                        Key = f.Key,
                        Value = f.Value,
                    }).ToList(),
                    DateOrder = order.DateOrder,
                    TotalPrice = order.TotalPrice,
                    CreatedBy = order.CreatedBy,
                    CustomerId = order.CustomerId,
                    Staff = order.Staff
                    
                }
            };
        }

        private decimal CalculateTotalCost(OrderRequestModel model)
        {
            decimal total = 0;
            foreach (var item in model.OrderItems)
            {
                var fish = _fishRepo.GetByName(item.Key);
                total += fish.Price * item.Value;
                
            }
            return total;
        }
        public Response<ICollection<OrderResponseModel>> GetAllOrder()
        {
            var orders = _orderRepo.AllOrders();

            if (orders == null || !orders.Any())
            {
                return new Response<ICollection<OrderResponseModel>>
                {
                    Message = "No pending orders found.",
                    Status = false,
                    Value = null
                };
            }
            var listOfOrders = orders.
                Where(f => f.OrderStatus == Status.Pending)
                .Select(f => new OrderResponseModel
                {
                    Id = f.Id,
                    DateOrder = f.DateOrder,
                    CreatedBy = f.CreatedBy,
                    TotalPrice = f.TotalPrice,
                    OrderStatus = f.OrderStatus,
                    OrderItems = f.OrderItems.Select(f => new OrderItemResponseModel
                    {
                        Id = f.Id,
                        Key = f.Key,
                        Value = f.Value,
                        OrderId = f.OrderId
                    }).ToList(),

                }).ToList();

            return new Response<ICollection<OrderResponseModel>>
            {
                Value = listOfOrders,
                Status = true
            };



        }


        //    var listOfOrder = orders.Select(o => new OrderResponseModel
        //    {

        //        Id = o.Id,
        //        DateOrder = o.DateOrder,
        //        CreatedBy = o.CreatedBy,
        //        TotalPrice = o.TotalPrice,
        //        OrderStatus = o.OrderStatus,
        //        OrderItems = o.OrderItems.Select(f => new OrderItemResponseModel
        //        {
        //            Id = f.Id,
        //            Key = f.Key,
        //            Value = f.Value,
        //            OrderId = f.OrderId
        //        }).ToList(),

        //    }).ToList();
        //    return new Response<ICollection<OrderResponseModel>>
        //    {
        //        Value = listOfOrder,
        //        Status = true
        //    };
        //}

        public Response<ICollection<OrderResponseModel>> GetOrderByCustomer(Guid customerId)
        {
            var order = _orderRepo.GetOrderByCustomer(customerId);
            if(order == null)
            {
                return new Response<ICollection<OrderResponseModel>>
                {
                    Message = "Customer instance Not Found",
                    Status = false,
                    Value = null
                };
            }
            

            var listOfOrder = order.Select(o => new OrderResponseModel
            {
                Id = o.Id,
                DateOrder = o.DateOrder,
                TotalPrice = o.TotalPrice,
                OrderStatus = o.OrderStatus == Status.Pending ? Status.Pending: Status.Delivered,
                OrderItems = o.OrderItems.Select(f => new OrderItemResponseModel()
                {
                    Id = f.Id,
                    OrderId = f.OrderId,
                    Key = f.Key,
                    Value = f.Value
                }).ToList(),
                OrderFishes = o.OrderFishes.Select(e => new OrderFish
                {
                    OrderId = e.OrderId,
                    Id = e.Id,
                    Fish = e.Fish,
                }).ToList()
            }).ToList();
            return new Response<ICollection<OrderResponseModel>>
            {
                Status = true,
                Value = listOfOrder,
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
                    Id = order.Id,
                    DateOrder = order.DateOrder,
                    TotalPrice = order.TotalPrice,
                    OrderItems = order.OrderItems.Select(f => new OrderItemResponseModel()
                    {
                        OrderId = f.OrderId,
                        Id = f.Id,
                        Key = f.Key,
                        Value = f.Value
                    }).ToList(),
                    OrderFishes = order.OrderFishes.Select(e => new OrderFish
                    {
                        Fish = e.Fish,
                    }).ToList(),
                    CreatedBy = order.CreatedBy,
                    OrderStatus = order.OrderStatus,
                    Staff = order.Staff
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
            if (order.Staff == null)
            {
                return new Response<OrderResponseModel>
                {
                    Message = "Only managers can approve orders.",
                    Status = false,
                };
            }
            order.OrderStatus = Status.Approved;
            _orderRepo.Update(order);
            _unitOfWork.Save();
            return new Response<OrderResponseModel>
            {
                Message = "Order approved successfully",
                Status = true,
                Value = new OrderResponseModel
                {
                    Id = order.Id,
                    CreatedBy = order.CreatedBy,
                    OrderStatus = order.OrderStatus,
                    DateOrder = order.DateOrder,
                    TotalPrice = order.TotalPrice,
                    Staff = order.Staff,
                    OrderItems = order.OrderItems.Select(f => new OrderItemResponseModel()
                    {
                        Id = f.Id,
                        OrderId = f.OrderId,
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
            _unitOfWork.Save();
            return new Response<OrderResponseModel>
            {
                Message = "Order approved successfully",
                Status = true,
                Value = new OrderResponseModel
                {
                    Id = order.Id,
                    CreatedBy = order.CreatedBy,
                    OrderStatus = order.OrderStatus,
                    DateOrder = order.DateOrder,
                    TotalPrice = order.TotalPrice,
                    Staff = order.Staff,
                    OrderItems = order.OrderItems.Select(f => new OrderItemResponseModel()
                    {
                        Id = f.Id,
                        OrderId = f.OrderId,
                        Key = f.Key,
                        Value = f.Value
                    }).ToList(),
                }
            };
        }
    }
}
