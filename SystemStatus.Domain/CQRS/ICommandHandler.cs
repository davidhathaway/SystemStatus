namespace SystemStatus.Domain
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        CommandResult<TCommand> Handle(TCommand command);
    }

    //public interface ICommandHandler<in TCommand, out TResult> where TCommand : ICommand
    //{
    //    TResult Handle(TCommand command);
    //}
}
