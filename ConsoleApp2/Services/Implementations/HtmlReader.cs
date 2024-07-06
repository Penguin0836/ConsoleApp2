using HtmlAgilityPack;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations;

public class HtmlReader(HtmlDocument htmlDocument) : IDocumentReader
{
    public string GetDocumentInfo()
    {
        return htmlDocument.DocumentNode.SelectSingleNode("/html/body").InnerText;
    }

    public void LoadDocument(string path)
    {
        htmlDocument.Load(path);
    }
}