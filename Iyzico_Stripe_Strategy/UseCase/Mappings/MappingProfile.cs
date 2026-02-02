using AutoMapper;
using Iyzico_Stripe_Strategy.Controllers;
using Iyzico_Stripe_Strategy.Domain;
using Iyzico_Stripe_Strategy.UseCase.DTOs.GetDTOs;
using Iyzico_Stripe_Strategy.UseCase.DTOs.PostDTOs;
using Iyzico_Stripe_Strategy.UseCase.DTOs.PutDTOs;

namespace Iyzico_Stripe_Strategy.UseCase.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerGetDTO>().ReverseMap();
            CreateMap<CustomerPostDTO, Customer>().ReverseMap();

            CreateMap<Product, ProductGetDTO>().ReverseMap();
            CreateMap<ProducPostDTO, Product>().ReverseMap();
            CreateMap<ProductPutDTO, Product>().ReverseMap();

            CreateMap<Order, OrderGetDTO>().ReverseMap();
            CreateMap<OrderPostDTO, Order>().ReverseMap();
            CreateMap<OrderPutDTO, Order>().ReverseMap();

            CreateMap<Payment, PaymentGetDTO>().ReverseMap();
            CreateMap<PaymentPostDTO, Payment>().ReverseMap();
            CreateMap<PaymentPutDTO, Payment>().ReverseMap();

            CreateMap<Identity, IdentityGetDTO>().ReverseMap();
            CreateMap<LoginDTO, Identity>().ReverseMap();
            CreateMap<IdentityPostDTO, Identity>().ReverseMap();
            CreateMap<IdentityPutDTO, Identity>().ReverseMap();
        }
    }
}
