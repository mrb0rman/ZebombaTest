using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZebombaTest.Scripts
{
    namespace Command
    {
        public class SetupCameraCommand : Command
        {
            private readonly IInstantiator _instantiator;

            public SetupCameraCommand(
                IInstantiator instantiator,
                CommandStorage commandStorage) : base(commandStorage)
            {
                _instantiator = instantiator;
            }
            public override CommandResult Execute()
            {
                _instantiator.InstantiatePrefabResource("Camera");
                
                Done?.Invoke(this, EventArgs.Empty);
            
                return base.Execute();
            }
        }
    }
}