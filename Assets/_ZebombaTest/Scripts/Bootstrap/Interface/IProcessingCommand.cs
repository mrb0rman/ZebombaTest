using System;
using ZebombaTest.Scripts.Command;

namespace ZebombaTest.Scripts
{
    namespace Bootstrap
    {
        public interface IProcessingCommand
        {
            event EventHandler AllCommandsDone;

            bool IsExecuting { get; }

            void AddCommand(ICommand cmd);
            void StartExecute();
            void Clear();

            bool Any();
        }
    }
}
