using AutoMapper;
using Icarus.DB.Entities;
using Icarus.Model.Product;
using Icarus.Model.User;

namespace Icarus.API.Infrastructure
{
    public class MappingProfile : Profile
    {
        // Hangi DB classı ile hangi ViewModelin map'leneceğini ayarladığımız kısım
        public MappingProfile()
        {
            CreateMap<UserViewModel, User>();
            CreateMap<User, UserViewModel>();

            CreateMap<User, LoginViewModel>();
            CreateMap<LoginViewModel, User>();

            CreateMap<Product, InsertProductViewModel>();
            CreateMap<InsertProductViewModel, Product>();

            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductViewModel, Product>();

            CreateMap<Product, UpdateProductViewModel>();
            CreateMap<UpdateProductViewModel, Product>();
        }
    }
}
