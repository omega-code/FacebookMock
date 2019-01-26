using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Boardgame.Controllers.API
{
    public class CategoryController : BaseApiController
    {
        public CategoryController(IServiceProvider sp) : base(sp) { }

        public JsonResult ListAll()
        {
            var categories = _dbContext.Categories
                .Where(r => r.ParentId == null)
                .Include(r => r.Children)
                .Include(r => r.Parent)
                .ToArray();

            return Json(categories);
        }
    }
}