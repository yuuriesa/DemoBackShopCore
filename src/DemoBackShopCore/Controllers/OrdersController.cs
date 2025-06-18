using DemoBackShopCore.Data;
using Microsoft.AspNetCore.Mvc;

namespace DemoBackShopCore.Controllers
{
    [ApiController]
    [Route("api/Orders")]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public OrdersController
        (
            ApplicationDbContext dbContext
        )
        {
            _dbContext = dbContext;
        }
    }
}