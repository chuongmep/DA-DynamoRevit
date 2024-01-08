using System;
using System.IO;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using DesignAutomationFramework;
using RDADHelper;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExampleImplementation
{
    [Transaction(TransactionMode.Manual),
     Regeneration(RegenerationOption.Manual)]
    public class Example : IExternalDBApplication
    {
        public ExternalDBApplicationResult OnShutdown(ControlledApplication application)
        {
            return ExternalDBApplicationResult.Succeeded;
        }

        public ExternalDBApplicationResult OnStartup(ControlledApplication application)
        {
            Console.WriteLine("<<!>> Example implementation started.");

            DesignAutomationBridge.DesignAutomationReadyEvent += HandleDesignAutomationReadyEvent;
            Actions.OnGraphResultReady += ProcessResult;

            return ExternalDBApplicationResult.Succeeded;
        }

        public void HandleDesignAutomationReadyEvent(object sender, DesignAutomationReadyEventArgs e)
        {
            Console.WriteLine("<<!>> RDADHelper implementation got called by DA event.");

            // optionally do some pre-execution setup
            var rvtFilePath = e.DesignAutomationData.FilePath;
            var workDir = Directory.GetCurrentDirectory();

            // the input folder name must be the same as inside your APS Activity Definition
            var inputFolder = Path.Combine(workDir, "input.zip");

            var inputData = File.ReadAllText(Path.Combine(inputFolder, "input.json"));
            var graphsToRun = JsonConvert.DeserializeObject<List<RunGraphArgs>>(inputData);
            foreach (var graphArgs in graphsToRun)
            {
                Actions.RunDynamoGraph(graphArgs);
            }
            
            ModelPath path = ModelPathUtils.ConvertUserVisiblePathToModelPath("result.rvt");
            var doc = e.DesignAutomationData.RevitDoc;
            if (doc.IsWorkshared)
            {
                WorksharingSaveAsOptions worksharingSaveAsOptions = new WorksharingSaveAsOptions();
                worksharingSaveAsOptions.SaveAsCentral = true;
                SaveAsOptions saveAsOptions = new SaveAsOptions();
                saveAsOptions.SetWorksharingOptions(worksharingSaveAsOptions);
                e.DesignAutomationData.RevitDoc.SaveAs(path, saveAsOptions);
            }
            else
            {
                e.DesignAutomationData.RevitDoc.SaveAs(path, new SaveAsOptions());
            }


            e.Succeeded = true;
        }

        private int n = 1;
        private void ProcessResult(GraphResultArgs args)
        {
            // optionally do some post-execution cleanup and management here

            // make sure you save your graph data for review and troubleshooting
            var graphData = JsonConvert.SerializeObject(args, Formatting.Indented);
            var saveName = $"{n++}_{args.GraphName}.json";
            var savePath = string.IsNullOrWhiteSpace(args.ResultFolder) ?
                saveName : Path.Combine(args.ResultFolder, saveName);
            File.WriteAllText(savePath, graphData);
        }
    }
}
