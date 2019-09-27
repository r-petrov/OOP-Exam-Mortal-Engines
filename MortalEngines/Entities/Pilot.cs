using MortalEngines.Common;
using MortalEngines.Entities.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MortalEngines.Entities
{
    public class Pilot : IPilot
    {
        private string name;

        public Pilot(string name)
        {
            this.Name = name;
            this.Machines = new List<IMachine>();
        }

        public string Name
        {
            get => this.name;

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(OutputMessages.PilotNameCannotBeNullOrEmpty);
                }

                this.name = value;
            }
        }

        public IList<IMachine> Machines { get; set; }

        public void AddMachine(IMachine machine)
        {
            if (machine == null)
            {
                throw new NullReferenceException(OutputMessages.NullMachineCannotBeAdded);
            }

            this.Machines.Add(machine);
        }

        public string Report()
        {
            var output = new StringBuilder();
            output.AppendLine($"{this.Name} - {this.Machines.Count} machines");
            if (this.Machines.Any())
            {
                foreach (var machine in this.Machines)
                {
                    output.AppendLine($"- {machine.Name}");
                    output.AppendLine($" *Type: {machine.GetType().Name}");
                    output.AppendLine($" *Health: {machine.HealthPoints}");
                    output.AppendLine($" *Attack: {machine.AttackPoints}");
                    output.AppendLine($" *Defense: {machine.DefensePoints}");
                    output.Append($" *Targets:  {(machine.Targets.Any() ? string.Join(separator: ',', values: machine.Targets) : "None")}");
                }
            }

            return output.ToString();
        }
    }
}
