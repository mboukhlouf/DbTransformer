using System;
using System.Collections.Generic;
using System.Text;
using DbTransformer.NinjectModules;
using DbTransformer.ViewModels;
using Ninject;

namespace DbTransformer
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {

        }

        public MainViewModel MainViewModel
        {
            get
            {
                return IocKernel.Get<MainViewModel>();
            }
        }
    }
}
