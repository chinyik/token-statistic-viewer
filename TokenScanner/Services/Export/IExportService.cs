using TokenScanner.Models;

namespace TokenScanner.Services.Export
{
    public interface IExportService
    {
        byte[] GenerateFileContent(IEnumerable<Token> inputData, char delimiter = ',');
    }
}
