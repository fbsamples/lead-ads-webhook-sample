//Omni-CAPI Sample Code
//Copyright (c) Meta Platforms, Inc. and affiliates.
//All rights reserved.
//This source code is licensed under the MIT license found in the
//LICENSE file in the root directory of this source tree.
namespace prjOmniCapi.workers;

using prjOmniCapi.pocos;
using prjOmniCapi.mappers;

using System;
using Newtonsoft.Json; // https://code.visualstudio.com/docs/csharp/package-management

abstract class SignalSender
{
    // constants
    public const string META_CAPI_ENDPOINT = "https://graph.facebook.com/v{{api version}}/{{dataset id}}/events";
    protected string? apiVersion;
    protected string? datasetId; // "Pixel ID"
    private readonly string? accessToken; // system_user access token (can be generated in Events Manager) - ideally this should be safely stored in a secure location
    private string endpoint;
    protected string sourceFile; // with the event data
    protected string requestData = ""; // request's body
    protected ColumnMapper mapper; // csv -> json mapper

    // constructor
    public SignalSender(string filePath, SampleSignalForwarder.Channel source)
    {
        this.apiVersion = Environment.GetEnvironmentVariable("META_API_VERSION", EnvironmentVariableTarget.Machine); // can be changed to User-scoped
        this.datasetId = Environment.GetEnvironmentVariable("META_DATASET_ID", EnvironmentVariableTarget.Machine);
        this.accessToken = Environment.GetEnvironmentVariable("META_ACCESS_TOKEN", EnvironmentVariableTarget.Machine);
        
        this.endpoint = META_CAPI_ENDPOINT.Replace("{{api version}}", this.apiVersion).Replace("{{dataset id}}", this.datasetId);
        this.sourceFile = filePath;
        this.mapper = new ColumnMapper(source);
    }

    public bool Check()
    {
        Console.WriteLine($">> Check() we're reading the file {this.sourceFile} and then calling Graph API v{this.apiVersion} for the Pixel {this.datasetId} at {this.endpoint}.");
    
        return (this.apiVersion != null && this.datasetId != null && this.accessToken != null); //short-circuit
    }

    // step 1, read the file into objects
    public abstract void ReadFile();

    // step 2, transform the object into json
    protected string PrepareBody(SignalPayload payload)
    {
        // https://www.newtonsoft.com/json/help/html/NullValueHandlingIgnore.htm
        var json = JsonConvert.SerializeObject(payload, Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

        this.requestData = json; // body
        Console.WriteLine(">> PrepareBody() " + json);

        return json; // body
    }

    // step 3, prepare the request and send it to META's endpoint
    protected async Task SendToMeta()
    {
        Console.WriteLine(">> SendToMeta() ");
        
        using var httpClient = new HttpClient();

        // Prepare the request data
        var requestContent = new StringContent(requestData, System.Text.Encoding.UTF8, "application/json");

        // Add the required headers (access token)
        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + this.accessToken);
        
        // Send the POST request
        var response = await httpClient.PostAsync(this.endpoint, requestContent);

        // Read the response from Meta
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseCode = response.StatusCode; // OK = 200

        // Process the response data (log)
        Console.WriteLine("---");
        Console.WriteLine($"Response (from Meta): {response.StatusCode} - {responseBody}");
        Console.WriteLine("---");
    }
}