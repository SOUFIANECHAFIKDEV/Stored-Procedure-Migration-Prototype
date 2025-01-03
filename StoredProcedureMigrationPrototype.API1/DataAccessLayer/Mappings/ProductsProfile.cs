using AutoMapper;
using StoredProcedureMigrationPrototype.API1.DataAccessLayer.Models;
using StoredProcedureMigrationPrototype.Data.Models.Api1.Model;

namespace StoredProcedureMigrationPrototype.API1.DataAccessLayer.Mappings
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            // Map Product entity to ProductReadDto
            CreateMap<Product, ProductReadDto>();

            // Map ProductCreateDto to Product entity
            CreateMap<ProductCreateDto, Product>();

            // Map ProductUpdateDto to Product entity
            CreateMap<ProductUpdateDto, Product>();
        }
    }
}