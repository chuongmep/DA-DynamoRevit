using Autodesk.Forge;
using Autodesk.Forge.DesignAutomation.Model;
using DA_DynamoRevit;
using NUnit.Framework;
using Alias = DA_DynamoRevit.Alias;
using Engine = DA_DynamoRevit.Engine;

namespace DA_UnitTest;

public class DaDynamoRevitTest
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase("b.ec0f8261-aeca-4ab9-a1a5-5845f952b17d",
        "urn:adsk.wipprod:fs.file:vf.Od8txDbKSSelToVg1oc1VA?version=13")]
    [Test]
    public async Task DynamoDesignAutomationTest(string projectId, string versionId)
    {
        string bundlePath =
            @"../../../../../samples/getting-data-from-revit/D4DA.bundle.zip";
        string inputZipPath =
            @"../../../../../samples/getting-data-from-revit/input.zip";
        Console.WriteLine(bundlePath);
        DesignAutomateConfiguration configuration = new DesignAutomateConfiguration()
        {
            AppName = "TestDynamoRevitDA",
            NickName = "chuong",
            Version = DA_DynamoRevit.Version.v2024,
            Engine = Engine.Revit,
            Alias = Alias.DEV,
            ActivityName = "TestDynamoRevitDAActivity",
            ActivityDescription = "TestDynamo Revit Design Automation",
            PackageZipPath = bundlePath,
            BundleDescription = "TestDynamo Revit Design Automation",
            ResultFileName = "result",
            ResultFileExt = ".zip"
        };
        DynamoRevitDesignAutomate dynamoRevitDesignAutomate = new DynamoRevitDesignAutomate(configuration);
        string forgeToken2Leg =
            await Authentication.Get2LeggedToken(configuration.ClientId, configuration.ClientSecret);
        Scope[] scope = new Scope[]
            { Scope.DataRead, Scope.DataWrite, Scope.DataCreate, Scope.BucketRead, Scope.BucketCreate, Scope.CodeAll };
        Status executeJob =
            await dynamoRevitDesignAutomate.ExecuteJob(forgeToken2Leg, projectId, versionId,
                inputZipPath);
        Assert.IsTrue(executeJob == Status.Success);
    }
}