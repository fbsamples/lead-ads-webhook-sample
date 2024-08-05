//Omni-CAPI Sample Code
//Copyright (c) Meta Platforms, Inc. and affiliates.
//All rights reserved.
//This source code is licensed under the MIT license found in the
//LICENSE file in the root directory of this source tree.
namespace prjOmniCapi.pocos;

using Newtonsoft.Json;

public class SignalPayload
{
    [JsonProperty("data")]
    public Data[] EventData // property
    { get; set; }

    // Default Constructor
    public SignalPayload()
    {
        this.EventData = new Data[1];
        this.EventData[0] = new Data();
    }

    // Inner class
    public class Data
    {
        [JsonProperty("event_name")]
        public string? EventName  // property
        { get; set; }

        [JsonProperty("event_time")]
        public long? EventTime // property
        { get; set; }

        [JsonProperty("event_id")]
        public string? EventId
        { get; set; }

        [JsonProperty("event_source_url")]
        public string? EventSourceUrl
        { get; set; }

        [JsonProperty("action_source")]
        public string? ActionSource // property
        { get; set; }

         [JsonProperty("user_data")]
        public UserData EventUserData // property
        { get; set; }

        [JsonProperty("custom_data")]
        public CustomData EventCustomData // property
        { get; set; }

        public Data() // constructor
        {
            this.EventUserData = new UserData();
            this.EventCustomData = new CustomData();
        }
    }

    // Inner class
    public class UserData
    {
        [JsonProperty("em")]
        public string[] HashedEmail // property
        { get; set; }

        [JsonProperty("ph")]
        public string[] HashedUserPhone // property
        { get; set; }

        [JsonProperty("fn")]
        public string? HashedFirstName // property
        { get; set; }

        [JsonProperty("ln")]
        public string? HashedLastName // property
        { get; set; }

        [JsonProperty("db")]
        public string? HashedDateOfBirth // property
        { get; set; }

        [JsonProperty("ct")]
        public string? HashedCity // property
        { get; set; }

        [JsonProperty("zip")]
        public string? HashedZip // property
        { get; set; }

        [JsonProperty("country")]
        public string? HashedCountry // property
        { get; set; }

        [JsonProperty("external_id")]
        public string[] HashedExternalId // property
        { get; set; }

        [JsonProperty("client_ip_address")]
        public string? ClientIpAddress
        { get; set; }

        [JsonProperty("client_user_agent")]
        public string? ClientUserAgent
        { get; set; }

        [JsonProperty("fbc")]
        public string? Fbc
        { get; set; }

        [JsonProperty("fbp")]
        public string? Fbp
        { get; set; }

        public UserData () // constructor
        {
            this.HashedExternalId = new string[1]; // TODO: could be 3!
            this.HashedEmail = new string[1];
            this.HashedUserPhone = new string[1];
        }
    }

    // Inner class
    public class CustomData
    {
        [JsonProperty("currency")]
        public string? Currency // property
        { get; set; }

        [JsonProperty("value")]
        public double? Value // property
        { get; set; }

        [JsonProperty("contents")]
        public Content[] EventContents // property
        { get; set; }

        [JsonProperty("order_id")]
        public string? OrderId // property
        { get; set; }

        public CustomData() // constructor
        {
            this.EventContents = new Content[1]; // TODO: for testing purposes, we're considering a single-item order
            this.EventContents[0] = new Content();
        }
    }

    // Inner class
    public class Content
    {
        [JsonProperty("id")]
        public string? Id // property
        { get; set; }

        [JsonProperty("quantity")]
        public int? Quantity // property
        { get; set; }

        [JsonProperty("delivery_category")]
        public string? Delivery
        { get; set; }
    }
}