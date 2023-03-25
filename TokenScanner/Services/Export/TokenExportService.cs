using System.Globalization;
using System.Text;
using TokenScanner.Models;

namespace TokenScanner.Services.Export
{
    public class TokenExportService : IExportService
    {
        public byte[] GenerateFileContent(IEnumerable<Token> inputData, char delimiter = ',')
        {
            try
            {
                decimal grandTotalSupply = inputData.Sum(x => x.TotalSupply);

                List<string[]> tokensToExport = (from data in inputData
                                                 select new string[] {
                                                 data.Id.ToString(),
                                                 data.Symbol,
                                                 data.Name,
                                                 data.ContractAddress,
                                                 data.TotalHolders.ToString(),
                                                 data.TotalSupply.ToString(),
                                                 (data.TotalSupply / grandTotalSupply).ToString("P", CultureInfo.InvariantCulture)
                                             }).ToList<string[]>();

                string[] header = new string[] {
                    nameof(Token.Id),
                    nameof(Token.Symbol),
                    nameof(Token.Name),
                    nameof(Token.ContractAddress),
                    nameof(Token.TotalHolders),
                    nameof(Token.TotalSupply),
                    $"{nameof(Token.TotalSupply)}%" };

                tokensToExport.Insert(0, header);

                StringBuilder sb = new();
                for (int i = 0; i < tokensToExport.Count; i++)
                {
                    string[] token = tokensToExport[i];
                    for (int j = 0; j < token.Length; j++)
                    {
                        sb.Append(token[j] + delimiter);
                    }

                    sb.Append(Environment.NewLine);

                }

                return Encoding.UTF8.GetBytes(sb.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Array.Empty<byte>();
            }
        }
    }
}
