### Design Automation Limitations

There’s no possibility of user interaction when running an app in the cloud. Therefore any Dynamo nodes that open a user interface or need user interaction will not work. However, any UI nodes that remember their selection should work fine.

---
### List of OOTB Nodes That Will Not Work

**Revit > Selection**

- Select Model Element By Category
- Select Model Elements By Category

**ImportExport > Data**

- Data.ImportExcel (use Data.OpenXMLImportExcel instead)
- Data.ExportToExcel (use Data.OpenXMLExportExcel instead)

**Revit > Analytical Automation**

- This is a built-in custom package that must be included in the work item’s input (see notes about Custom Packages)
- You can usually find it here: `C:\Program Files\Autodesk\Revit 2024\AddIns\DynamoForRevit\Revit\nodes\analytical-automation-pkg`

**Revit > Steel Connections**

- This is a built-in custom package that must be included in the work item’s input (see notes about Custom Packages)
- You can usually find it here: `C:\Program Files\Autodesk\Revit 2024\AddIns\DynamoForRevit\Revit\nodes\steel-pkg`