## Developing Revit Design Automation Dynamo (RDADynamo)

1. Clone this **DynamoCore**
https://github.com/dimven/Dynamo/tree/RC2.17.4_D4DA
    - internals visible to RDADynamo
    - external python install path
2. Use **Revit 2024.0**
3. Clone this branch of **DynamoRevit**
https://github.com/dimven-adsk/DynamoRevit/tree/D4DA
(Dynamo for Design Automation)
4. Compile DynamoCore
5. Run `src\RDADynamo\update_packages.bat` . This will update the following package files from the newly compiled DynamoCore RDADynamo compatible branch to your DynamoRevit reference packages:
    - DynamoCore.dll (DynamoVisualProgramming.Core)
    - DSCPython.dll (DynamoVisualProgramming.Core)
    - DynamoServices.dll (DynamoVisualProgramming.DynamoServices)
6. Compile DynamoRevit with the Debug_RDA config.
    - Make sure you follow the build steps in the README.md of the repository
7. Create a RDAHelper implementation as per the example implementation repository: https://github.com/tothom/RDADynamo-example-implementation
8. To prepare the app bundle, use the scripts in `src\RDADynamo\bundle\RDADynamo.bundle\Contents`
    1. `1_copy_bundle.bat` copies all necessary files from DynamoCore and RDADynamo and the example implementation. You can then review the content or verify that all necessary files are there before running the next step
    2. `2_zip_bundle.bat` zips the final bundle for uploading to APS.
    3. Please note the included `PackageContents.xml` and `*.addin`  manifest files and adjust them if you need to do any renaming.
9. Add your implementation to the RDADynamo.bundle
    - currently the app bundle scripts assume you’re using the example implementation bundle
    
>💡 This is a temporary measure. We expect that in the future the RDADynamo bundle will be included on the Revit design automation systems and you will only need a tiny implementation bundle that references RDADHelper.dll, similar to the example implementation
    
10. Deploy to APS using an activity similar to the one in the example implementation repository.
11. Create a work item with the required inputs.

>💡 If you want to run this bundle locally, you must have a “RevitEngineConsole” distribution, so that the ASM assemblies are discovered. It is usually located in: C:\Program Files\Common Files\Autodesk Shared\Revit Interoperability 2024\Rx
>
> Alternatively, you can copy all of the necessary ASM.dlls to a folder at the same path.
