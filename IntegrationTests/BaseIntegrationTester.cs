using StackScript0.Core;

namespace StackScript0.IntegrationTests
{
    public class BaseIntegrationTester
    {
        public BaseIntegrationTester() { }

        public virtual string GetName() => "";

        public void Run()
        {
            var source = GetSource();
            var interpreter = new Interpreter();
            interpreter.Eval(source);
        }

        protected virtual string GetSource() => "";
    }
}
