using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Xunit;
using Xunit.Sdk;

namespace TestFancyProgram;

public class FancyProgramTest
{
    [Fact]
    public void ExecuteFancyCommand()
    {
        // sample test data and custom parameters you could use
        var xmlInput = GetTestData("Sample.xml");
        var customParams = new string[] { "//ID: 71mUJgN0sKbr", "//Customer: Acme" };

        var program = new FancyProgram();
        var mainDom = new XmlDocument();
        mainDom.LoadXml(xmlInput);

        program.ExecuteFancyCommand("App.fancy", customParams, mainDom);

        // TODO: assert something
    }

    private string GetTestData(string filename)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestData", filename);
        return File.ReadAllText(path);
    }
}