//Omni-CAPI Sample Code
//Copyright (c) Meta Platforms, Inc. and affiliates.
//All rights reserved.
//This source code is licensed under the MIT license found in the
//LICENSE file in the root directory of this source tree.
namespace prjOmniCapi.mappers;

using Newtonsoft.Json;

public class ColumnMapper
{
    private const string MAPPING_FILE = @"./mappers/?-mapping.json";
    public Dictionary<string, int> Mapping // required property 
    { get; set; }

    protected string? FilePath
    { get; set; }

    // Constructor
    public ColumnMapper (SampleSignalForwarder.Channel channel) {
        this.FilePath = MAPPING_FILE.Replace("?",channel.ToString());

        // https://www.newtonsoft.com/json/help/html/ToObjectComplex.htm
        var json = File.ReadAllText(this.FilePath);

        // dictionary
        var dictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
        this.Mapping = dictionary ?? [];
    }
}