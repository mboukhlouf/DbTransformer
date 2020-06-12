using DbTransformer.Services;
using DbTransformer.ViewModels;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbTransformer.NinjectModules
{
    public class IocBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<MainViewModel>().ToSelf().InTransientScope();
            Bind<IFileDialog>().To<FileDialog>().InTransientScope();
            Bind<IMessageBox>().To<MessageBox>().InSingletonScope();
        }
    }
}
