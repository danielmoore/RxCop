using System;
using System.Reactive.Subjects;

namespace NorthHorizon.RxCop.Tests
{
    public class DoNotImplementFrameworkInterfaces
    {
        public class Problem1<T> : IObservable<T>
        {
            public IDisposable Subscribe(IObserver<T> observer)
            {
                throw new NotImplementedException();
            }
        }

        public class Problem2<T> : IObserver<T>
        {
            public void OnCompleted()
            {
                throw new NotImplementedException();
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnNext(T value)
            {
                throw new NotImplementedException();
            }
        }

        public class Problem3<T> : ISubject<T>
        {
            public void OnCompleted()
            {
                throw new NotImplementedException();
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnNext(T value)
            {
                throw new NotImplementedException();
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                throw new NotImplementedException();
            }
        }

        public class Problem4<TIn, TOut> : ISubject<TIn, TOut>
        {
            public void OnCompleted()
            {
                throw new NotImplementedException();
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnNext(TIn value)
            {
                throw new NotImplementedException();
            }

            public IDisposable Subscribe(IObserver<TOut> observer)
            {
                throw new NotImplementedException();
            }
        }


        public class OkSubject<T> : ISubject<T>
        {
            public void OnCompleted()
            {
                throw new NotImplementedException();
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnNext(T value)
            {
                throw new NotImplementedException();
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
