using StackScript0.Core;

namespace StackScript0.IntegrationTests
{
    public class ConsoleWhileLoopTester : BaseIntegrationTester
    {
        public ConsoleWhileLoopTester() { }

        public override string GetName() => "Console While Loop";

        protected override string GetSource()
        {
            var random = new Random();
            var r = random.Next() % 100;

            var lines = new List<string>
            {
                // define target
                $"{r} {Grammer.WordBegin} target {Grammer.WordEnd}",

                // initial condition
                $"{r + 1} {Grammer.WordBegin} guess {Grammer.WordEnd}",

                // while loop test
                $"{Grammer.ExpBegin} guess target {Grammer.Equal} {Grammer.Not} {Grammer.ExpEnd}",

                // while loop body
                $"{Grammer.ExpBegin}",
                $"`Type the number {r}, then enter.` {Grammer.ConsoleOut} {Grammer.Pop} {Grammer.ConsoleIn}",
                $"{Grammer.CastInt} {Grammer.WordBegin} guess {Grammer.WordEnd}",
                $"{Grammer.ExpEnd}",

                // while loop head
                $"{Grammer.ExpWhile}",
            };

            return string.Join(Environment.NewLine, lines);
        }
    }
}
