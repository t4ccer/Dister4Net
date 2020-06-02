using System;
using System.Collections.Generic;
using System.Text;
using Dister4Net.Serialization;

namespace Dister4Net.Modules
{
    public abstract class ModuleAdder : Module
    {
        public override int StartPriority => 1;
    }
    public abstract class Module
    {
        public virtual int StartPriority => 0;
        protected ISerializer serializer;
        protected DisterService service;
        public abstract string ModuleName { get;}

        public delegate void ModuleStartedHandler(string moduleName);
        public event ModuleStartedHandler ModuleStarted;
        public void OnModuleStarted(string moduleName) => ModuleStarted?.Invoke(moduleName);

        public delegate void ModuleStartingHandler(string moduleName);
        public event ModuleStartingHandler ModuleStarting;
        public void OnModuleStarting(string moduleName) => ModuleStarting?.Invoke(moduleName);

        public void StartModule()
        {
            OnModuleStarting(ModuleName);
            Start();
            OnModuleStarted(ModuleName);
        }
        public void Initialize(ISerializer serializer, DisterService service)
        {
            this.serializer = serializer;
            this.service = service;
        }
        protected abstract void Start();

    }
}
