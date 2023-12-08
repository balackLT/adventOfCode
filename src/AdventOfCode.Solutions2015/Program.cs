using AdventOfCode.Executor;
using Microsoft.Extensions.Configuration;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true, true)
    .Build();

var collector = new SolutionCollector();
var inputGenerator = new InputFactory(config["year"], config["inputFolder"], config["cookie"]);

var day = 17;

var executor = collector.GetSolutionExecutor(day);

var input = await inputGenerator.GetDefaultInputAsync(day);

executor.ExecuteFirstPart(input);
executor.ExecuteSecondPart(input);