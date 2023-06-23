using StackScript0.Core;
using StackScript0.IntegrationTests;
using StackScript0.Tests;

namespace StackScript0;

public static class Program
{
    public static void Main(string[] args)
    {
        RunArguments(args);
    }

    #region Helpers

    private static void RunArguments(string[] args)
    {
        if (args.Length < 1)
        {
            PrintUsage();
        }
        else if (args[0] == "-syntax")
        {
            foreach (var pair in Grammer.Descriptions)
            {
                Console.WriteLine($"{pair.Key} : {pair.Value}");
            }
        }
        else if (args[0] == "-examples")
        {
            Examples.Examples.ViewAll();
        }
        else if (args[0] == "-repl")
        {
            var interpreter = new Interpreter();
            interpreter.REPL();
        }
        else if (args[0] == "-i")
        {
            var targs = args.ToList().Skip(1).ToList();
            HandleIntegrationTestArguments(targs);
        }
        else if (args[0] == "-t")
        {
            var targs = args.ToList().Skip(1).ToList();
            HandleTestArguments(targs, true);
        }
        else if (args[0] == "-ts")
        {
            var targs = args.ToList().Skip(1).ToList();
            HandleTestArguments(targs, false);
        }
        else
        {
            PrintUsage();
        }
    }

    private static void HandleTestArguments(List<string> targs, bool verbose)
    {
        if (targs.Count < 1)
        {
            RunAllTesters(verbose);
        }
        else
        {
            var i = 0;

            while (i < targs.Count)
            {
                if (!targs[i].StartsWith("-"))
                    break;

                if (i + 1 < targs.Count && !targs[i + 1].StartsWith("-") && int.TryParse(targs[i + 1], out var index))
                {
                    RunTester(targs[i], index, verbose);
                    i += 2;
                }
                else
                {
                    RunTester(targs[i], -1, verbose);
                    i += 1;
                }
            }
        }
    }

    private static void HandleIntegrationTestArguments(List<string> targs)
    {
        if (targs.Count < 1) return;

        var first = targs.First();

        if (MapOfIntegrationTests.ContainsKey(first))
        {
            var tester = MapOfIntegrationTests[first]();

            tester.Run();
        }
    }

    private static void RunAllTesters(bool verbose)
    {
        MapOfTests.Values
            .ToList()
            .ForEach(f =>
            {
                var tester = f(verbose);
                tester.RunAll();
            });
    }

    private static void RunTester(string key, int index, bool verbose)
    {
        if (!MapOfTests.ContainsKey(key)) return;

        var tester = MapOfTests[key](verbose);

        if (index < 0)
            tester.RunAll();
        else
            tester.RunIndividual(index);
    }

    private static Dictionary<string, Func<bool, BaseTester>> MapOfTests =
        new Dictionary<string, Func<bool, BaseTester>>
        {
            { "-grammer", v => new GrammerTester(v) },
            { "-tokenizer", v => new TokenizerTester(v) },
            { "-varOp", v => new VarOpEvaluatorTester(v) },
            { "-wordInc", v => new WordIncStackEvaluatorTester(v) },
            { "-general", v => new GeneralEvaluatorTester(v) }
        };

    private static Dictionary<string, Func<BaseIntegrationTester>> MapOfIntegrationTests =
        new Dictionary<string, Func<BaseIntegrationTester>>
        {
            { "-conWhile", () => new ConsoleWhileLoopTester() },
            { "-guessTheNumber", () => new GuessTheNumberTester() }
        };

    private static void PrintUsage()
    {
        var lines = new List<string>
        {
            "[basic usage]",
            "language syntax: <program> -syntax",
            "example scripts: <program> -examples",
            "read, evaluate, print, loop: <program> -repl",
            "",
            "[unit tests]",
            "unit tests: <program> -t",
            "unit test(s) without details: <program> -ts",
            "individual unit test: <program> -(t|ts) -<type> <number>"
        };

        foreach (var pair in MapOfTests)
        {
            var key = pair.Key;
            var name = pair.Value(true).GetName();
            lines.Add($"`{name}` unit tests: <program> -(t|ts) {key}");
        }

        lines.Add("");
        lines.Add("[integration tests]");

        foreach (var pair in MapOfIntegrationTests)
        {
            var key = pair.Key;
            var name = pair.Value().GetName();
            lines.Add($"`{name}` integration test: <program> -i {key}");
        }

        var usage = string.Join(Environment.NewLine, lines);
        Console.WriteLine(usage);
    }

    #endregion Helpers
}
