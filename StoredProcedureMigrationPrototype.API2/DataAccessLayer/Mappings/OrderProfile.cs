

using AutoMapper;
using StoredProcedureMigrationPrototype.API2.DataAccessLayer.Models;
using StoredProcedureMigrationPrototype.Data.Models.Api2.Model;

namespace StoredProcedureMigrationPrototype.API2.DataAccessLayer.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Map Order entity to OrderReadDto
            CreateMap<Order, OrderReadDto>();

            // Map OrderCreateDto to Order entity
            CreateMap<OrderCreateDto, Order>();

            // Map OrderUpdateDto to Order entity
            CreateMap<OrderUpdateDto, Order>();
        }
    }
}
