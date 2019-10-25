using System;
using Unity;

namespace Architecture
{
    public class ComponentContainer
    {
        static readonly Lazy<ComponentContainer> implementation = new Lazy<ComponentContainer>(() => CreateContainer(), isThreadSafe: true);

        public static ComponentContainer Current
        {
            get
            {
                var ret = implementation.Value;

                if (ret == null)
                {
                    throw new NotImplementedException();
                }

                return ret;
            }
        }

        private static ComponentContainer CreateContainer()
        {
            return new ComponentContainer
            {
                unityContainer = new UnityContainer()
            };
        }

        public void Register<TInterface, TType>(bool singelton = false)
        {
            if (singelton)
            {
                unityContainer.RegisterSingleton(typeof(TInterface), typeof(TType));
            }
            else
            {
                unityContainer.RegisterType(typeof(TInterface), typeof(TType));
            }
        }

        public T Resolve<T>()
        {
            return unityContainer.Resolve<T>();
        }

        private IUnityContainer unityContainer;
    }
}
