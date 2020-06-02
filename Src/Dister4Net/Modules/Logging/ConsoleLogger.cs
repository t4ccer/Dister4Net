using System;
using System.Collections.Generic;
using System.Text;
using Dister4Net.Modules;
using Dister4Net.Modules.Communicators;

namespace Dister4Net.Modules.Logging
{
    public class ConsoleLogger : Logger
    {
        public override string ModuleName => "Console logger";

        public override void Log(string message) => Console.WriteLine($"Logger: {message}");
        protected override void Start()
        {
            foreach (var module in service.GetModules())
            {
                if (module is Communicator communicator)
                {
                    communicator.MessageArrived += (msg) => Log("Message arrived: " + serializer.Serialize(msg));
                    communicator.MessageSent += (target, msg) => Log($"Message sent to {target}: " + serializer.Serialize(msg));
                }
                module.ModuleStarting += (name) => Log($"Module {name} starting");
                module.ModuleStarted += (name) => Log($"Module {name} started");
            }
        }
    }
}
