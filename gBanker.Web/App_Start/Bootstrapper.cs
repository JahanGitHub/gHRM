using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using eRecruitment.Infrastructure.Service.CacheManagerServices;
using gHRM.Data;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using gHRM.Service;
using gHRM.Service.CacheManagerServices;
using gHRM.Service.eRecruit;
using gHRM.Web.Helpers;
using gHRM.Web.Infrastucture.Framework;
using gHRM.Web.Mappings;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace gHRM.Web
{
    public static class Bootstrapper
    {
        //This method will be called when the application runs for the first time....
        public static void Run()
        {
            SetAutofacContainer();
            //Configure AutoMapper
            AutoMapperConfiguration.Configure();
        }
        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UnitOfWorkCodeFirst>().As<IUnitOfWorkCodeFirst>().InstancePerRequest();

            builder.RegisterType<DatabaseFactoryCodeFirst>().As<IDatabaseFactoryCodeFirst>().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(EmployeeRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(EmployeeService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterType<Logger>().As<ILogger>().InstancePerRequest();

            builder.RegisterType<CacheManagerService>().As<ICacheManagerService>().InstancePerRequest();

            builder.Register(c => new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new UserManagementEntities())))
                .As<UserManager<ApplicationUser>>().InstancePerRequest();

            builder.RegisterType<WorkContext>().As<IWorkContext>().InstancePerRequest();

            //this line can be used for HttpContextBase resolve purpose
            builder.RegisterModule(new AutofacWebTypesModule());

            //for web api
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterFilterProvider();
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //for web api
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}