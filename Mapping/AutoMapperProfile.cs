
using AutoMapper;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<OrderDTO, Order>().ReverseMap()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id));
        
        CreateMap<ProductDTO, Product>().ReverseMap()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id));
        
        CreateMap<OrderItemDTO, OrderItem>().ReverseMap()
            .ForMember(dest => dest.OrderItemId, opt => opt.MapFrom(src => src.Id));
        
        CreateMap<CustomerDTO, Customer>().ReverseMap()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id));
    
        
    }
    
}