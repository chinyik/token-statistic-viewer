using System.Text;
using TokenScanner.Models;

namespace TokenScanner.Services.TokenPrice
{
    public class TokenPriceService : BackgroundService, ITokenPriceService
    {
        private const string CURRENCY = "USD";
        private const string ENDPOINT = "https://min-api.cryptocompare.com/data/price";

        private TokenContext _context;
        private int ServiceDelayInHours;
        private int ServiceDelayInMinutes;
        private int ServiceDelayInSeconds;

        public TokenPriceService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<TokenContext>();
            ServiceDelayInHours = configuration.GetValue<int>(nameof(ServiceDelayInHours));
            ServiceDelayInMinutes = configuration.GetValue<int>(nameof(ServiceDelayInMinutes));
            ServiceDelayInSeconds = configuration.GetValue<int>(nameof(ServiceDelayInSeconds));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                TimeSpan delay = new(ServiceDelayInHours, ServiceDelayInMinutes, ServiceDelayInSeconds);

                Console.WriteLine("Starting background price updating service");

                await Task.Delay(delay, stoppingToken);
                await UpdateAllPriceAsync();

                Console.WriteLine("Finished background price updating service");
            }
        }

        public async Task UpdateAllPriceAsync()
        {
            foreach (Token token in _context.Tokens)
            {
                decimal price = await GetPriceAsync(token.Symbol);

                token.Price = price;
                _context.Tokens.Attach(token);
                _context.Entry(token).Property(x => x.Price).IsModified = true;
            }

            await _context.SaveChangesAsync();
        }

        private async Task<decimal> GetPriceAsync(string symbol)
        {
            string requestUrl = ConstructUrl(symbol, CURRENCY);

            using HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync(requestUrl, CancellationToken.None);

            if (!response.IsSuccessStatusCode)
            {
                return 0;
            }

            dynamic content;
            try
            {
                content = await response.Content.ReadFromJsonAsync<Dictionary<string, decimal>>();

                return content.TryGetValue(CURRENCY, out decimal result)
                ? result
                : 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        private string ConstructUrl(string symbol, string currency)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append(ENDPOINT);
            stringBuilder.Append("?fsym=");
            stringBuilder.Append(symbol);
            stringBuilder.Append("&tsyms=");
            stringBuilder.Append(currency);

            return stringBuilder.ToString();
        }
    }
}
