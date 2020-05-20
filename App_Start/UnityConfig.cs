using SoftSolution.Database;
using SoftSolution.Menaxheret;
using SoftSolution.Models;
using SoftSolution.Models;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace SoftSolution
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

			// register all your components with the container here
			// it is NOT necessary to register your controllers

			// e.g. container.RegisterType<ITestService, TestService>();

			container.RegisterType<KerkesStore>();
			container.RegisterType<KerkesMenaxher>();

			container.RegisterType<SSDBContext>();

			GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
		}
    }
}