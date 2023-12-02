using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdventOfCode.Executor;

public class InputFactory
{
    private const string DefaultFileName = "default";
    private readonly string _year;
    private readonly string _folder;
    private readonly string _cookie;

    public InputFactory(string year, string folder, string cookie)
    {
        _year = year;
        _folder = folder;
        _cookie = cookie;
    }

    private async Task<Input> GetInputFromFile(int day, string file)
    {
        var path = ConstructPath(day, file);

        var lines = await File.ReadAllLinesAsync(path);

        return new Input(lines);
    }

    public async Task<Input> GetDefaultInputAsync(int day)
    {
        if (File.Exists(ConstructDefaultPath(day)))
        {
            return await GetInputFromFile(day, DefaultFileName);
        }
            
        await DownloadFileAsync(day, _cookie, $"{_folder}/day{day}_{DefaultFileName}.txt");

        return await GetInputFromFile(day, DefaultFileName);
    }

    private string ConstructDefaultPath(int day)
    {
        return ConstructPath(day, DefaultFileName);
    }

    private string ConstructPath(int day, string file)
    {
        var path = $"./{_folder}/day{day}_{file}.txt";

        return path;
    }

    private string GetUrl(int day)
    {
        var url = $"https://adventofcode.com/{_year}/day/{day}/input";

        return url;
    }

    private async Task DownloadFileAsync(int day, string cookie, string targetFile)
    {
        using var client = new HttpClient();
        
        var request = new HttpRequestMessage {
            RequestUri = new Uri(GetUrl(day)),
            Method = HttpMethod.Get
        };
        request.Headers.Add("Cookie", $"session={cookie}");

        var result = await client.SendAsync(request);
        result.EnsureSuccessStatusCode();
        var input = await result.Content.ReadAsStringAsync();
        await File.WriteAllTextAsync(targetFile, input.Trim());
    }
}