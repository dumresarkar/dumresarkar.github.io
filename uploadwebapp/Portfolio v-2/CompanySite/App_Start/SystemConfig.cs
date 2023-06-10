using System.Collections;
using System.Configuration;

namespace CompanySite
{
    public class SystemConfig
    {
        /// <summary>
        /// Configファイルの情報取得
        /// </summary>
        public static void Setting()
        {
            //グローバル変数設定
            MvcApplication.GLOBAL_DB_CONNECT = ConfigurationManager.ConnectionStrings["APP_DB"].ConnectionString;

            //web.config設定取得
            MvcApplication.GLOBAL_SYSTEM_SETTING = new Hashtable();
            MvcApplication.GLOBAL_SYSTEM_SETTING.Add("ERROR_LOG_FILE_PATH", ConfigurationManager.AppSettings["ErrorLogFilePath"]);
            MvcApplication.GLOBAL_SYSTEM_SETTING.Add("LOG_FILE_PATH", ConfigurationManager.AppSettings["LogFilePath"]);
            MvcApplication.GLOBAL_SYSTEM_SETTING.Add("LOG_FILE_PREFIX", ConfigurationManager.AppSettings["LogFilePrefix"]);
            MvcApplication.GLOBAL_SYSTEM_SETTING.Add("LOG_OUTPUT_FLG", ConfigurationManager.AppSettings["LogOutputFlg"]);
            MvcApplication.GLOBAL_SYSTEM_SETTING.Add("ROOT_URL_BO", ConfigurationManager.AppSettings["RootUrlBO"]);
            MvcApplication.GLOBAL_SYSTEM_SETTING.Add("IMAGE_URL", ConfigurationManager.AppSettings["ImageUrl"]);

            //メール設定情報
            MvcApplication.GLOBAL_SYSTEM_SETTING.Add("SMTP_SERVER", ConfigurationManager.AppSettings["SMTPServer"]);
            MvcApplication.GLOBAL_SYSTEM_SETTING.Add("SMTP_PORT", ConfigurationManager.AppSettings["SMTPPort"]);
            MvcApplication.GLOBAL_SYSTEM_SETTING.Add("USER_NAME", ConfigurationManager.AppSettings["UserName"]);
            MvcApplication.GLOBAL_SYSTEM_SETTING.Add("PASSWORD", ConfigurationManager.AppSettings["Password"]);
            MvcApplication.GLOBAL_SYSTEM_SETTING.Add("SYSTEM_MAIL_FROM", ConfigurationManager.AppSettings["SystemMailFrom"]);
            MvcApplication.GLOBAL_SYSTEM_SETTING.Add("SYSTEM_MAIL_TO", ConfigurationManager.AppSettings["SystemMailTo"]);
            MvcApplication.GLOBAL_SYSTEM_SETTING.Add("ERROR_MAIL_TITLE", ConfigurationManager.AppSettings["ErrorMailTitle"]);
            MvcApplication.GLOBAL_SYSTEM_SETTING.Add("SITE_ADMIN_MAIL_TO", ConfigurationManager.AppSettings["SiteAdminMailTo"]);

            //操作ログ出力用
            MvcApplication.GLOBAL_OPERATION_LOG = new Hashtable();
            MvcApplication.GLOBAL_OPERATION_LOG.Add("USER_ID", "");
            MvcApplication.GLOBAL_OPERATION_LOG.Add("LOG_FILE_PATH", MvcApplication.GLOBAL_SYSTEM_SETTING["LOG_FILE_PATH"].ToString());
            MvcApplication.GLOBAL_OPERATION_LOG.Add("LOG_FILE_PREFIX", MvcApplication.GLOBAL_SYSTEM_SETTING["LOG_FILE_PREFIX"].ToString());
            MvcApplication.GLOBAL_OPERATION_LOG.Add("LOG_OUTPUT_FLG", MvcApplication.GLOBAL_SYSTEM_SETTING["LOG_OUTPUT_FLG"].ToString());
            MvcApplication.GLOBAL_OPERATION_LOG.Add("LOG_URL", "");
            MvcApplication.GLOBAL_OPERATION_LOG.Add("LOG_REMOTE_ADDR", "");
            MvcApplication.GLOBAL_OPERATION_LOG.Add("CONSULTANT_KEYID", "");
        }
    }
}