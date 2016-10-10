using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using SystemStatus.Domain.Migrations;

namespace SystemStatus.Domain
{
    public class SystemStatusModel : DbContext
    {
        public DbSet<SystemEvent> SystemEvents { get; set; }

        public DbSet<SystemGroup> Systems { get; set; }

        public DbSet<App> Apps { get; set; }

        public DbSet<AppEvent> AppEvents { get; set; }

        public DbSet<AppEventHookType> AppEventHookTypes { get; set; }

        public DbSet<AppEventMessage> AppEventMessages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SystemGroup>()
                .HasOptional(x => x.Parent)
                .WithMany(x => x.ChildGroups);

        }

        public static void App_Start()
        {
            Database.SetInitializer<SystemStatusModel>(new MigrateDatabaseToLatestVersion<SystemStatusModel, Configuration>());
        }

        internal void Seed()
        {
            AppEventHookTypes.AddOrUpdate(x => x.AppEventHookTypeID, new AppEventHookType() { AppEventHookTypeID = 1, Name = "Ping" });
            AppEventHookTypes.AddOrUpdate(x => x.AppEventHookTypeID, new AppEventHookType() { AppEventHookTypeID = 2, Name = "Http" });
            AppEventHookTypes.AddOrUpdate(x => x.AppEventHookTypeID, new AppEventHookType() { AppEventHookTypeID = 3, Name = "Service" });
            AppEventHookTypes.AddOrUpdate(x => x.AppEventHookTypeID, new AppEventHookType() { AppEventHookTypeID = 4, Name = "SqlServer" });
            //AppEventHookTypes.AddOrUpdate(x => x.AppEventHookTypeID == 3, new AppEventHookType() { AppEventHookTypeID = 3, Name = "Api Hook" });
            //AppEventHookTypes.AddOrUpdate(x => x.AppEventHookTypeID == 4, new AppEventHookType() { AppEventHookTypeID = 4, Name = "System Status Reciver" });
            this.SaveChanges();


//#if DEBUG

//            Apps.AddOrUpdate(x => x.AppID, new App() { AppID = 1, Name = "RPCDSQL01", MachineName=Environment.MachineName, Description = "Remote Server" });
//            Apps.AddOrUpdate(x => x.AppID, new App() { AppID = 2, Name = "www.google.com", MachineName = Environment.MachineName, Description = "Website" });
//            Apps.AddOrUpdate(x => x.AppID, new App() { AppID = 3, Name = "SQL Express", MachineName = Environment.MachineName, Description = "Local Service" });

//            foreach (var item in Apps.ToList())
//            {
//                item.MachineName = Environment.MachineName;
//            }

//            this.SaveChanges();

//            AppEventHooks.AddOrUpdate(x => x.AppEventHookID, 
//                new AppEventHook() { 
//                    AppEventHookID = 1,
//                    AppID = 1,
//                    AppEventHookTypeID = 1, 
//                    Command="10.71.4.9",
//                    Name = "Ping 10.71.4.9",
//                    Active = true,
//                    FastStatusLimit = 5,
//                    NormalStatusLimit = 10
//                });

//            AppEventHooks.AddOrUpdate(x => x.AppEventHookID,
//                new AppEventHook()
//                {
//                    AppEventHookID = 2,
//                    AppID = 2,
//                    AppEventHookTypeID = 2,
//                    HttpUrl = "http://www.google.com",
//                    Name = "www.google.com",
//                    Active = true,
//                    FastStatusLimit = 200,
//                    NormalStatusLimit = 400
//                });


//            AppEventHooks.AddOrUpdate(x => x.AppEventHookID,
//              new AppEventHook()
//              {
//                  AppEventHookID = 3,
//                  AppID = 3,
//                  AppEventHookTypeID = 3,
//                  Command = "MSSQL$SQLEXPRESS",
//                  Name = "SQL Server (SQLEXPRESS) - Service",
//                  Active = true,
//                  FastStatusLimit = 0,
//                  NormalStatusLimit = 0
//              });

//            this.SaveChanges(); 
            
//#endif          
        }
    }

    //public static class SQlLiteMigrationHelper
    //{
    //    public static void CreateTable<TEntity>(SystemStatusModel source, Expression<Func<SystemStatusModel, DbSet<TEntity>>> action)
    //        where TEntity : class, new()
    //    {
    //        //get property name
    //        var modelType = typeof(SystemStatusModel);
    //        var expression = action.Body as MemberExpression;
    //        if (expression == null) {
    //            throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.",
    //                action.ToString()));
    //        }    
    //        var propInfo = expression.Member as PropertyInfo;
    //        if (propInfo == null) { 
    //            throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.",
    //                action.ToString()));
    //        }
    //        var tableName = propInfo.Name;
    //        CreateTable(tableName, typeof(TEntity));
    //    }
    //    public static void CreateTable(string tableName, Type type)
    //    {
    //        var sb = new StringBuilder();
    //        var sql = sb.ToString();
    //    }
    //}
}
