using MortalEngines.Common;
using MortalEngines.Entities.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MortalEngines.Entities
{
    public class Fighter : BaseMachine, IFighter
    {
        private const double initialHealthPoints = 200;

        public Fighter(string name, double attackPoints, double defensePoints) 
            : base(name: name, attackPoints: attackPoints, defensePoints: defensePoints, healthPoints: initialHealthPoints)
        {
            this.AggressiveMode = false;
            this.ToggleAggressiveMode();
        }

        public bool AggressiveMode { get; private set; }

        public void ToggleAggressiveMode()
        {
            if (this.AggressiveMode == true)
            {
                this.AttackPoints -= Constants.FighterAttackPoints;
                this.DefensePoints += Constants.FighterDefensePoints;
                this.AggressiveMode = false;
            }
            else
            {
                this.AttackPoints += Constants.FighterAttackPoints;
                this.DefensePoints -= Constants.FighterDefensePoints;
                this.AggressiveMode = true;
            }
        }

        public override string ToString()
        {
            var output = new StringBuilder();
            output.AppendLine(string.Format(format: base.ToString(), arg0: this.GetType().Name));
            output.Append($" *Aggressive: {(this.AggressiveMode == true ? Constants.On : Constants.Off)}");

            return output.ToString();
        }
    }
}
