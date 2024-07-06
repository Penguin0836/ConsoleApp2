namespace TestTask.Services.Interfaces;

public interface IDocumentReader
{
    string GetDocumentInfo();
    void LoadDocument(string path);
}