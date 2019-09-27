using MortalEngines.Common;
using MortalEngines.Entities.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MortalEngines.Entities
{
    public abstract class BaseMachine : IMachine
    {

        private string name;
        private IPilot pilot;
        private double healthPoints;
        private double attackPoints;
        private double defensePoints;

        public BaseMachine(string name, double attackPoints, double defensePoints, double healthPoints)
        {
            this.Name = name;
            this.AttackPoints = attackPoints;
            this.DefensePoints = defensePoints;
            this.HealthPoints = healthPoints;
            this.Targets = new List<string>();
        }

        public string Name
        {
            get => this.name;

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(OutputMessages.MachineNameCannotBeNullOrEmpty);
                }

                this.name = value;
            }
        }

        public IPilot Pilot
        {
            get => this.pilot;

            set
            {
                this.pilot = value ?? throw new NullReferenceException(string.Format(format: OutputMessages.ValueCannotBeNull, arg0: "Pilot")); //
            }
        }

        public double HealthPoints
        {
            get => this.healthPoints;

            set
            {
                //if (value < 0)
                //{
                //    throw new ArgumentOutOfRangeException(string.Format(format: OutputMessages.ValueCannotBeNegative, arg0: nameof(healthPoints)));
                //}

                this.healthPoints = value;
            }
        }

        public double AttackPoints
        {
            get => this.attackPoints;

            set
            {
                //if (value < 0)
                //{
                //    throw new ArgumentOutOfRangeException(string.Format(format: OutputMessages.ValueCannotBeNegative, arg0: nameof(attackPoints)));
                //}

                this.attackPoints = value;
            }
        }

        public double DefensePoints
        {
            get => this.defensePoints;

            set
            {
                //if (value < 0)
                //{
                //    throw new ArgumentOutOfRangeException(string.Format(format: OutputMessages.ValueCannotBeNegative, arg0: nameof(defensePoints)));
                //}

                this.defensePoints = value;
            }
        }

        public IList<string> Targets { get; protected set; }

        public void Attack(IMachine target)
        {
            if (target == null)
            {
                throw new NullReferenceException(string.Format(format: OutputMessages.ValueCannotBeNull, arg0: "Target"));
            }

            if (this.AttackPoints > target.DefensePoints)
            {
                var damagePoints = this.AttackPoints - target.DefensePoints;
                target.HealthPoints -= damagePoints;
                if (target.HealthPoints < 0)
                {
                    target.HealthPoints = 0;
                }
            }

            this.Targets.Add(target.Name);
        }

        public override string ToString()
        {
            var output = new StringBuilder();
            output.AppendLine($"- {this.Name}");
            output.AppendLine(" *Type: {0}");
            output.AppendLine($" *Health: {this.HealthPoints}");
            output.AppendLine($" *Attack: {this.AttackPoints}");
            output.AppendLine($" *Defense: {this.DefensePoints}");
            output.Append($" *Targets: {(this.Targets.Any() ? string.Join(separator: ',', values: this.Targets) : "None" )}");

            return output.ToString();
        }
    }
}
