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
    public class ValuesController : ControllerBase // without view support (MVC), since it is not a SSR
    {
        private readonly ILogger<TestsController> _logger;

        public ValuesController
        (ILogger<TestsController> logger) // dependency injection
        {
            this._logger = logger;            
        }

        // GET api/tests        
        [HttpGet]
        [EnableCors("FBCORSPolicy")]        
        public IActionResult Get()
        {
            return Ok
            (
                new string[] { "value1", "value2"}
            );
        }       
    }
}
