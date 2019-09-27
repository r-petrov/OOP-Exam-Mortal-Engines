using MortalEngines.Common;
using MortalEngines.Core.Contracts;
using MortalEngines.Enums;
using MortalEngines.IO.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;

namespace MortalEngines.Core
{
    // ConcreteCommand
    public class Command : ICommand
    {
        private readonly IWriter writer;

        public Command(IWriter writer)
        {
            this.writer = writer;
        }

        public event EventHandler CanExecuteChanged;

        public string CommandLine { get; set; }

        public bool CanExecute(object parameter)
        {
            return parameter != null;
        }

        public void Execute(object parameter)
        {
            var machinesManager = (IMachinesManager)parameter;
            var inputCommandParts = this.CommandLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string inputCommandName = this.GetCommandName(inputCommandParts);
            List<object> commandParameters = this.GetCommandParameters(inputCommandParts);

            var type = typeof(IMachinesManager);
            var commandMethod = type.GetMethod(name: inputCommandName);
            var output = (string)commandMethod.Invoke(obj: machinesManager, parameters: commandParameters.ToArray());

            this.writer.Write(output);
        }

        private string GetCommandName(string[] inputCommandParts)
        {
            var inputCommandName = inputCommandParts[0];
            if (Enum.IsDefined(enumType: typeof(InputCommands), value: inputCommandName))
            {
                var methodName = $"Generate{inputCommandName}CommandName";
                var method = this.GetType().GetMethod(name: methodName, bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance);
                inputCommandName = (string)method.Invoke(this, new object[] { inputCommandName });
            }

            return inputCommandName;
        }

        private List<object> GetCommandParameters(string[] inputCommandParts)
        {
            var inputCommandParameters = inputCommandParts.Skip(1);
            var commandParameters = new List<object>();
            commandParameters.AddRange(inputCommandParameters);
            if (commandParameters.Count == Constants.ManufactureCommandsParametersCount)
            {
                for (int i = 1; i < commandParameters.Count; i++)
                {
                    if (commandParameters[i] is IConvertible)
                    {
                        commandParameters[i] = ((IConvertible)commandParameters[i]).ToDouble(null);
                    }
                }
            }

            return commandParameters;
        }

        private string GenerateAggressiveModeCommandName(string inputCommandName)
        {
            var commandName = $"ToggleFighter{inputCommandName}";

            return commandName;
        }

        private string GenerateDefenseModeCommandName(string inputCommandName)
        {
            var commandName = $"ToggleTank{inputCommandName}";

            return commandName;
        }

        private string GenerateEngageCommandName(string inputCommandName)
        {
            var commandName = $"{inputCommandName}Machine";

            return commandName;
        }

        private string GenerateAttackCommandName(string inputCommandName)
        {
            var commandName = $"{inputCommandName}Machines";

            return commandName;
        }
    }
}
