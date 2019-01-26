using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Boardgame.Controllers
{
    public class BaseController : Controller
    {
        protected readonly AppDbContext _dbContext;

        public BaseController(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        }

        public ActionResult ViewOrJson()
        {
            if (Request.Headers["Accept"][0].Contains("json"))
            {
                var x = new { };

                return Json(x);
            }

            return View();
        }

        public ActionResult ViewOrJson(object model)
        {
            if (Request.Headers["Accept"][0].Contains("json"))
            {
                return Json(model);
            }

            return View(model);
        }

        public ActionResult ViewOrJson(string viewName)
        {
            if (Request.Headers["Accept"][0].Contains("json"))
            {
                var x = new { };
                return Json(x);
            }

            return View();
        }

        public ActionResult ViewOrJson(string viewName, object model)
        {
            if (Request.Headers["Accept"][0].Contains("json"))
            {
                var x = new { };
                return Json(model);
            }

            return View(viewName, model);
        }
    }
}