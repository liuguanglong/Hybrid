using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Hybrid.Server.WorkflowSteps
{
    public class HelloWorldStep : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Hello World!");
            return ExecutionResult.Next();
        }
    }
}
