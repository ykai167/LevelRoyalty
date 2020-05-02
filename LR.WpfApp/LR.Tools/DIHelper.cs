using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Tools
{
    public static class DIHelper
    {
        static Unity.IUnityContainer container;
        static DIHelper()
        {
            container = new Unity.UnityContainer();
        }
        public static void RegistTransient<T, TMap>()
        {
            container.RegisterType(typeof(T), typeof(TMap), null, new Unity.Lifetime.TransientLifetimeManager());
        }

        public static T GetInstance<T>() where T : class
        {
            return container.Resolve(typeof(T), null) as T;
        }

        public static object GetInstance(Type type)
        {
            return container.Resolve(type, null);
        }
    }
}
