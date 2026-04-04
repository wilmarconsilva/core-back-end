using Application.Interfaces.Services;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Infrastructure.Pdf
{
    public class PdfService : IPdfService
    {
        private static readonly SemaphoreSlim _initLock = new(1, 1);
        private static IBrowser? _browser;

        public async Task<byte[]> GenerateFromHtmlAsync(string html, CancellationToken cancellationToken)
        {
            IBrowser browser = await GetOrCreateBrowserAsync();

            await using IPage page = await browser.NewPageAsync();
            await page.SetContentAsync(html, new NavigationOptions { WaitUntil = [WaitUntilNavigation.DOMContentLoaded] });

            return await page.PdfDataAsync(new PdfOptions { Format = PaperFormat.A4, PrintBackground = true });
        }

        private static async Task<IBrowser> GetOrCreateBrowserAsync()
        {
            if (_browser is { IsClosed: false })
                return _browser;

            await _initLock.WaitAsync();
            try
            {
                if (_browser is { IsClosed: false })
                    return _browser;

                BrowserFetcher browserFetcher = new BrowserFetcher();
                if (!browserFetcher.GetInstalledBrowsers().Any())
                    await browserFetcher.DownloadAsync();

                _browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
                return _browser;
            }
            finally
            {
                _initLock.Release();
            }
        }
    }
}
