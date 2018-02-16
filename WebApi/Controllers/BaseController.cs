using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    
    [Produces("application/json")]
    [Authorize]
    public class BaseController : Controller
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult BuildJsonResponse(int responseCode , string message , object data = null)
        {
            string status = string.Empty;
            switch (responseCode)
            {
                case 200:
                    status = "OK";
                    break;
                case 400:
                    status = "NOT FOUND";
                    break;
                case 401:
                    status = "UNAUTHORIZED";
                    break;
            }
            return Json(new
            {
                Status = status,
                Message = message,
                Data = data
            });
        }
    }
}