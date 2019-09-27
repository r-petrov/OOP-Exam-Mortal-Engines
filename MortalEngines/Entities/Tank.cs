using MortalEngines.Common;
using MortalEngines.Entities.Contracts;
using System.Text;

namespace MortalEngines.Entities
{
    public class Tank : BaseMachine, ITank
    {
        private const double initialHealthPoints = 100;

        public Tank(string name, double attackPoints, double defensePoints) : base(name: name, attackPoints: attackPoints, defensePoints: defensePoints, healthPoints: initialHealthPoints)
        {
            this.DefenseMode = false;
            this.ToggleDefenseMode();
        }

        public bool DefenseMode { get; private set; }

        public void ToggleDefenseMode()
        {
            if (this.DefenseMode == true)
            {
                this.AttackPoints += Constants.TankAttackPoints;
                this.DefensePoints -= Constants.TankDefensePoints;
                this.DefenseMode = false;
            }
            else
            {
                this.AttackPoints -= Constants.TankAttackPoints;
                this.DefensePoints += Constants.TankDefensePoints;
                this.DefenseMode = true;
            }
        }

        public override string ToString()
        {
            var output = new StringBuilder();
            output.AppendLine(string.Format(format: base.ToString(), arg0: this.GetType().Name));
            output.Append($" *Defense: {(this.DefenseMode == true ? Constants.On : Constants.Off)}");

            return output.ToString();
        }
    }
}
