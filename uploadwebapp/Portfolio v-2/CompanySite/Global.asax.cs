using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CompanySite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //グローバル変数定義
        public static string GLOBAL_DB_CONNECT;
        public static Hashtable GLOBAL_SYSTEM_SETTING;  //web.config appSettings定義
        public static Hashtable GLOBAL_OPERATION_LOG;   //操作ログ用の定義
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SystemConfig.Setting();
        }
    }
}
