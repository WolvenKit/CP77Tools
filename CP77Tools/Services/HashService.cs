using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CP77Tools.Services
{
    public class HashService
    {
        private readonly HttpClient _client = new HttpClient();
        
        private const string ResourceUrl = "https://nyxmods.com/cp77/files/archivehashes.csv";
        private readonly string _resourcePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/archivehashes.csv");
        private readonly string _eTagPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/archivehashes-etag.txt");

        public async Task Refresh()
        {
            var etag = GetCurrentEtag();
            
            var request = new HttpRequestMessage(HttpMethod.Get, ResourceUrl);
            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            var serverEtag = response.Headers.GetValues("etag").Single().Trim('"');
            
            if (!string.IsNullOrEmpty(etag) && !string.IsNullOrEmpty(serverEtag) && string.Equals(etag, serverEtag, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            
            Console.WriteLine("Downloading latest Archive Hashes...");
            
            var stream = await response.Content.ReadAsStreamAsync();

            await WriteHashes(stream);
            await WriteEtag(serverEtag);
            
            Console.WriteLine("Archive Hashes updated.");
        }

        private async Task WriteHashes(Stream source)
        {
            await using var hashesFs = File.Create(_resourcePath);
            await source.CopyToAsync(hashesFs);
        }

        private async Task WriteEtag(string etag)
        {
            await using var etagFs = File.Create(_eTagPath);
            await using var etagWriter = new StreamWriter(etagFs);
            await etagWriter.WriteLineAsync(etag);
        }

        private string GetCurrentEtag()
        {
            if (!File.Exists(_eTagPath)) return null;
            
            var lines = File.ReadLines(_eTagPath)
                .ToList();

            if (!lines.Any() || lines.Count > 1)
                return null;

            return lines.Single();
        }
    }
}