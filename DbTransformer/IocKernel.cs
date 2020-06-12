using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbTransformer
{
    public static class IocKernel
    {
        private static StandardKernel kernel;

        public static T Get<T>()
        {
            return kernel.Get<T>();
        }

        public static void Initialize(params INinjectModule[] modules)
        {
            if (kernel == null)
            {
                kernel = new StandardKernel(modules);
            }
        }
    }
}
