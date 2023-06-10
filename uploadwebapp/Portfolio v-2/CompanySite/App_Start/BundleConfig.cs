using System.Web;
using System.Web.Optimization;

namespace CompanySite
{
    public class BundleConfig
    {
        // バンドルの詳細については、https://go.microsoft.com/fwlink/?LinkId=301862 を参照してください
        public static void RegisterBundles(BundleCollection bundles)
        {
            // 開発と学習には、Modernizr の開発バージョンを使用します。次に、実稼働の準備が
            // 運用の準備が完了したら、https://modernizr.com のビルド ツールを使用し、必要なテストのみを選択します。
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/project_script").Include(
                      "~/Scripts/_common.js"));

            //bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
            //          "~/Content/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/project_style").Include(
                      "~/Content/css/Site.css",
                      "~/Content/css/_common.css",
                      "~/Content/css/_media-style.css"));
        }
    }
}
