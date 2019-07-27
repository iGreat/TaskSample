using System;

namespace TaskSample
{
    public interface ITaskable
    {
        ITaskable Runable(Action action);

        ITaskable Then(Action action);

        void Catch(Action<Exception> action);
    }

    public interface ITaskable<T> : ITaskable
    {
        ITaskable<T> Runable(Action<T> action);

        ITaskable<TResult> Runable<TResult>(Func<T, TResult> func);

        ITaskable<T> Then(Action<T> action);

        ITaskable<TResult> Then<TResult>(Func<T, TResult> func);
    }
}