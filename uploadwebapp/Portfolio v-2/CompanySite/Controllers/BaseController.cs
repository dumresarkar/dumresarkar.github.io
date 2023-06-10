using APP_COMMON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace CompanySite.Controllers
{
    /// <summary>
    /// Baseコントローラ
    /// </summary>
    [ValidateInput(false)]
    public class BaseController : Controller
    {
        #region "メンバ変数・定数"

        /// <summary>
        /// ログの情報用
        /// </summary>
        protected Hashtable MEB_LOG_INFO;

        /// <summary>
        /// コントローラ名
        /// </summary>
        protected string MEB_CONTROLLER_NAME;

        /// <summary>
        /// ビュー名
        /// </summary>
        protected string MEB_ACTION_NAME;

        /// <summary>
        /// 言語ID
        /// </summary>
        protected string MEB_LANGUAGE_ID;

        /// <summary>
        /// パンくずリスト
        /// </summary>
        protected Dictionary<string, string> MEB_BREADCRUMB;

        #endregion

        /// <summary>
        /// BASEの初期設定
        /// </summary>
        /// <returns></returns>
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        /// <summary>
        /// アクション メソッドを呼び出す前に呼び出します。
        /// </summary>
        /// <param name="filterContext">コンテキスト</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                //言語ID
                MEB_LANGUAGE_ID = CommonConst.LanguageKbn.PUB_C_JAPANESE;

                // コントローラー名、アクション名の取得
                MEB_CONTROLLER_NAME = this.RouteData.Values["controller"].ToString().ToUpper();
                MEB_ACTION_NAME = this.RouteData.Values["action"].ToString().ToUpper();

                if (Request == null || Request.Url == null)
                {
                    filterContext.Result = Redirect(CommonConst.PUB_C_SYSMSG_URL);
                    return;
                }

                //操作ログの設定
                MEB_LOG_INFO = new Hashtable();
                MEB_LOG_INFO.Add("USER_ID", "");
                MEB_LOG_INFO.Add("LOG_URL", Request.Url.AbsoluteUri);
                MEB_LOG_INFO.Add("LOG_REMOTE_ADDR", Request.ServerVariables["REMOTE_ADDR"]);
                MEB_LOG_INFO.Add("LOG_FILE_PATH", MvcApplication.GLOBAL_SYSTEM_SETTING["LOG_FILE_PATH"]);
                MEB_LOG_INFO.Add("LOG_FILE_PREFIX", MvcApplication.GLOBAL_SYSTEM_SETTING["LOG_FILE_PREFIX"]);
                MEB_LOG_INFO.Add("LOG_OUTPUT_FLG", MvcApplication.GLOBAL_SYSTEM_SETTING["LOG_OUTPUT_FLG"]);

                //パンくずリストの初期化
                MEB_BREADCRUMB = new Dictionary<string, string>();

                //otherwise continue with action
                base.OnActionExecuting(filterContext);
            }
            catch (Exception ex)
            {
                //エラーログ出力
                ErrorMng.ErrorOutPut(MvcApplication.GLOBAL_SYSTEM_SETTING, ex);
                Response.StatusCode = 500;

                filterContext.Result = Redirect(CommonConst.PUB_C_ERROR_URL);
            }
        }

        /// <summary>
        /// アクション メソッドを呼び出し後に呼び出します。
        /// </summary>
        /// <param name="filterContext">コンテキスト</param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //パンくずリスト設定
            ViewBag.Breadcrumb = MEB_BREADCRUMB;

            base.OnActionExecuted(filterContext);
        }
    }
}