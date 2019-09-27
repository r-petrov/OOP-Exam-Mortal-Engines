using MortalEngines.IO.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MortalEngines.Core.Contracts
{
    // Invoker 
    public class Engine : IEngine
    {
        private readonly IReader reader;
        private readonly IMachinesManager machinesManager;

        public Engine(IReader reader, IMachinesManager machinesManager)
        {
            this.reader = reader;
            this.machinesManager = machinesManager;
        }

        public void Run()
        {
            var commands = this.reader.ReadCommands();
            foreach (var command in commands)
            {
                command.Execute(this.machinesManager);
            }
        }
    }
}
