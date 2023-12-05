using AdventOfCode.Executor;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true, true)
    .Build();

var collector = new SolutionCollector();
var inputGenerator = new InputFactory(config["year"], config["inputFolder"], config["cookie"]);

const int day = 5;

var executor = collector.GetSolutionExecutor(day);

var input = await inputGenerator.GetDefaultInputAsync(day);

// await executor.ExecuteBothPartsAsync(input);
executor.ExecuteFirstPart(input);
executor.ExecuteSecondPart(input);