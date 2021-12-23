// (C) Copyright 2021 by Chris J Andrew (chris.andrew84@gmail.com)
//
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;

using ObjectId = Autodesk.AutoCAD.DatabaseServices.ObjectId;


// This line is not mandatory, but improves loading performances
[assembly: ExtensionApplication(typeof(SetPWVarsACAD.MyPlugin))]

namespace SetPWVarsACAD
{
    
    public class MyPlugin : IExtensionApplication
    {
        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        static public string psdName = "ProjectWise Properties";


        void IExtensionApplication.Initialize()
        {
            // Check ParameterSetDefinition exist 
            ObjectId psdId = MyFunctions.GetPropertySetDefinitionIdByName(psdName);
            if (psdId == ObjectId.Null)
            {
                MyFunctions.CreatePropertySetDefinition(MyPlugin.psdName);
                ed.WriteMessage("\n Property set defenition {0} created", psdName);
            }


        }

        void IExtensionApplication.Terminate()
        {

        }

    }

}
