using System;
using System.Collections.Generic;

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

using Autodesk.Aec.PropertyData.DatabaseServices;



namespace SetPWVarsACAD
{
    class MyFunctions
    {
        // Property Set Definitions 
        public static ObjectId GetPropertySetDefinitionIdByName(string psdName)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            ObjectId psdId = ObjectId.Null;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            Autodesk.AutoCAD.DatabaseServices.TransactionManager tm = db.TransactionManager;

            using (Transaction tr = tm.StartTransaction())
            {
                try
                {
                    DictionaryPropertySetDefinitions psdDict = new DictionaryPropertySetDefinitions(db);
                    if (psdDict.Has(psdName, tr))
                    {
                        psdId = psdDict.GetAt(psdName);
                    }
                }
                catch
                {
                    ed.WriteMessage("\n GetPropertySetDefinitionIdByName failed");
                }
                tr.Commit();
                return psdId;
            }
        }


        public static ObjectId CreatePropertySetDefinition(string psdName)
            
            {
            int lProjectNo = 0;
            int lDocumentNo = 0;
            if (1 == PWWrapper.aaApi_SelectDocument(lProjectNo, lDocumentNo))
            { 

            ObjectId psdId;

            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            Autodesk.AutoCAD.DatabaseServices.TransactionManager tm = db.TransactionManager;

            DictionaryPropertySetDefinitions dict = new DictionaryPropertySetDefinitions(db);


            // Check for existing propert set definition ... If so just return its ObjectId.
            psdId = GetPropertySetDefinitionIdByName(psdName);

            if (psdId != ObjectId.Null)
            {
                ed.WriteMessage("\n Property set definition {0} exist", psdName);
                return psdId;
                // check version and correctness not implemented
            }
            else
            {
                // Create the new property set definition;
                PropertySetDefinition psd = new PropertySetDefinition();
                psd.SetToStandard(db);
                psd.SubSetDatabaseDefaults(db);
                psd.AlternateName = psdName;
                psd.IsLocked = true;
                psd.IsVisible = false;
                psd.IsWriteable = true;

                // Setup an array of objects that this property set definition will apply to
                System.Collections.Specialized.StringCollection appliesto = new System.Collections.Specialized.StringCollection
                {
                    "AcDb3dSolid"
                };
                psd.SetAppliesToFilter(appliesto, false);

                // Add the property set definition to the dictionary to make formula property work correctly
                using (Transaction tr = tm.StartTransaction())
                {
                    dict.AddNewRecord(psdName, psd);
                    tr.AddNewlyCreatedDBObject(psd, true);

                    // Properties (managed by app)
                    PropertyDefinition def;

                    // Visable properties (exposed to user)

                        def = new PropertyDefinition();
                        def.SetToStandard(db);
                        def.SubSetDatabaseDefaults(db);
                        def.Name = "PWVAR_VAULTID";
                        def.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                        def.DefaultData = $"{lProjectNo}";
                        def.IsVisible = true;
                        psd.Definitions.Add(def);

                        def = new PropertyDefinition();
                        def.SetToStandard(db);
                        def.SubSetDatabaseDefaults(db);
                        def.Name = "PWVAR_DOCID";
                        def.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                        def.DefaultData = $"{lDocumentNo}";
                        def.IsVisible = true;
                        psd.Definitions.Add(def);

                        def = new PropertyDefinition();
                        def.SetToStandard(db);
                        def.SubSetDatabaseDefaults(db);
                        def.Name = "PWVAR_ORIGINALNO";
                        def.DataType = Autodesk.Aec.PropertyData.DataType.Real;
                        def.DefaultData = $"{PWWrapper.aaApi_GetDocumentNumericProperty(PWWrapper.DocumentProperty.OriginalNumber, 0)}";
                        def.IsVisible = true;
                        psd.Definitions.Add(def);

                        def = new PropertyDefinition();
                        def.SetToStandard(db);
                        def.SubSetDatabaseDefaults(db);
                        def.Name = "PWVAR_DOCNAME";
                        def.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                        def.DefaultData = $"{PWWrapper.aaApi_GetDocumentStringProperty(PWWrapper.DocumentProperty.Name, 0)}";
                        def.IsVisible = true;
                        psd.Definitions.Add(def);

                        def = new PropertyDefinition();
                        def.SetToStandard(db);
                        def.SubSetDatabaseDefaults(db);
                        def.Name = "PWVAR_FILENAME";
                        def.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                        def.DefaultData = $"{PWWrapper.aaApi_GetDocumentStringProperty(PWWrapper.DocumentProperty.FileName, 0)}";
                        def.IsVisible = true;
                        psd.Definitions.Add(def);

                        def = new PropertyDefinition();
                        def.SetToStandard(db);
                        def.SubSetDatabaseDefaults(db);
                        def.Name = "PWVAR_DOCDESC";
                        def.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                        def.DefaultData = $"{PWWrapper.aaApi_GetDocumentStringProperty(PWWrapper.DocumentProperty.Desc, 0)}";
                        def.IsVisible = true;
                        psd.Definitions.Add(def);

                        def = new PropertyDefinition();
                        def.SetToStandard(db);
                        def.SubSetDatabaseDefaults(db);
                        def.Name = "PWVAR_DOCVERSION";
                        def.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                        def.DefaultData = $"{PWWrapper.aaApi_GetDocumentStringProperty(PWWrapper.DocumentProperty.Version, 0)}";
                        def.IsVisible = true;
                        psd.Definitions.Add(def);

                        def = new PropertyDefinition();
                        def.SetToStandard(db);
                        def.SubSetDatabaseDefaults(db);
                        def.Name = "PWVAR_DOCCREATETIME";
                        def.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                        def.DefaultData = $"{PWWrapper.aaApi_GetDocumentStringProperty(PWWrapper.DocumentProperty.CreateTime, 0)}";
                        def.IsVisible = true;
                        psd.Definitions.Add(def);

                        def = new PropertyDefinition();
                        def.SetToStandard(db);
                        def.SubSetDatabaseDefaults(db);
                        def.Name = "PWVAR_DOCUPDATETIME";
                        def.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                        def.DefaultData = $"{PWWrapper.aaApi_GetDocumentStringProperty(PWWrapper.DocumentProperty.UpdateTime, 0)}";
                        def.IsVisible = true;
                        psd.Definitions.Add(def);

                        def = new PropertyDefinition();
                        def.SetToStandard(db);
                        def.SubSetDatabaseDefaults(db);
                        def.Name = "PWVAR_DOCFILEUPDATETIME";
                        def.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                        def.DefaultData = $"{PWWrapper.aaApi_GetDocumentStringProperty(PWWrapper.DocumentProperty.FileUpdateTime, 0)}";
                        def.IsVisible = true;
                        psd.Definitions.Add(def);

                        def = new PropertyDefinition();
                        def.SetToStandard(db);
                        def.SubSetDatabaseDefaults(db);
                        def.Name = "PWVAR_DOCLASTRTLOCKTIME";
                        def.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                        def.DefaultData = $"{PWWrapper.aaApi_GetDocumentStringProperty(PWWrapper.DocumentProperty.LastRtLockTime, 0)}";
                        def.IsVisible = true;
                        psd.Definitions.Add(def);

                        def = new PropertyDefinition();
                        def.SetToStandard(db);
                        def.SubSetDatabaseDefaults(db);
                        def.Name = "PWVAR_DOCWORKFLOW";
                        def.DataType = Autodesk.Aec.PropertyData.DataType.Real;
                        def.DefaultData = $"{PWWrapper.GetWorkflowName(PWWrapper.aaApi_GetDocumentNumericProperty(PWWrapper.DocumentProperty.WorkFlowID, 0))}";
                        def.IsVisible = true;
                        psd.Definitions.Add(def);

                        def = new PropertyDefinition();
                        def.SetToStandard(db);
                        def.SubSetDatabaseDefaults(db);
                        def.Name = "PWVAR_DOCWORKFLOWSTATE";
                        def.DataType = Autodesk.Aec.PropertyData.DataType.Real;
                        def.DefaultData = $"{PWWrapper.GetStateName(PWWrapper.aaApi_GetDocumentNumericProperty(PWWrapper.DocumentProperty.StateID, 0))}";
                        def.IsVisible = true;
                        psd.Definitions.Add(def);

                        def = new PropertyDefinition();
                        def.SetToStandard(db);
                        def.SubSetDatabaseDefaults(db);
                        def.Name = "PWVAR_VAULTPATHNAME";
                        def.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                        def.DefaultData = $"{PWWrapper.GetProjectNamePath2(lProjectNo)}";
                        def.IsVisible = true;
                        psd.Definitions.Add(def);

                        def = new PropertyDefinition();
                        def.SetToStandard(db);
                        def.SubSetDatabaseDefaults(db);
                        def.Name = "PWVAR_FULLFILEPATHNAME";
                        def.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                        def.DefaultData = $"{PWWrapper.GetDocumentNamePath(lProjectNo, lDocumentNo)}";
                        def.IsVisible = true;
                        psd.Definitions.Add(def);

                        def = new PropertyDefinition();
                        def.SetToStandard(db);
                        def.SubSetDatabaseDefaults(db);
                        def.Name = "PWVAR_DOCGUID";
                        def.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                        def.DefaultData = $"{PWWrapper.GetGuidStringFromIds(lProjectNo, lDocumentNo)}";
                        def.IsVisible = true;
                        psd.Definitions.Add(def);

                        def = new PropertyDefinition();
                        def.SetToStandard(db);
                        def.SubSetDatabaseDefaults(db);
                        def.Name = "PWVAR_DOCLINK";
                        def.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                        def.DefaultData = $"{PWWrapper.GetDocumentURL(lProjectNo, lDocumentNo)}";
                        def.IsVisible = true;
                        psd.Definitions.Add(def);

                        if (1 == PWWrapper.aaApi_SelectProject(lProjectNo))
                        {
                            def = new PropertyDefinition();
                            def.SetToStandard(db);
                            def.SubSetDatabaseDefaults(db);
                            def.Name = "PWVAR_VAULTNAME";
                            def.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                            def.DefaultData = $"{PWWrapper.aaApi_GetProjectStringProperty(PWWrapper.ProjectProperty.Name, 0)}";
                            def.IsVisible = true;
                            psd.Definitions.Add(def);

                            def = new PropertyDefinition();
                            def.SetToStandard(db);
                            def.SubSetDatabaseDefaults(db);
                            def.Name = "PWVAR_VAULTDESC";
                            def.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                            def.DefaultData = $"{PWWrapper.aaApi_GetProjectStringProperty(PWWrapper.ProjectProperty.Desc, 0)}";
                            def.IsVisible = true;
                            psd.Definitions.Add(def);
                        }

                        System.Collections.Generic.SortedList<string, string> slProps = PWWrapper.GetProjectPropertyValuesInList(lProjectNo);

                        foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in slProps)
                        {
                            def = new PropertyDefinition();
                            def.SetToStandard(db);
                            def.SubSetDatabaseDefaults(db);
                            def.Name = $"PWVAR_PROJPROP_{kvp.Key.ToUpper().Replace(" ", "_")}";
                            def.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                            def.DefaultData = kvp.Value;
                            def.IsVisible = true;
                            psd.Definitions.Add(def);
                        }

                        System.Collections.Generic.SortedList<string, string> slAttrs = PWWrapper.GetAllAttributeColumnValuesInList(lProjectNo, lDocumentNo);

                        foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in slAttrs)
                        {
                            def = new PropertyDefinition();
                            def.SetToStandard(db);
                            def.SubSetDatabaseDefaults(db);
                            def.Name = $"PWVAR_ATTR_{kvp.Key.ToUpper().Replace(" ", "_")}";
                            def.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                            def.DefaultData = kvp.Value;
                            def.IsVisible = true;
                            psd.Definitions.Add(def);
                        }

                        tr.Commit();

                    psdId = psd.ObjectId;
                    return psdId;
                }
            }
        }


        
        }

        private bool mcmMain_GetDocumentIdByFilePath(string sFileName, int iValidateWithChkl,
        ref int iProjectNo, ref int iDocumentNo)
        {
            bool bRetVal = false;

            Guid[] docGuids = new Guid[1];
            int iNumGuids = 0;

            try
            {
                IntPtr pGuid = IntPtr.Zero;

                int iRetVal = PWWrapper.aaApi_GetGuidsFromFileName(ref pGuid, ref iNumGuids, sFileName, iValidateWithChkl);

                if (iNumGuids == 1)
                {
                    Guid docGuid = (Guid)System.Runtime.InteropServices.Marshal.PtrToStructure(pGuid, typeof(Guid));

                    if (1 == PWWrapper.aaApi_GUIDSelectDocument(ref docGuid))
                    {
                        bRetVal = true;

                        iProjectNo =
                            PWWrapper.aaApi_GetDocumentNumericProperty(PWWrapper.DocumentProperty.ProjectID, 0);
                        iDocumentNo =
                            PWWrapper.aaApi_GetDocumentNumericProperty(PWWrapper.DocumentProperty.ID, 0);
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                BPSUtilities.WriteLog($"Error: {ex.Message}\n{ex.StackTrace}");
            }

            return bRetVal;
        }
    }


}
