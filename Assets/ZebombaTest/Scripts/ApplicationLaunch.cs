using System;

namespace ZebombaTest.Scripts
{
    public class ApplicationLaunch
    {
        private Bootstrap.Bootstrap _bootstrap;

        public ApplicationLaunch()
        {
            _bootstrap = new Bootstrap.Bootstrap();
            
            _bootstrap.AllCommandsDone += AllCommandsDoneHandler;
            _bootstrap.StartExecute();
        }

        private void AllCommandsDoneHandler(object sender, EventArgs e)
        {
            _bootstrap.AllCommandsDone -= AllCommandsDoneHandler;
        }
    }
}

