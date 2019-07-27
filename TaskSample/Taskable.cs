using System;

namespace TaskSample
{
    public class Taskable : ITaskable
    {
        private Exception _exception;

        public ITaskable Runable(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                _exception = e;
            }

            return this;
        }

        public ITaskable Then(Action action)
        {
            try
            {
                if (_exception == null)
                {
                    action();
                }
            }
            catch (Exception e)
            {
                _exception = e;
            }

            return this;
        }

        public void Catch(Action<Exception> action)
        {
            if (_exception != null)
            {
                action(_exception);
            }
        }

        public static ITaskable Run(Action action)
        {
            return new Taskable().Runable(action);
        }
    }

    public class Taskable<T> : Taskable, ITaskable<T>
    {
        public ITaskable<T> Runable(Action<T> action)
        {
            throw new NotImplementedException();
        }

        public ITaskable<TResult> Runable<TResult>(Func<T, TResult> func)
        {
            throw new NotImplementedException();
        }

        public ITaskable<T> Then(Action<T> action)
        {
            throw new NotImplementedException();
        }

        public ITaskable<TResult> Then<TResult>(Func<T, TResult> func)
        {
            throw new NotImplementedException();
        }
    }
}