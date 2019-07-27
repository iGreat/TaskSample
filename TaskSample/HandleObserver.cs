using System;

namespace TaskSample
{
    public class HandleObserver<T> : IObserver<T>
    {
        private IDisposable _unsubscriber;

        private Action<T> _action;
        private Action _completeAction;
        private Action<Exception> _errorAction;

        public HandleObserver(Action<T> action, Action completeAction = null, Action<Exception> errorAction = null)
        {
            _action = action;
            _completeAction = completeAction;
            _errorAction = errorAction;
        }

        public void OnCompleted()
        {
            _completeAction?.Invoke();
        }

        public void OnError(Exception error)
        {
            _errorAction?.Invoke(error);
        }

        public void OnNext(T value)
        {
            _action(value);
        }

        public void Subscribe(IObservable<T> provider)
        {
            _unsubscriber = provider.Subscribe(this);
        }

        public void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }
    }
}