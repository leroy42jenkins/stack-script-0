namespace StackScript0.Tests
{
    public class BaseTester
    {
        protected string Name = "";
        protected bool Verbose = true;
        protected int Successes = 0;
        protected int Failures = 0;
        protected List<Action> TestList = null;

        public void RunAll()
        {
            Console.WriteLine("");

            var count = GetNumberOfTests();

            if (count == 0)
            {
                Console.WriteLine("No tests found.");
                return;
            }

            Failures = 0;
            Successes = 0;

            for (var i = 0; i < count; ++i)
                TestList[i]();

            if (Verbose) Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{Name} Test Summary: {Successes} successes and {Failures} failures");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void RunIndividual(int index)
        {
            if (index < 0 || index >= GetNumberOfTests()) return;

            TestList[index]();
        }

        public string GetName() => Name;

        public int GetNumberOfTests() =>
            TestList == null ? 0 : TestList.Count;

        protected void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        protected void PrintFailure(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
