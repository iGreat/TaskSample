using System;
using System.Threading;
using Timer = System.Timers.Timer;

namespace TaskSample
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //workflow task test

            Taskable.Run(() => { Console.WriteLine($"first task"); })
                .Then(() =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("second task");
                    throw new Exception("oops");
                })
                .Then(() => { Console.WriteLine("third task"); })
                .Catch(e => { Console.WriteLine(e.Message); });

            //observerable test
            var timerProvider = new EventObserverable<DateTime>(DateTime.Now);

            var reporter1 = new HandleObserver<DateTime>(t =>
            {
                Console.WriteLine($"report1: current time:{t:HH:mm:ss}");
            });
            var reporter2 = new HandleObserver<DateTime>(t =>
            {
                Console.WriteLine($"report2: current time:{t:HH:mm:ss}");
            });

            reporter1.Subscribe(timerProvider);
            reporter2.Subscribe(timerProvider);

            var maxReportCount = 7;

            var timer = new Timer(1000);
            timer.Elapsed += (s, e) =>
            {
                if (--maxReportCount == 0)
                    timer.Dispose();
                if (maxReportCount == 3)
                    reporter2.Unsubscribe();

                timerProvider.Run(t => DateTime.Now);
            };

            timer.Start();

            Console.WriteLine("PRESS ENTER TO QUIT.");
            Console.ReadKey();
        }
    }
}