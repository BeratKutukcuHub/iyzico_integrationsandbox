using AutoMapper;
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
            CreateMap<Customer, CustomerGetDTO>();
            CreateMap<CustomerPostDTO, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Ip, opt => opt.Ignore());

            CreateMap<Product, ProductGetDTO>();
            CreateMap<ProducPostDTO, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ReservedStock, opt => opt.MapFrom(src => 0));
            CreateMap<ProductPutDTO, Product>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Order, OrderGetDTO>();
            CreateMap<OrderPostDTO, Order>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore()) // Backend'de JWT'den alınacak
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()) // Backend'de hesaplanacak
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => OrderStatus.PendingPayment))
                .ForMember(dest => dest.PaidAt, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentId, opt => opt.Ignore());
            CreateMap<OrderPutDTO, Order>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore())
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())
                .ForMember(dest => dest.Currency, opt => opt.Ignore())
                .ForMember(dest => dest.PaidAt, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentId, opt => opt.Ignore());

            CreateMap<Payment, PaymentGetDTO>()
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency.ToString()));
            CreateMap<PaymentPostDTO, Payment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => PaymentStatus.Pending))
                .ForMember(dest => dest.ProviderRef, opt => opt.Ignore())
                .ForMember(dest => dest.RawInitResponse, opt => opt.Ignore())
                .ForMember(dest => dest.RawWebhookPayload, opt => opt.Ignore())
                .ForMember(dest => dest.PaidAt, opt => opt.Ignore());
            CreateMap<PaymentPutDTO, Payment>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.IdempotencyKey, opt => opt.Ignore())
                .ForMember(dest => dest.Provider, opt => opt.Ignore())
                .ForMember(dest => dest.ProviderRef, opt => opt.Ignore())
                .ForMember(dest => dest.Amount, opt => opt.Ignore())
                .ForMember(dest => dest.Currency, opt => opt.Ignore())
                .ForMember(dest => dest.RawInitResponse, opt => opt.Ignore())
                .ForMember(dest => dest.RawWebhookPayload, opt => opt.Ignore())
                .ForMember(dest => dest.PaidAt, opt => opt.Ignore());

            CreateMap<Identity, IdentityGetDTO>();
            CreateMap<IdentityPostDTO, Identity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
            CreateMap<IdentityPutDTO, Identity>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            
            CreateMap<Controllers.LoginDTO, Identity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Username, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Surname, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());
        }
    }
}
