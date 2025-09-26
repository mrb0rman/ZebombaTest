using System;

namespace ZebombaTest.Scripts
{
    namespace Command
    {
        public abstract class Command : ICommand, IDisposable
        {
            private CommandStorage _commandStorage;
            public EventHandler Done { get; set; }

            public CommandStorage Storage => _commandStorage;

            public Command(CommandStorage commandStorage)
            {
                _commandStorage = commandStorage;
                Storage.AddCommand(this);
                Done += OnDone;
            }

            public virtual CommandResult Undo()
            {
                return new CommandResult();
            }
        
            public virtual CommandResult Execute()
            {
                Storage.AddToHistory(this);
                return new CommandResult();
            }

            public virtual CommandResult Redo()
            {
                return new CommandResult();
            }

            protected virtual void OnDone(object sender, EventArgs e)
            {
                Storage.RemoveCommand(this);
            }

            public virtual void Cancel()
            {
            
            }

            public void Dispose()
            {
            
            }
        }
    }
}