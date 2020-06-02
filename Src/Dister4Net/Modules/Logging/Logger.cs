using Dister4Net.Modules;

namespace Dister4Net.Modules.Logging
{
    public abstract class Logger : Module
    {
        public override int StartPriority => 2;
        public abstract void Log(string message);
    }
}
