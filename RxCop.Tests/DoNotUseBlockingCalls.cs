using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace NorthHorizon.RxCop.Tests
{
    public class DoNotUseBlockingCalls
    {
        public void Problem()
        {
            Observable.Range(0, 10).First();
        }

        public void Ok()
        {
            var subj = new BehaviorSubject<int>(5);

            subj.First();
        }
    }
}
