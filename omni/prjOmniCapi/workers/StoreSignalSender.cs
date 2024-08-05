//Omni-CAPI Sample Code
//Copyright (c) Meta Platforms, Inc. and affiliates.
//All rights reserved.
//This source code is licensed under the MIT license found in the
//LICENSE file in the root directory of this source tree.
namespace prjOmniCapi.workers;

using prjOmniCapi.pocos;

class StoreSignalSender(string filePath) : SignalSender(filePath, SampleSignalForwarder.Channel.store)
{
    private const string EVENT_NAME = "Purchase"; // https://developers.facebook.com/docs/meta-pixel/reference
    private const char COLUMN_SEPARATOR = ';'; // (Excel) field separator (delimiter)

    public override void ReadFile()
    {
        Console.WriteLine(">> ReadFile()");

        using (StreamReader reader = new (this.sourceFile)) // simplified syntax
        {
            string? line; // can be null
            int counter = 0;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains(EVENT_NAME)) // for testing purposes let us ignore events other than "Purchase"
                {
                    var values = line.Split(COLUMN_SEPARATOR);

                    // preparing a new payload
                    var p = new SignalPayload();

                    // --- DATA ---
                    p.EventData[0].EventName = values[mapper.Mapping["EVENT_NAME"]];
                    p.EventData[0].EventTime = Int64.Parse(values[mapper.Mapping["EVENT_TIME"]]);
                    p.EventData[0].ActionSource = values[mapper.Mapping["ACTION_SOURCE"]];
                    // --- USER DATA ---
                    p.EventData[0].EventUserData.HashedExternalId[0] = values[mapper.Mapping["EXTERNAL_ID"]];
                    p.EventData[0].EventUserData.HashedEmail[0] = values[mapper.Mapping["EMAIL"]];
                    p.EventData[0].EventUserData.HashedUserPhone[0] = values[mapper.Mapping["PHONE"]];
                    p.EventData[0].EventUserData.HashedFirstName = values[mapper.Mapping["FIRST_NAME"]];
                    p.EventData[0].EventUserData.HashedLastName = values[mapper.Mapping["LAST_NAME"]];
                    p.EventData[0].EventUserData.HashedDateOfBirth = values[mapper.Mapping["DATE_OF_BIRTH"]];
                    p.EventData[0].EventUserData.HashedCity = values[mapper.Mapping["CITY"]];
                    p.EventData[0].EventUserData.HashedZip = values[mapper.Mapping["ZIP"]];
                    p.EventData[0].EventUserData.HashedCountry = values[mapper.Mapping["COUNTRY"]];
                    // --- CUSTOM DATA ---
                    p.EventData[0].EventCustomData.OrderId = values[mapper.Mapping["ORDER_ID"]];
                    p.EventData[0].EventCustomData.Currency = values[mapper.Mapping["CURRENCY"]];
                    p.EventData[0].EventCustomData.Value =
                        double.Parse(values[mapper.Mapping["VALUE"]], System.Globalization.CultureInfo.InvariantCulture);
                    p.EventData[0].EventCustomData.EventContents[0].Id = values[mapper.Mapping["CONTENT_ID"]];
                    p.EventData[0].EventCustomData.EventContents[0].Quantity =
                        int.Parse(values[mapper.Mapping["CONTENT_QUANTITY"]]);

                    this.PrepareBody(p);
                    this.SendToMeta().Wait(); // TODO: you can comment out this line for testing purposes

                    counter++;
                }
            }

            reader.Close(); // not necessary since 'using', but good practice
            Console.WriteLine();
            Console.WriteLine($"Total lines: {counter}.");
        }
    }
}