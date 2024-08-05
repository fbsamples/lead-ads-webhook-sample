//Omni-CAPI Sample Code
//Copyright (c) Meta Platforms, Inc. and affiliates.
//All rights reserved.
//This source code is licensed under the MIT license found in the
//LICENSE file in the root directory of this source tree.
namespace prjOmniCapi.workers;

using prjOmniCapi.pocos;

using Microsoft.VisualBasic.FileIO; // o.O

class WebsiteSignalSender(string filePath) : SignalSender(filePath, SampleSignalForwarder.Channel.web)
{
    private const string EVENT_NAME = "Purchase"; // https://developers.facebook.com/docs/meta-pixel/reference
    private const string COLUMN_SEPARATOR = ";"; // (Excel) field separator (delimiter)
    public override void ReadFile()
    {
        Console.WriteLine(">> ReadFile()");

        // because we could have the separator as valid data
        using (TextFieldParser parser = new (this.sourceFile)) // simplified
        {
            string[]? currentRow;
            int counter = 0;

            parser.HasFieldsEnclosedInQuotes = true; // user agent field
            parser.SetDelimiters(COLUMN_SEPARATOR);

            // https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualbasic.fileio.textfieldparser?view=net-8.0 o.O
            while (!parser.EndOfData)
            {
                currentRow = parser.ReadFields();

                if (currentRow !=null && currentRow.Contains(EVENT_NAME)) // for testing purposes let us ignore events other than "Purchase"
                {
                    // preparing a new payload
                    var p = new SignalPayload();

                    // --- DATA ---
                    p.EventData[0].EventName = currentRow[mapper.Mapping["EVENT_NAME"]];
                    p.EventData[0].EventTime = Int64.Parse(currentRow[mapper.Mapping["EVENT_TIME"]]);
                    p.EventData[0].EventId = currentRow[mapper.Mapping["EVENT_ID"]];
                    p.EventData[0].EventSourceUrl = currentRow[mapper.Mapping["EVENT_SOURCE_URL"]];
                    p.EventData[0].ActionSource = currentRow[mapper.Mapping["ACTION_SOURCE"]];
                    // --- USER DATA ---
                    p.EventData[0].EventUserData.HashedExternalId[0] = currentRow[mapper.Mapping["EXTERNAL_ID"]];
                    p.EventData[0].EventUserData.HashedEmail[0] = currentRow[mapper.Mapping["EMAIL"]];
                    p.EventData[0].EventUserData.HashedUserPhone[0] = currentRow[mapper.Mapping["PHONE"]];
                    p.EventData[0].EventUserData.ClientIpAddress = currentRow[mapper.Mapping["CLIENT_IP_ADDRESS"]];
                    p.EventData[0].EventUserData.ClientUserAgent = currentRow[mapper.Mapping["CLIENT_USER_AGENT"]];
                    p.EventData[0].EventUserData.Fbc = currentRow[mapper.Mapping["FBC"]];
                    p.EventData[0].EventUserData.Fbp = currentRow[mapper.Mapping["FBP"]];
                    // --- CUSTOM DATA ---
                    p.EventData[0].EventCustomData.OrderId = currentRow[mapper.Mapping["ORDER_ID"]];
                    p.EventData[0].EventCustomData.Currency = currentRow[mapper.Mapping["CURRENCY"]];
                    p.EventData[0].EventCustomData.Value =
                        double.Parse(currentRow[mapper.Mapping["VALUE"]], System.Globalization.CultureInfo.InvariantCulture);
                    p.EventData[0].EventCustomData.EventContents[0].Id = currentRow[mapper.Mapping["CONTENT_ID"]];
                    p.EventData[0].EventCustomData.EventContents[0].Quantity =
                        int.Parse(currentRow[mapper.Mapping["CONTENT_QUANTITY"]]);
                    p.EventData[0].EventCustomData.EventContents[0].Delivery = currentRow[mapper.Mapping["CONTENT_DELIVERY"]];

                    this.PrepareBody(p);
                    this.SendToMeta().Wait(); // TODO: you can comment out this line for testing purposes

                    counter++;
                }
            }

            parser.Close(); // not necessary since 'using', but good practice
            Console.WriteLine();
            Console.WriteLine($"Total lines: {counter}.");
        }  
    }
}