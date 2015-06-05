using DozorDatabaseLib;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DozorWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Create Database instance "D:\\Votum\\DozorDatabase\\Students.fdbB;"
            FbConnectionStringBuilder connectString = new FbConnectionStringBuilder();
            connectString.Database = "D:\\Votum\\DozorDatabase\\Students.fdb";
            connectString.Dialect = 3;
            connectString.UserID = "SYSDBA";
            connectString.Password = "masterkey";
            connectString.Charset = "win1251";
            DozorDatabase.CreateInstance(connectString.ConnectionString);
        }
    }
}
