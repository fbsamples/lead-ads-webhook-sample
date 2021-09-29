//Copyright (c) Facebook, Inc. and its affiliates.
//
//This source code is licensed under the MIT license found in the
//LICENSE file in the root directory of this source tree.

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Cors; // EnableCors...
using Microsoft.Extensions.Logging; // ILogger

using Microsoft.AspNetCore.Http; // DefaultHttpContext();
using Microsoft.AspNetCore.Http.Extensions; // GetEncodedUrl();

using System.Text.Json; // JsonElement

namespace prjFBLeadAds.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")] // important!
    public class WebhooksController : ControllerBase // without view support (MVC), since it is not a SSR
    {
        private readonly ILogger<TestsController> _logger;
        private readonly string SUBSCRIPTION_VERIFICATION = "leadAdsRockPlayer"; // change this to your verification code =)            

        public WebhooksController
        (ILogger<TestsController> logger) // dependency injection
        {
            this._logger = logger;            
        }

        // Ref.: https://developers.facebook.com/docs/graph-api/webhooks/getting-started
        // GET api/webhooks
        [HttpGet]
        [EnableCors("FBCORSPolicy")]
        public IActionResult VerificationRequest( // Creating an Endpoint
            [FromQuery(Name = "hub.mode")] string mode = "", // This value will always be set to subscribe.
            [FromQuery(Name = "hub.challenge")] string challenge = "", // An int you must pass back to us.
            [FromQuery(Name = "hub.verify_token")] string verifyToken = "" // A string that that we grab from the Verify Token field in your app's App Dashboard. 
            ) 
        {
            this._logger.LogInformation("*** public IActionResult VerificationRequest() ***");

            this._logger.LogInformation("mode: " + mode);
            this._logger.LogInformation("challenge: " + challenge);
            this._logger.LogInformation("verify: " + verifyToken);

            if (verifyToken.Equals(SUBSCRIPTION_VERIFICATION)) 
            {
                this._logger.LogInformation("Ok!!!");
                int result = System.Int32.Parse(challenge); //An int you must pass back to us.
                this._logger.LogInformation("challenge: " + result);

                return Ok
                (
                    result //challenge
                );

            } else {
                this._logger.LogInformation("NOT Ok!!!");

                return StatusCode(400, 
                    (new string[] { 
                            "mode: " + mode, 
                            "challenge: " + challenge,
                            "verify: " + verifyToken
                            }
                    ));
            }            
        }

        // POST api/webhooks
        [HttpPost]
        [EnableCors("FBCORSPolicy")]
        public IActionResult YouGotANewLead([FromBody]  JsonElement body)
        {
            this._logger.LogInformation("*** public IActionResult YouGotANewLead() ***");

            var encodedUrl = Request.GetEncodedUrl(); 
            string json = System.Text.Json.JsonSerializer.Serialize(body);

            // now you have your lead id, you can (later) retrieve it (the full information about it)...
            this._logger.LogInformation("******************************************************");
            this._logger.LogInformation("Body: " + json);
            this._logger.LogInformation("******************************************************");
    
            /* just a sample below...
            {
                "object":"page",
                "entry":[
                    {
                        "id":"987987987",
                        "time":1630087927,
                        "changes":[
                            {
                            "value":{
                                "form_id":"456456456",
                                "leadgen_id":"321321321", <=======
                                "created_time":1630087926,
                                "page_id":"123123123"
                            },
                            "field":"leadgen"
                            }
                        ]
                    }
                ]
            }            
            */

            var entry = body.GetProperty("entry");
            string entryString = System.Text.Json.JsonSerializer.Serialize(entry);

            var changes = entry[0].GetProperty("changes");
            string changesString = System.Text.Json.JsonSerializer.Serialize(changes);

            var value = changes[0].GetProperty("value");
            string valueString = System.Text.Json.JsonSerializer.Serialize(value);

            var leadgen_id = value.GetProperty("leadgen_id");
            string leadgen_idString = System.Text.Json.JsonSerializer.Serialize(leadgen_id);

            return Ok
            (
                new string[] {
                    "POST webhooks", 
                    "public IActionResult YouGotANewLead([FromBody] string content)",
                    "encoded url: " + encodedUrl,
                    "this is what we received in the body: " + json,
                    "entry: " + entryString, 
                    "changes: " + changesString, 
                    "value: " + valueString,
                    "leadgen_id: " + leadgen_idString 
                }
            );
        }        
    }
}
