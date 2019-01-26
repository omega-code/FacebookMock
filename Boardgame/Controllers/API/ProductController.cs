using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Boardgame.Controllers.API
{
    public class ProductController : BaseApiController
    {
        public ProductController(IServiceProvider sp) : base(sp)
        {
        }

        public JsonResult ListAll()
        {
            var all = _dbContext.Products
                .Include(r => r.Category)
                .ToArray();

            return Json(all);
        }
    }
}