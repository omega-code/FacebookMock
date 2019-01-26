using System;
using Microsoft.AspNetCore.Mvc;

namespace Boardgame.Controllers.API
{
    [Area("Api")]
    public abstract class BaseApiController : BaseController
    {
        public BaseApiController(IServiceProvider sp) : base(sp) { }
    }
}