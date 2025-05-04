using Mango.Service.OrderAPI.Models;
using Mango.Service.OrderAPI.Models.Dto;

namespace Mango.Services.OrderAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
