[assembly: WebActivator.PostApplicationStartMethod(typeof(SystemStatus.Web.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace SystemStatus.Web.App_Start
{
    using System.Reflection;
    using System.Web.Mvc;

    using SimpleInjector;
    using SimpleInjector.Extensions;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using SystemStatus.Domain;
    using System;
    using System.Web.Http;
    using SimpleInjector.Integration.WebApi;
    
    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            // Did you know the container can diagnose your configuration? 
            // Go to: https://simpleinjector.org/diagnostics
            var container = new Container();
            
            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);   
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
     
        private static void InitializeContainer(Container container)
        {
            container.RegisterManyForOpenGeneric(typeof(IQueryHandler<,>), AppDomain.CurrentDomain.GetAssemblies());
            container.RegisterManyForOpenGeneric(typeof(ICommandHandler<>), AppDomain.CurrentDomain.GetAssemblies());
            container.Register<IQueryProcessor, QueryProcessor>();
            // For instance:
            // container.Register<IUserRepository, SqlUserRepository>();
        }
    }
}