using System;
using System.Activities;
using System.Threading;

namespace Wedding
{

    class Program
    {
        static void Main(string[] args)
        {
            AutoResetEvent syncEvent = new AutoResetEvent(false);

            WorkflowApplication i = new WorkflowApplication(new Workflow1());

            i.OnUnhandledException = (waueea) =>
            {
                Console.WriteLine("{0} - {1}", waueea.UnhandledException.GetType(), 
                    waueea.UnhandledException.Message);
                return UnhandledExceptionAction.Cancel;
            };

            i.Completed = (wacea) => { syncEvent.Set(); };

            i.Run();

            syncEvent.WaitOne();

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }
    }

    public class CallItOffException : Exception
    {
        public CallItOffException()
            : base()
        {
        }

        public CallItOffException(string message)
            : base(message)
        {
        }
    }
}
