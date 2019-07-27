using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskSample
{
    public class EventObserverable<T> : IObservable<T>
    {
        private readonly List<IObserver<T>> _observers = new List<IObserver<T>>();

        private readonly T _observableData;

        public EventObserverable(T observableData)
        {
            _observableData = observableData;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            return new Unsubscriber(_observers, observer);
        }

        public void Run(Func<T, T> func)
        {
            foreach (var observer in _observers.ToList())
            {
                try
                {
                    observer.OnNext(func(_observableData));
                }
                catch (Exception e)
                {
                    observer.OnError(e);
                }
                finally
                {
                    observer.OnCompleted();
                }
            }
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<T>> _observers;
            private readonly IObserver<T> _observer;

            public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers != null && _observers.Contains(_observer))
                {
                    _observers.Remove(_observer);
                }
            }
        }
    }
}