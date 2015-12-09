
namespace SystemStatus.Domain
{
    using System;
    using System.Collections.Generic;

    public interface ICommandProcessor
    {
        IEnumerable<TResult> Process<TCommand, TResult>(TCommand command) where TCommand : ICommand;
        void Process<TCommand, TResult>(TCommand command, Action<TResult> resultHandler) where TCommand : ICommand;
        void Process<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
