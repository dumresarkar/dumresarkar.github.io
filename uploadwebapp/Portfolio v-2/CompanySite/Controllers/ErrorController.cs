using APP_COMMON;
using System;
using System.Collections;
using System.Web.Mvc;

namespace CompanySite.Controllers
{
    /// <summary>
    /// エラーコントローラ
    /// </summary>
    public class ErrorController : Controller
    {
        #region メンバ変数

        /// <summary>
        /// ビューパス
        /// </summary>
        private const String MEM_C_VIEW_PATH = "~/Views/Error/";

        #endregion

        #region アクション

        /// <summary>
        /// エラー画面表示アクション
        /// </summary>
        /// <param name="id">キー値</param>
        /// <returns>ビュー</returns>
        public ActionResult Index(string id)
        {
            try
            {
                string msg = "";
                switch (id)
                {
                    case "1":

                        //不正エラーページ
                        msg = "不正なページ要求です。";
                        break;

                    default:

                        //システムエラー
                        msg = "システムエラーが発生しました。\r\n管理者に問い合わせてください。";
                        break;
                }

                //メッセージ設定
                ViewData["msg"] = msg.Replace("\\r\\n", "<br/>");

                //return View(MEM_C_VIEW_PATH + "Error.cshtml");
                return View(MEM_C_VIEW_PATH + "internal_server_error.html");

            }
            catch (Exception ex)
            {
                //エラーログ出力
                ErrorMng.ErrorOutPut(MvcApplication.GLOBAL_SYSTEM_SETTING, ex);
                Response.StatusCode = 500;

                return View(MEM_C_VIEW_PATH + "Error.cshtml");
            }
        }

        #endregion
    }
}
