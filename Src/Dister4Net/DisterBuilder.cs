using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dister4Net.Attributes;
using Dister4Net.Helpers;
using Dister4Net.Modules;
using Dister4Net.Serialization;

namespace Dister4Net
{
    public class DisterBuilder
    {
        private readonly List<Timer> timers = new List<Timer>();
        private readonly DisterService service;
        public DisterBuilder(DisterService service)
            => this.service = service;

        public static DisterBuilder Build(DisterService service)
        {
            return new DisterBuilder(service);
        }
        public DisterBuilder Serializer(ISerializer serializer)
        {
            service.Serializer = serializer;
            return this;
        }
        public DisterBuilder Module(Modules.Module module)
        {
            service.AddModule(module);
            return this;
        }

        public void Start()
        {
            CheckRequiredModules();
            StartModules();

            ExecuteEntryPoint();
            ExecuteOnce();
            ExecuteEndless();
            ExecuteParallel();
            ExecuteParallelLoop();
        }

        private void ExecuteParallelLoop()
        {
            service.GetType()
                .GetMethods()
                .WithAttribute(typeof(ParallelLoopAttribute))
                .Select(x => (method: x, x.GetCustomAttributes(typeof(ParallelLoopAttribute), true).First().ConvertTo<ParallelLoopAttribute>().count))
                .ForEach(x => Enumerable.Repeat<Action>(() => x.method.Invoke(service, null), x.count).ForEach(y => new Thread(z => { while (true) y(); }).Start()));
        }
        private void ExecuteParallel()
        {
            service.GetType()
                .GetMethods()
                .WithAttribute(typeof(ParallelAttribute))
                .Select(x => (method: x, x.GetCustomAttributes(typeof(ParallelAttribute), true).First().ConvertTo<ParallelAttribute>().count))
                .ForEach(x => Enumerable.Repeat<Action>(() => x.method.Invoke(service, null), x.count).ForEach(y => new Thread(z => y()).Start()));
        }

        private void ExecuteEndless()
        {
            service.GetType()
                .GetMethods()
                .WithAttribute(typeof(EndlessAttribute))
                .Select(x => (method: x, x.GetCustomAttributes(typeof(EndlessAttribute), true).First().ConvertTo<EndlessAttribute>().interval))
                .ForEach(x =>
                {
                    if (x.interval > 0)
                        timers.Add(new Timer(y => x.method.Invoke(service, null), null, 0, x.interval));
                    else
                        new Thread(() => { while (true) x.method.Invoke(service, null); });
                });
        }
        private void ExecuteOnce()
        {
            service.GetType()
                .GetMethods()
                .WithAttribute(typeof(OnceAttribute))
                .ForEach(x => new Thread(y => x.Invoke(service, null)).Start());
        }
        private void ExecuteEntryPoint()
        {
           service.GetType()
                .GetMethods()
                .WithAttribute(typeof(EntryPointAttribute))
                .FirstOrDefault()
                ?.Invoke(service, null);
        }

        private void CheckRequiredModules()
        {
            var allModukes = service.GetModules().Select(x => x.GetType());
            foreach (var module in service.GetModules())
            {
                var requiredModules = module.GetType()
                    .GetCustomAttributes(typeof(RequiresModuleAttribute), true)
                    .Select(x => (RequiresModuleAttribute)x)
                    .Select(x => x.type);

                foreach (var requiredModule in requiredModules)
                {
                    if (!allModukes.Any(m => m.IsSubclassOf(requiredModule) || m == requiredModule))
                        throw new System.Exception($"Module '{module.ModuleName}' ({module.GetType().FullName}) requires '{requiredModule}' module. Use Module() to add that module to DisterService.");
                }
            }
        }
        private void StartModules()
        {
            service.GetModules()
                .Select(x => x.StartPriority)
                .Distinct()
                .OrderByDescending(x => x)
                .ToImmutableList()
                .ForEach(p =>
                    service.GetModules()
                    .Where(x => x.StartPriority == p)
                    .ToImmutableList()
                    .ForEach(x => 
                    { 
                        x.Initialize(service.Serializer, service);  
                        x.StartModule(); 
                    }));
        }
    }
}
