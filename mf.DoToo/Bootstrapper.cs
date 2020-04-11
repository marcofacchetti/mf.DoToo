using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using mf.DoToo.ViewModels;
using Xamarin.Forms;
using mf.DoToo.Repositories;

namespace mf.DoToo
{
    public abstract class Bootstrapper
    {
        protected ContainerBuilder ContainerBuilder { get; private set; }
        public Bootstrapper()
        {
            Initialize();
            FinishInitialization();
        }

        protected virtual void Initialize()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            ContainerBuilder = new ContainerBuilder();
            foreach (var type in currentAssembly.DefinedTypes.Where(e=>e.IsSubclassOf(typeof(Page)) || e.IsSubclassOf(typeof(BaseViewModel))))
            {
                ContainerBuilder.RegisterType(type.AsType());
            }
            ContainerBuilder.RegisterType<TodoItemRepository>().SingleInstance();
        }

        private void FinishInitialization()
        {
            var container = ContainerBuilder.Build();
            Resolver.Inizialize(container);
        }
    }
}
