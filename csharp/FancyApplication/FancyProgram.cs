using System.Xml;

public class FancyProgram
{
    public static void Main(string[] args)
    {
        var program = new FancyProgram();
        var mainDom = new XmlDocument();
        if (args.Length > 0)
        {
            program.ExecuteFancyCommand("App.fancy", args, mainDom);
        }
        else
        {
            Console.WriteLine("Executing a standard command");
            if (mainDom == null)
                return;

            var commandName = "App.standard";
            var ndCmd =
                mainDom.SelectSingleNode($"MainMenu/Commands/Command[@id='{commandName}']");
            if (ndCmd == null)
                return;

            var ndToolId = ndCmd.SelectSingleNode("Tool/@idref")?.Value;
            if (string.IsNullOrEmpty(ndToolId))
                return;

            var ndTool = mainDom.SelectSingleNode($"MainMenu/Lib/Tools/Tool[@id='{ndToolId}']");
            if (ndTool == null)
                return;

            program.StartUIWithCommand(ndToolId, ndTool.OuterXml);
        }
    }

    public void ExecuteFancyCommand(string commandName, string[] commandArgs, XmlDocument mainDom)
    {
        Console.WriteLine("Executing a fancy command");
        if (mainDom == null)
            return;

        var ndCmd =
            mainDom.SelectSingleNode($"MainMenu/Commands/Command[@id='{commandName}']");
        if (ndCmd == null)
            return;

        var ndToolId = ndCmd.SelectSingleNode("Tool/@idref")?.Value;
        if (string.IsNullOrEmpty(ndToolId))
            return;

        var ndTool = mainDom.SelectSingleNode($"MainMenu/Lib/Tools/Tool[@id='{ndToolId}']");
        if (ndTool == null)
            return;

        var docCmdNd = ndCmd.OwnerDocument;
        var ndPrms = ndTool.SelectSingleNode("Parameters");
        if (ndPrms == null && docCmdNd != null)
        {
            ndPrms = docCmdNd.CreateNode(System.Xml.XmlNodeType.Element, "Parameters", null);
            ndTool.AppendChild(ndPrms);
        }

        var cmdLnPrms = ndPrms?.SelectSingleNode("Parameter[@name='CustomParameters']");
        if (cmdLnPrms == null && docCmdNd != null)
        {
            cmdLnPrms =
                docCmdNd.CreateNode(System.Xml.XmlNodeType.Element, "Parameter", null);
            var ndAttrName = docCmdNd.CreateAttribute("name");
            ndAttrName.Value = "CustomParameter";
            cmdLnPrms.Attributes.Append(ndAttrName);
            ndAttrName = docCmdNd.CreateAttribute("value");
            ndAttrName.Value = string.Join(", ", commandArgs);
            cmdLnPrms.Attributes.Append(ndAttrName);
            ndPrms.AppendChild(cmdLnPrms);
        }
        else
        {
            var ndAttrValue = (cmdLnPrms as System.Xml.XmlElement)?.GetAttributeNode("value");
            if (ndAttrValue != null)
                ndAttrValue.Value = string.Join(", ", commandArgs);
        }

        if (!String.IsNullOrEmpty(ndToolId) && !String.IsNullOrEmpty(ndTool.InnerXml))
            StartUIWithCommand(ndToolId, ndTool.OuterXml);
    }

    public void StartUIWithCommand(string toolId, string nodeTool)
    {
        throw new NotImplementedException("cannot be called from a unit test - this method will open a UI window.");
    }
}