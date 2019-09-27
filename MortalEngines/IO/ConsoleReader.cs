using MortalEngines.Common;
using MortalEngines.Core;
using MortalEngines.IO.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MortalEngines.IO
{
    public class ConsoleReader : IReader
    {
        private readonly IWriter writer;
        private IList<ICommand> commands;

        public ConsoleReader(IWriter writer)
        {
            this.commands = new List<ICommand>();
            this.writer = writer;
        }
        
        public IList<ICommand> ReadCommands()
        {
            var commandLine = Console.ReadLine();
            while (commandLine != Constants.QuitCommand)
            {
                ICommand command = new Command(this.writer) { CommandLine = commandLine };
                this.commands.Add(command);

                commandLine = Console.ReadLine();
            }

            return this.commands;
        }
    }
}
