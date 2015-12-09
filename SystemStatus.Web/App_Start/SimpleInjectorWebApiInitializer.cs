[assembly: WebActivator.PostApplicationStartMethod(typeof(SystemStatus.Web.App_Start.SimpleInjectorWebApiInitializer), "Initialize")]

namespace SystemStatus.Web.App_Start
{
    using System.Web.Http;
    using SimpleInjector;
    using SimpleInjector.Extensions;
    using SimpleInjector.Integration.WebApi;
    using SystemStatus.Domain;
    using System;
    
    public static class SimpleInjectorWebApiInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            // Did you know the container can diagnose your configuration? 
            // Go to: https://simpleinjector.org/diagnostics
            var container = new Container();
            
            InitializeContainer(container);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
       
            container.Verify();
            
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }
     
        private static void InitializeContainer(Container container)
        {
            container.RegisterManyForOpenGeneric(typeof(IQueryHandler<,>), AppDomain.CurrentDomain.GetAssemblies());
            container.Register<IQueryProcessor, QueryProcessor>();


            // For instance:
            // container.RegisterWebApiRequest<IUserRepository, SqlUserRepository>();
        }
    }
}