using System;
using System.Windows.Forms;
using Autodesk.Navisworks.Api.Plugins;


namespace CustomTab
{
    [AddInPlugin(AddInLocation.AddIn,
            CanToggle = true,
            LoadForCanExecute = true,
            CallCanExecute = CallCanExecute.Always,
            Icon = @"C:\Program Files\Autodesk\Navisworks Manage 2019\Plugins\CustomTab.Speech\icon.png", 
            LargeIcon = @"C:\Program Files\Autodesk\Navisworks Manage 2019\Plugins\CustomTab.Speech\icon.png",
            Shortcut = "Ctrl+Shift+P",
            ShortcutWindowTypes = "")]

    [PluginAttribute("CustomTab.CustomTab", 
                    "Speech", 
                    ToolTip = "Добавление пользовательской панели свойств", 
                    DisplayName = "Custom Tab")]

    public class CustomTab : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
        try {
                // Open Plugin Dialog that will run the Main Plugin 
                OpenPluginWindow dialog = new OpenPluginWindow();
                dialog.Topmost = true;
                dialog.ShowDialog();
            }
        catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return 0;
        }
    }
}