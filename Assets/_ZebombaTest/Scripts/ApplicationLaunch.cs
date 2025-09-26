using System;
using ZebombaTest.Scripts.Command;
using ZebombaTest.Scripts.UI;
using Zenject;

namespace ZebombaTest.Scripts
{
    public class ApplicationLaunch
    {
        private readonly IUIService _uiService;
        private Bootstrap.Bootstrap _bootstrap;

        public ApplicationLaunch(
            IInstantiator instantiator,
            IUIService uiService)
        {
            _uiService = uiService;
            _bootstrap = new Bootstrap.Bootstrap();
            
            _bootstrap.AddCommand(instantiator.Instantiate<SetupUIRootCommand>());
            
            _bootstrap.AllCommandsDone += AllCommandsDoneHandler;
            _bootstrap.StartExecute();
        }

        private void AllCommandsDoneHandler(object sender, EventArgs e)
        {
            _bootstrap.AllCommandsDone -= AllCommandsDoneHandler;
            _uiService.Show<UIMainMenuWindow>();
        }
    }
}

