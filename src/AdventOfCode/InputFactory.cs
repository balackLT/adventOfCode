using System;
using System.IO;
using System.Net;

namespace AdventOfCode.Executor
{
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

        public Input GetInputFromFile(int day, string file)
        {
            var path = ConstructPath(day, file);

            var lines = File.ReadAllLines(path);

            return new Input(lines);
        }

        public Input GetDefaultInput(int day)
        {
            if (File.Exists(ConstructDefaultPath(day)))
            {
                return GetInputFromFile(day, DefaultFileName);
            }
            
            DownloadFile(day, _cookie, $"{_folder}/day{day}_{DefaultFileName}.txt");

            return GetInputFromFile(day, DefaultFileName);
        }

        private string ConstructDefaultPath(int day)
        {
            return ConstructPath(day, DefaultFileName);
        }

        private string ConstructPath(int day, string file)
        {
            var path = $@"./{_folder}/day{day}_{file}.txt";

            return path;
        }

        private string GetUrl(int day)
        {
            var url = $"https://adventofcode.com/{_year}/day/{day}/input";

            return url;
        }

        private void DownloadFile(int day, string cookie, string targetFile)
        {
            using var client = new WebClient();
            client.Headers.Add(HttpRequestHeader.Cookie, $"session={cookie}");

            var url = GetUrl(day);

            var input = client.DownloadString(url).Trim();
            File.WriteAllText(targetFile, input);
        }
    }
}
