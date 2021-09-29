//Copyright (c) Facebook, Inc. and its affiliates.
//
//This source code is licensed under the MIT license found in the
//LICENSE file in the root directory of this source tree.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Cors; // EnableCors...
using Microsoft.Extensions.Logging; // ILogger

using Microsoft.AspNetCore.Http; // DefaultHttpContext();

namespace prjFBLeadAds.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")] // important!
    public class TestsController : ControllerBase // without view support (MVC), since it is not a SSR
    {
        private readonly ILogger<TestsController> _logger;        
        private readonly string APP_VERSION = "0.2.4";

        public TestsController
        (ILogger<TestsController> logger) // dependency injection
        {
            this._logger = logger;            
        }

        // GET api/tests        
        [HttpGet]
        [EnableCors("FBCORSPolicy")]        
        public IActionResult Get()
        {
            var protocol = HttpContext.Request.IsHttps ? "https://" : "http://";
            var domain = HttpContext.Request.Host;
            var path = HttpContext.Request.Path;
            var query = HttpContext.Request.QueryString;            
            var pathAndQuery = path + query;
            var url = protocol + domain + pathAndQuery;
            
            this._logger.LogInformation("Path and Query: " + pathAndQuery);
            
            return Ok
            (
                new string[] { "value1", "value2", "some other value", 
                    "Path and Query: " + pathAndQuery,
                    "Url: " + url}
            );
        }

        // GET api/tests/version
        [HttpGet("version")]
        public IActionResult Version() // https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-3.1
        {
            this._logger.LogInformation("*** public IActionResult Version() ***");
            this._logger.LogInformation("Versão: " + APP_VERSION);

            return Ok
            (
                new { version = APP_VERSION }
            );
        }

        // this one below is not published on the API Gateway...
        // GET api/tests/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
    }
}
