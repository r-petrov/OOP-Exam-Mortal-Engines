using MortalEngines.Core;
using MortalEngines.Core.Contracts;
using MortalEngines.IO;
using MortalEngines.IO.Contracts;

namespace MortalEngines
{
    // Client  
    public class StartUp
    {
        public static void Main()
        {
            IWriter writer = new ConsoleWriter();
            IReader reader = new ConsoleReader(writer);

            // Receiver
            IMachinesManager machinesManager = new MachinesManager();

            // Invoker 
            IEngine engine = new Engine(reader: reader, machinesManager: machinesManager);

            engine.Run();
        }
    }
}