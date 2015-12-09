namespace SystemStatus.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public abstract class CommandHandlerBase<TCommand> : ICommandHandler<TCommand>
        where TCommand:ICommand
    {
        protected abstract CommandResult<TCommand> Validate(TCommand command);

        public virtual CommandResult<TCommand> Handle(TCommand command)
        {
            var result = Validate(command);
            if(result.Success)
            {
                try
                {
                    OnHandle(command, result);
                }
                catch (System.Exception ex)
                {
                    result.AddError("Exception", ex.ToString());
                }
            }
            return result;
        }

        protected abstract void OnHandle(TCommand command, CommandResult<TCommand> result);
    }
}
