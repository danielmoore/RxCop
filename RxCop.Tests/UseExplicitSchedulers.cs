using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace NorthHorizon.RxCop.Tests
{
    public class UseExplicitSchedulers
    {
        public void Problem()
        {
            Observable
                .Range(0, 10)
                .TakeUntil(Observable.Timer(TimeSpan.FromMilliseconds(50)));
        }

        public void Ok()
        {
            Observable
               .Range(0, 10)
               .TakeUntil(Observable.Timer(TimeSpan.FromMilliseconds(50), Scheduler.ThreadPool));
        }
    }
}
