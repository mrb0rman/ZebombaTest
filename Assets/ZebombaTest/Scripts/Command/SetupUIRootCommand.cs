using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZebombaTest.Scripts.UI;

namespace ZebombaTest.Scripts
{
    namespace Command
    {
        public class SetupUIRootCommand : Command
        {
            private readonly IUIRoot _uiRoot;
            
            public SetupUIRootCommand(
                IUIRoot uiRoot,
                CommandStorage commandStorage) : base(commandStorage)
            {
                _uiRoot = uiRoot;
            }
            
            public override CommandResult Execute()
            {
                Done?.Invoke(this, EventArgs.Empty);
            
                return base.Execute();
            }
        }
    }
}

