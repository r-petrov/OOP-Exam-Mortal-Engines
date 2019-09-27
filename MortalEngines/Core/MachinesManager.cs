namespace MortalEngines.Core
{
    using Contracts;
    using MortalEngines.Common;
    using MortalEngines.Entities;
    using MortalEngines.Entities.Contracts;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    //Receiver
    public class MachinesManager : IMachinesManager
    {
        private IDictionary<string, IPilot> pilots;
        private IDictionary<string, IMachine> machines;

        public MachinesManager()
        {
            this.pilots = new Dictionary<string, IPilot>();
            this.machines = new Dictionary<string, IMachine>();
        }

        public string HirePilot(string name)
        {
            if (this.pilots.ContainsKey(name))
            {
                return string.Format(format: OutputMessages.PilotExists, arg0: name);
            }

            IPilot pilot = new Pilot(name);
            this.pilots.Add(key: name, value: pilot);

            return string.Format(format: OutputMessages.PilotHired, arg0: name);
        }

        public string ManufactureTank(string name, double attackPoints, double defensePoints)
        {
            if (this.machines.ContainsKey(name))
            {
                return string.Format(format: OutputMessages.MachineExists, arg0: name);
            }

            var tank = new Tank(name: name, attackPoints: attackPoints, defensePoints: defensePoints);
            this.machines.Add(key: name, value: tank);

            return string.Format(format: OutputMessages.TankManufactured, arg0: name, arg1: tank.AttackPoints, arg2: tank.DefensePoints);
        }

        public string ManufactureFighter(string name, double attackPoints, double defensePoints)
        {
            if (this.machines.ContainsKey(name))
            {
                return string.Format(format: OutputMessages.MachineExists, arg0: name);
            }

            var fighter = new Fighter(name: name, attackPoints: attackPoints, defensePoints: defensePoints);
            this.machines.Add(key: name, value: fighter);
            var messageParameters = new object[] { fighter.Name, fighter.AttackPoints, fighter.DefensePoints, fighter.AggressiveMode == true ? Constants.On : Constants.Off };

            return string.Format(format: OutputMessages.FighterManufactured, args: messageParameters);
        }

        public string EngageMachine(string selectedPilotName, string selectedMachineName)
        {
            if (!this.pilots.ContainsKey(selectedPilotName))
            {
                return string.Format(format: OutputMessages.PilotNotFound, arg0: selectedPilotName);
            }

            if (!this.machines.ContainsKey(selectedMachineName))
            {
                return string.Format(format: OutputMessages.MachineNotFound, arg0: selectedMachineName);
            }

            var machine = this.machines[selectedMachineName];
            if (machine.Pilot != null)
            {
                return string.Format(format: OutputMessages.MachineHasPilotAlready, arg0: selectedMachineName);
            }

            var pilot = this.pilots[selectedPilotName];
            pilot.AddMachine(machine);
            machine.Pilot = pilot;

            return string.Format(format: OutputMessages.MachineEngaged, arg0: selectedPilotName, arg1: selectedMachineName);
        }

        public string AttackMachines(string attackingMachineName, string defendingMachineName)
        {
            if (!this.machines.ContainsKey(attackingMachineName))
            {
                return string.Format(format: OutputMessages.MachineNotFound, arg0: attackingMachineName);
            }

            if (!this.machines.ContainsKey(defendingMachineName))
            {
                return string.Format(format: OutputMessages.MachineNotFound, arg0: defendingMachineName);
            }

            var attackingMachine = this.machines[attackingMachineName];
            if (attackingMachine.HealthPoints < 1)
            {
                return string.Format(format: OutputMessages.DeadMachineCannotAttack, arg0: attackingMachineName);
            }

            var defendingMachine = this.machines[defendingMachineName];
            if (defendingMachine.HealthPoints < 1)
            {
                return string.Format(format: OutputMessages.DeadMachineCannotAttack, arg0: defendingMachineName);
            }

            attackingMachine.Attack(defendingMachine);

            return string.Format(format: OutputMessages.AttackSuccessful, arg0: defendingMachineName, arg1: attackingMachineName, arg2: defendingMachine.HealthPoints);
        }

        public string PilotReport(string pilotReporting)
        {
            if (!this.pilots.ContainsKey(pilotReporting))
            {
                return string.Format(format: OutputMessages.PilotNotFound, arg0: pilotReporting);
            }

            var pilot = this.pilots[pilotReporting];

            return pilot.Report();
        }

        public string MachineReport(string machineName)
        {
            if (!this.machines.ContainsKey(machineName))
            {
                return string.Format(format: OutputMessages.MachineNotFound, arg0: machineName);
            }

            var machine = this.machines[machineName];

            return machine.ToString();
        }

        public string ToggleFighterAggressiveMode(string fighterName)
        {
            if (!this.machines.ContainsKey(fighterName))
            {
                return string.Format(format: OutputMessages.MachineNotFound, arg0: fighterName);
            }

            this.ToggleMachineMode(machineName: fighterName, toggleMethodName: nameof(Fighter.ToggleAggressiveMode));

            return string.Format(format: OutputMessages.FighterOperationSuccessful, arg0: fighterName);
        }

        public string ToggleTankDefenseMode(string tankName)
        {
            if (!this.machines.ContainsKey(tankName))
            {
                return string.Format(format: OutputMessages.MachineNotFound, arg0: tankName);
            }

            this.ToggleMachineMode(machineName: tankName, toggleMethodName: nameof(Tank.ToggleDefenseMode));

            return string.Format(format: OutputMessages.TankOperationSuccessful, arg0: tankName);
        }

        private void ToggleMachineMode(string machineName, string toggleMethodName)
        {
            var machine = this.machines[machineName];
            var type = machine.GetType();
            var toggleMethodInfo = type.GetMethod(name: toggleMethodName, bindingAttr: BindingFlags.Public | BindingFlags.Instance);
            toggleMethodInfo.Invoke(obj: machine, parameters: null);
        }
    }
}