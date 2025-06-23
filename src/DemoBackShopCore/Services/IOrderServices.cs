using DemoBackShopCore.Models;
using DemoBackShopCore.Utils;

namespace DemoBackShopCore.Services
{
    public interface IOrderServices
    {
        public IQueryable<Order> GetAll(PaginationFilter paginationFilter);
    }
}