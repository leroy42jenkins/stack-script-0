using StackScript0.Core;

namespace StackScript0.IntegrationTests
{
    public class GuessTheNumberTester : BaseIntegrationTester
    {
        public GuessTheNumberTester() { }

        public override string GetName() => "Guess The Number";

        protected override string GetSource()
        {
            var random = new Random();
            var r = random.Next() % 100;

            var lines = new List<string>
            {
                // define target
                $"{Grammer.WordBegin} target {r} {Grammer.WordEnd}",

                // initial condition
                $"{Grammer.WordBegin} i 1 {Grammer.WordEnd}",

                // test word
                $"{Grammer.WordBegin} test",
                $"{Grammer.ExpBegin} {Grammer.Dup} target {Grammer.Equal} {Grammer.ExpEnd}",
                $"{Grammer.ExpBegin} `Correct!` {Grammer.ConsoleOut} {Grammer.Pop} " + 
                $"{Grammer.WordBegin} i 0 {Grammer.WordEnd} {Grammer.ExpEnd}",
                $"{Grammer.ExpBegin}",
                //
                $"{Grammer.ExpBegin} {Grammer.Dup} target {Grammer.Lower} {Grammer.ExpEnd}",
                $"{Grammer.ExpBegin} `Higher...` {Grammer.ConsoleOut} {Grammer.Pop} {Grammer.ExpEnd}",
                $"{Grammer.ExpBegin} `Lower...` {Grammer.ConsoleOut} {Grammer.Pop} {Grammer.ExpEnd}",
                $"{Grammer.ExpBranch}",
                //
                $"{Grammer.ExpEnd} {Grammer.ExpBranch}",
                //
                $"{Grammer.WordEnd}",

                // while loop test
                $"{Grammer.ExpBegin} i {Grammer.ExpEnd}",

                // while loop body
                $"{Grammer.ExpBegin}",
                $"`Type a number a number between 0 and 100, then enter.` {Grammer.ConsoleOut} {Grammer.Pop}",
                $"{Grammer.ConsoleIn} {Grammer.CastInt} test {Grammer.Pop}",
                $"{Grammer.ExpEnd}",

                // while loop head
                $"{Grammer.ExpWhile}",
            };

            return string.Join(Environment.NewLine, lines);
        }
    }
}
