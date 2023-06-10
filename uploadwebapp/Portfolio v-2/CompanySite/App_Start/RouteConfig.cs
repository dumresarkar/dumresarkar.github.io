using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CompanySite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //TOP
            routes.MapRoute(
                name: "top",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );

            //エラー
            routes.MapRoute(
                name: "error",
                url: "error/{action}/{id}/",
                defaults: new { controller = "Error", action = "Index", id = UrlParameter.Optional }
            );

            //問い合わせ入力
            routes.MapRoute(
                name: "Contact",
                url: "contact",
                defaults: new { controller = "Home", action = "Contact" }
            );

            //問い合わせ確認
            routes.MapRoute(
                name: "ContactConfirm",
                url: "contactconfirm",
                defaults: new { controller = "Home", action = "ContactConfirm" }
            );

            //問い合わせ完了
            routes.MapRoute(
                name: "ContactComplete",
                url: "contactcomplete",
                defaults: new { controller = "Home", action = "ContactComplete" }
            );

            //個人情報保護方針
            routes.MapRoute(
                name: "privacy",
                url: "privacy",
                defaults: new { controller = "Policy", action = "Privacy" }
            );

            routes.MapRoute(
                name: "Test",
                url: "newpage/{action}/{id}",
                defaults: new { controller = "Newpage", action = "Test", id = UrlParameter.Optional }
            );
         //   routes.MapRoute(
         //      name: "Submitconfirm",
         //      url: "submitconfirm",
         //      defaults: new { controller = "Newpage", action = "Submitconfirm" }
         //  );
         //   routes.MapRoute(
         //      name: "Submitcomplete",
         //      url: "Submitcomplete",
         //      defaults: new { controller = "Newpage", action = "SubmitComplete" }
         //  );
         //   routes.MapRoute(
         //    name: "ShowRegister",
         //    url: "ShowRegister",
         //    defaults: new { controller = "Newpage", action = "ShowRegister" }
         //);
            //routes.MapRoute(
            //    name: "ShowRegister",
            //    url: "newpage/{action}/{id}",
            //    defaults: new { controller = "Newpage", action = "ShowRegister", id = UrlParameter.Optional }
            //);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
