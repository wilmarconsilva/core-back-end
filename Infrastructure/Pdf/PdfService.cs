using Application.Interfaces.Services;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Infrastructure.Pdf
{
    public class PdfService : IPdfService
    {
        private static readonly SemaphoreSlim _downloadLock = new(1, 1);
        private static bool _browserDownloaded;

        public async Task<byte[]> GenerateFromHtmlAsync(string html, CancellationToken cancellationToken)
        {
            await EnsureBrowserDownloadedAsync();

            await using IBrowser browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            await using IPage page = await browser.NewPageAsync();
            await page.SetContentAsync(html, new NavigationOptions { WaitUntil = [WaitUntilNavigation.Networkidle0] });

            return await page.PdfDataAsync(new PdfOptions { Format = PaperFormat.A4, PrintBackground = true });
        }

        private static async Task EnsureBrowserDownloadedAsync()
        {
            if (_browserDownloaded)
                return;

            await _downloadLock.WaitAsync();
            try
            {
                if (!_browserDownloaded)
                {
                    await new BrowserFetcher().DownloadAsync();
                    _browserDownloaded = true;
                }
            }
            finally
            {
                _downloadLock.Release();
            }
        }
    }
}
