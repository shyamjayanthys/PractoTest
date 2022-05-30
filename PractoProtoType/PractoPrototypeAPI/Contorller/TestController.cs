using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PractoPrototypeAPI.Logic.BusinessLogic.Doctor;

namespace PractoPrototypeAPI.Contorller
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITest _logic;

        public TestController(ITest logic)
        {
            _logic = logic;
        }
        //[HttpGet]
        //public async Task<IActionResult> TestMethod()
        //{
        //    var operatorUserName = string.Empty;

        //    try
        //    {
        //        return Ok(await _logic.meth());
        //    }

        //    catch (Exception e)
        //    {               
        //        return BadRequest();
        //    }
        //}
    }
}
