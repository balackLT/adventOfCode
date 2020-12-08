using AdventOfCode.Executor;
using Microsoft.Extensions.Configuration;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true, true)
    .Build();

var collector = new SolutionCollector();
var inputGenerator = new InputFactory(config["year"], config["inputFolder"], config["cookie"]);

var day = 8;

var executor = collector.GetSolutionExecutor(day);

// var input = inputGenerator.GetInputFromFile(day, "test1");
var input = inputGenerator.GetDefaultInput(day);

executor.ExecuteFirstPart(input);
executor.ExecuteSecondPart(input);
