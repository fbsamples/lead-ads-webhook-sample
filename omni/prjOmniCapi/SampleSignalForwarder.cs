//Omni-CAPI Sample Code
//Copyright (c) Meta Platforms, Inc. and affiliates.
//All rights reserved.
//This source code is licensed under the MIT license found in the
//LICENSE file in the root directory of this source tree.
namespace prjOmniCapi;

using prjOmniCapi.workers;

public class SampleSignalForwarder
{

    private const string SOURCE_FOLDER = @"./bucket"; // where the .csv files to be processed are stored
    private const string PROCESSED_FOLDER = @"./bucket/processed"; // where the processed .csv files are moved to

    public enum Channel // or System
    {
        store,
        web,
        mobile,
        whatsapp,
        crm
    }

    //-----------------------
    // application entrypoint
    static void Main(string[] args) // TODO: could receive SOURCE/PROCESSED folders here as well
    {
        Console.WriteLine(">> Main() "); // logs to the standard output

        try
        {
            // process each .csv file
            string[] files = Directory.GetFiles(SOURCE_FOLDER, "*.csv", SearchOption.TopDirectoryOnly);

            foreach (string file in files) {
                var filePath = SOURCE_FOLDER + "/" + Path.GetFileName(file);
                var newFilePath = PROCESSED_FOLDER + "/" + Path.GetFileName(file);
                
                // Ingest a file...
                ProcessFile(file);

                File.Move(filePath, newFilePath);
            }
        }
        catch (Exception ex) //TODO: single error-handling routine, could be improved 
        {
            Console.WriteLine("Error in Main(): " + ex.Message);
        }
    }

    //--------------------------
    // process a single csv file
    private static void ProcessFile(string filePath)
    {
        Console.WriteLine(">> ProcessFile() " + filePath);

        SignalSender sender; // abstract/super class

        // Process the file accordingly to the particular channel or system...
        if (filePath.Contains(Channel.store.ToString())) {
            // --- STORE ---
            sender = new StoreSignalSender(filePath);
        } else if (filePath.Contains(Channel.web.ToString())) {
            // --- WEBSITE ---
            sender = new WebsiteSignalSender(filePath);
        // TODO: should add other cases...
        } else {
            Console.WriteLine("Can't recognize channel/system - skipping file.");
            return;
        }

        if (!sender.Check())
            return; // environment variables not set

        sender.ReadFile();
    }
}