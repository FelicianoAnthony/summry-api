using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarterApi.ApiModels.Login;

namespace StarterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagesController : ControllerBase
    {

        [HttpGet]
        public async Task<Dictionary<string, string>> GetPage()
        {
            Dictionary<string, string> response = new();
            string html = "<p> change me and api will rebuild <p>";
            response.Add("content", html);

            return response;
        }
    }



}
