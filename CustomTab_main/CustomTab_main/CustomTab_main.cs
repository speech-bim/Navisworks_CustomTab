using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;
using System.IO;


namespace CustomTab_main
{

    [PluginAttribute("CustomTab_main",
                    "Speech",
                    ToolTip = "Основной плагин. Не запускать",
                    DisplayName = "Don't Click")]
    public class Main : AddInPlugin
    {
        private string textFile_path = "";
        private string Start_Plugin = "No";
        private int proBar_index = 0;
        private String AllLines;
        private static AutoResetEvent resetEvent = new AutoResetEvent(false);
        public const double meter_scale = 0.3048;

        public override int Execute(params string[] parameters)
        {
            try
            {
                if (parameters.Length != 0)
                {
                    // Get properties from received command
                    textFile_path = parameters[0];
                    //Get the path of textfile
                    Start_Plugin = parameters[1];
                    // Start plugin
                    if (Start_Plugin == "Yes")
                    {
                        // Get the active document
                        Document Doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
                        // Select items to add newtab to 
                        IEnumerable<ModelItem> items = Doc.Models.RootItemDescendants;
                        // Read the textfile of the properties
                        AllLines = File.ReadAllText(textFile_path, Encoding.Default);

                        if (AllLines.Length > 0)
                        {
                            string[] Lines = AllLines.Split('\n');
                            int N = Lines.Length;
                            // Add newtab for the selected items
                            Add_newTab(items, Lines, N);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("** ERROR **: " + e.Message);
            }
            return 0;

        }

        public void Add_newTab(IEnumerable<ModelItem> items, string[] Lines, int N)
        {
            // Intialize the parameters 
            ComApi.InwOpState9 oState = ComApiBridge.State;
            string[] OldCategory = new string[N];
            string[] OldParameters = new string[N];
            string[] NewParameters = new string[N];
            string[] Parameters_hasAdded = new string[N];
            int ind = 0;
            Boolean Parameter_isNotRepeated = true;
            ComApi.InwOaProperty[] NewProperties = new ComApi.InwOaProperty[N];
            try
            {
                // Iterate over all the selected items
                foreach (ModelItem oItem in items)
                {
                    if (oItem.Children.Count() > 0)
                    {
                        // Convert the .NET to COM object
                        ComApi.InwOaPath oPath = ComApiBridge.ToInwOaPath(oItem);
                        // Create new property category
                        ComApi.InwOaPropertyVec newPvec = (ComApi.InwOaPropertyVec)oState.ObjectFactory(ComApi.nwEObjectType.eObjectType_nwOaPropertyVec, null, null);
                        // Get properties collection of the path
                        ComApi.InwGUIPropertyNode2 propn = (ComApi.InwGUIPropertyNode2)oState.GetGUIPropertyNode(oPath, true);
                        // Iterate over all properties in textfile
                        ind = 0;
                        for (int i = 1; i < N; i++)
                        {
                            // Check if line is empty
                            if (Lines[i] != null && Lines[i].Length > 1)
                            {                                
                                // Get Category, old property and new property from the line
                                string[] WordsInLine = Lines[i].Split('\t');
                                OldCategory[i] = WordsInLine[0];
                                OldParameters[i] = WordsInLine[1];
                                NewParameters[i] = WordsInLine[2];
                                // By default the property is not repeated
                                Parameter_isNotRepeated = true;
                                // Iterate over properties of the item
                                foreach (PropertyCategory oPC in oItem.PropertyCategories)
                                {
                                    if (oPC.DisplayName == OldCategory[i])
                                    {
                                        foreach (DataProperty oDP in oPC.Properties)
                                        {
                                            if (oDP.DisplayName == OldParameters[i])
                                            {
                                                // Create new property
                                                NewProperties[i] = (ComApi.InwOaProperty)oState.ObjectFactory(ComApi.nwEObjectType.eObjectType_nwOaProperty, null, null);
                                                NewProperties[i].name = NewParameters[i];
                                                NewProperties[i].UserName = NewParameters[i];

                                                // Check if the property had been added
                                                Parameters_hasAdded[ind] = NewParameters[i]; ind = ind + 1;
                                                for (int d = 0; d < ind - 1; d++)
                                                {
                                                    if (NewParameters[i] == Parameters_hasAdded[d])
                                                    {
                                                        Parameter_isNotRepeated = false;
                                                    }
                                                }
                                                //  If the property had not added yet, add it
                                                if (Parameter_isNotRepeated)
                                                {
                                                    // Check datatype of the property
                                                    switch (oDP.Value.DataType)
                                                    {
                                                        case VariantDataType.Boolean:
                                                            bool NewValueBool = oDP.Value.ToBoolean();
                                                            NewProperties[i].value = NewValueBool;
                                                            break;
                                                        case VariantDataType.DateTime:
                                                            DateTime NewValueDate = oDP.Value.ToDateTime();
                                                            NewProperties[i].value = NewValueDate;
                                                            break;
                                                        case VariantDataType.DisplayString:
                                                            string NewValueDString = oDP.Value.ToDisplayString();
                                                            NewProperties[i].value = NewValueDString;
                                                            break;
                                                        case VariantDataType.Double:
                                                            double NewValueDouble = oDP.Value.ToDouble();
                                                            NewProperties[i].value = NewValueDouble;
                                                            break;
                                                        case VariantDataType.DoubleAngle:
                                                            double NewValueAngle = oDP.Value.ToDoubleAngle();
                                                            NewProperties[i].value = NewValueAngle;
                                                            break;
                                                        case VariantDataType.DoubleArea:
                                                            double NewValueArea = oDP.Value.ToDoubleArea();
                                                            NewProperties[i].value = NewValueArea * Math.Pow(meter_scale, 2);
                                                            break;
                                                        case VariantDataType.DoubleLength:
                                                            double NewValueLength = oDP.Value.ToDoubleLength();
                                                            NewProperties[i].value = NewValueLength * meter_scale;
                                                            break;
                                                        case VariantDataType.DoubleVolume:
                                                            double NewValueVolume = oDP.Value.ToDoubleVolume();
                                                            NewProperties[i].value = NewValueVolume * Math.Pow(meter_scale, 3);
                                                            break;
                                                        case VariantDataType.IdentifierString:
                                                            string NewValueIString = oDP.Value.ToIdentifierString();
                                                            NewProperties[i].value = NewValueIString;
                                                            break;
                                                        case VariantDataType.Int32:
                                                            int NewValueInt = oDP.Value.ToInt32();
                                                            NewProperties[i].value = NewValueInt;
                                                            break;
                                                        case VariantDataType.NamedConstant:
                                                            NamedConstant NewValueConst = oDP.Value.ToNamedConstant();
                                                            String s = NewValueConst.DisplayName;
                                                            s = s.Substring(s.IndexOf("\"") + 1);
                                                            s = s.Substring(0, s.IndexOf("\""));

                                                            NewProperties[i].value = s;
                                                            break;
                                                        case VariantDataType.None:
                                                            string NewValueNone = oDP.Value.ToString();
                                                            NewProperties[i].value = NewValueNone;
                                                            break;
                                                        case VariantDataType.Point3D:
                                                            string NewValuePoint = oDP.Value.ToString();
                                                            NewProperties[i].value = NewValuePoint;
                                                            break;
                                                    }
                                                    // Add the new property to the new property category
                                                    newPvec.Properties().Add(NewProperties[i]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        // Add new tab to the object
                        // The name of the newtab is saved in first line in the textfile
                        propn.SetUserDefined(0, Lines[0], "MyAttribute", newPvec);
                    }
                    proBar_index++;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(" ERROR : " + e.Message);
            }
        }
    }
}