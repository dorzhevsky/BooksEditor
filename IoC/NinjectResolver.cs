using Ninject;
using Repositories.Implementation;
using Repositories.Interfaces;

namespace IoC
{
    public static class NinjectResolver
    {
        public static void AddBindings(IKernel ninjectKernel)
        {
            ninjectKernel.Bind<IBooksRepository>().To<BooksRepository>();
        }         
    }
}