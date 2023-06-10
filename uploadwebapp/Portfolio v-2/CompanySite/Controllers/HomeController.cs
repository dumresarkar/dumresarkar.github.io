using APP_COMMON;
using CompanySite.Models;
using System;
using System.Text;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace CompanySite.Controllers
{
    public class HomeController : BaseController
    {

        //string folderName = @"C:\Users\Sabin Dumre\Desktop\CompanySite1 orginal\CompanySite\filesupload";
        //System.IO.Directory.CreateDirectory(pathStrin


        #region メンバ変数

        /// <summary>
        /// ビューパス
        /// </summary>
        private const String MEM_C_VIEW_PATH = "~/Views/Home/";
        

        #endregion

        #region アクション
        /// <summary>
        /// トップ画面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            Home model = new Home();

            try
            {
                //エラーメッセージ情報取得
                if (TempData[CommonConst.TempDataName.PUB_C_ERROR_MSG] != null)
                {
                    model.ErrorList = (Dictionary<string, string>)TempData[CommonConst.TempDataName.PUB_C_ERROR_MSG];
                }

                //セッションの確認
                if (Session[CommonConst.SessionName.PUB_C_FORM] != null && Session[CommonConst.SessionName.PUB_C_FORM] is Home.ContactItem)
                {
                    //セッションから情報取得
                    model.Contact = (Home.ContactItem)Session[CommonConst.SessionName.PUB_C_FORM];
                    Session.Remove(CommonConst.SessionName.PUB_C_FORM);
                }

                ////初期表示処理
                //InitProcess(ref model);

                return View(model);
            }
            catch (Exception ex)
            {
                //エラーログ出力
                ErrorMng.ErrorOutPut(MvcApplication.GLOBAL_SYSTEM_SETTING, ex);
                Response.StatusCode = 500;

                return Redirect(CommonConst.PUB_C_ERROR_URL);
            }
            finally
            {
                //インスタンスを破棄
                model.Dispose();
            }

        }

        /// <summary>
        /// 問い合わせ入力画面
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            Home model = new Home();

            try
            {
                //エラーメッセージ情報取得
                if (TempData[CommonConst.TempDataName.PUB_C_ERROR_MSG] != null)
                {
                    model.ErrorList = (Dictionary<string, string>)TempData[CommonConst.TempDataName.PUB_C_ERROR_MSG];
                }

                //セッションの確認
                if (Session[CommonConst.SessionName.PUB_C_FORM] != null && Session[CommonConst.SessionName.PUB_C_FORM] is Home.ContactItem)
                {
                    //セッションから情報取得
                    model.Contact = (Home.ContactItem)Session[CommonConst.SessionName.PUB_C_FORM];
                    Session.Remove(CommonConst.SessionName.PUB_C_FORM);
                }

                ////初期表示処理
                //InitProcess(ref model);

                //パンくず設定
                MEB_BREADCRUMB.Add("", "問い合わせ入力");

                return View(model);
            }
            catch (Exception ex)
            {
                //エラーログ出力
                ErrorMng.ErrorOutPut(MvcApplication.GLOBAL_SYSTEM_SETTING, ex);
                Response.StatusCode = 500;

                return Redirect(CommonConst.PUB_C_ERROR_URL);
            }
            finally
            {
                //インスタンスを破棄
                model.Dispose();
            }

        }

            /// <summary>
            /// 問い合わせ確認画面
            /// </summary>
            /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactConfirm(HttpPostedFileBase file, HttpPostedFileBase pic)
        //public ActionResult ContactConfirm(HttpPostedFileBase picture)
        {
            Home model = new Home();
            string message1 = "{0}を入力してください。";
            string message2 = "{0}の入力値が不正です。";
            string message3 = "{0}needed";

            try
            {
                //画面の値取得
                model.Contact.NAME = BaseMng.GetRequestFormParam(Request, "name");
                model.Contact.EMAIL = BaseMng.GetRequestFormParam(Request, "email");
                model.Contact.SUBJECT = BaseMng.GetRequestFormParam(Request, "subject");
                model.Contact.MESSAGE = BaseMng.GetRequestFormParam(Request, "message");
                model.Contact.ADDRESS = BaseMng.GetRequestFormParam(Request, "address");
                //model.Contact.HOBBIES = BaseMng.GetRequestFormParam(Request, "hob");
                model.Contact.HOBBIES = BaseMng.GetRequestFormParam(Request, "hob").Split(',').ToList();
                model.Contact.GENDER = BaseMng.GetRequestFormParam(Request, "gender");
                model.Contact.BACKSTORY = BaseMng.GetRequestFormParam(Request, "backstory");
                model.Contact.COURSE = BaseMng.GetRequestFormParam(Request, "course");
                //model.Contact.FILE = BaseMng.GetRequestFormParam(Request, "file");
                if(file != null)
                {
                    String FileName = System.IO.Path.GetFileName(file.FileName);
                    String FileExtension = System.IO.Path.GetExtension(file.FileName);
                    //String FilePath = System.IO.Path.GetFullPath(file.FileName);
                    model.Contact.FILE = FileName;
                    model.Contact.FILEEXT = FileExtension;
                    String FilePath = Server.MapPath("~/filesupload");
                    model.Contact.FILEPATH = FilePath;
                    string filename = Path.GetFileName(file.FileName);
                    string filepath = Path.Combine(Server.MapPath("~/filesupload"), filename);
                    file.SaveAs(filepath);

                }
                if (pic != null)
                {
                    String PictureName = System.IO.Path.GetFileName(pic.FileName);
                    String PictureExtension = System.IO.Path.GetExtension(pic.FileName);
                    String PicturePath = Server.MapPath("~/filesupload/pictureupload");
                    model.Contact.PICTURE = PictureName;
                    model.Contact.PICTUREEXT = PictureExtension;
                    model.Contact.PICTUREPATH = PicturePath;
                    string picname = Path.GetFileName(pic.FileName);
                    string picpath = Path.Combine(Server.MapPath("~/filesupload/pictureupload"), picname);
                    pic.SaveAs(picpath);
                    System.Drawing.Image sourceimage = System.Drawing.Image.FromStream(pic.InputStream);
                    String[] imagefiles = Directory.GetFiles(PicturePath);
                    ViewBag.images = imagefiles;
                }

                if (BaseMng.GetRequestFormParam(Request, "hdnContactPageFlg") == "1")
                {
                    model.Contact.RedirectPage = "/contact/";
                }
                else
                {
                    model.Contact.RedirectPage = "/#inquiry";
                }

                try
                {
                    model.Contact.MESSAGE = BaseMng.GetRequestFormParam(Request, "message", 1);
                }
                catch (Exception ex)
                {
                    ErrorMng.LogOutPut(MvcApplication.GLOBAL_SYSTEM_SETTING, ex.ToString(), mailSendFlg: false);

                    model.ErrorList.Add("txtMessage", string.Format(message2, "メッセージ"));
                    TempData[CommonConst.TempDataName.PUB_C_ERROR_MSG] = model.ErrorList;
                    Session[CommonConst.SessionName.PUB_C_FORM] = model.Contact;
                    return Redirect(model.Contact.RedirectPage);
                }

                //企業名(名前)
                if (string.IsNullOrEmpty(model.Contact.NAME))
                {
                    model.ErrorList.Add("txtName", string.Format(message1, "企業名(名前)"));
                }

                //Eメール
                if (string.IsNullOrEmpty(model.Contact.EMAIL))
                {
                    model.ErrorList.Add("txtEmail", string.Format(message1, "Eメール"));
                }

                //件名
                if (string.IsNullOrEmpty(model.Contact.SUBJECT))
                {
                    model.ErrorList.Add("txtSubject", string.Format(message1, "件名"));
                }

                //メッセージ
                if (string.IsNullOrEmpty(model.Contact.MESSAGE))
                {
                    model.ErrorList.Add("txtMessage", string.Format(message1, "メッセージ"));
                }

                //住所
                if (string.IsNullOrEmpty(model.Contact.ADDRESS))
                {
                    model.ErrorList.Add("txtAddress", string.Format(message3, "Address"));
                }
                //Hobbies
                //if (List<string>.IsNullOrEmpty(model.Contact.HOBBIES))
                //{
                //    model.ErrorList.Add("txtHob", string.Format(message1, "Hobbies"));
                //}
                //Gender
                if (string.IsNullOrEmpty(model.Contact.GENDER))
                {
                    model.ErrorList.Add("txtGender", string.Format(message3, "Gender"));
                }
                //Backstory
                if (string.IsNullOrEmpty(model.Contact.BACKSTORY))
                {
                    model.ErrorList.Add("txtBackstory", string.Format(message3, "Backstory"));
                }
                //Course
                if (string.IsNullOrEmpty(model.Contact.COURSE))
                {
                    model.ErrorList.Add("txtCourse", string.Format(message3, "Course"));
                }

                //file
                //if (string.IsNullOrEmpty(model.Contact.FILE))
                //{
                //    model.ErrorList.Add("txtFile", string.Format(message3, "File"));
                //}
                //add verification


                if (model.ErrorList.Count >= 1)
                {
                    TempData[CommonConst.TempDataName.PUB_C_ERROR_MSG] = model.ErrorList;
                    Session[CommonConst.SessionName.PUB_C_FORM] = model.Contact;
                    return Redirect(model.Contact.RedirectPage);
                }

                //パンくず設定
                MEB_BREADCRUMB.Add("/contact/", "問い合わせ入力");
                MEB_BREADCRUMB.Add("", "問い合わせ確認");

                model.Contact.CheckComplete = true;
                Session[CommonConst.SessionName.PUB_C_FORM] = model.Contact;

                return View(model);
            }
            catch (Exception ex)
            {
                //エラーログ出力
                ErrorMng.ErrorOutPut(MvcApplication.GLOBAL_SYSTEM_SETTING, ex);
                Response.StatusCode = 500;

                return Redirect(CommonConst.PUB_C_ERROR_URL);
            }
            finally
            {
                //インスタンスを破棄
                model.Dispose();
            }
        }

        /// <summary>
        /// 問い合わせ完了画面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactComplete()
        {
            Home model = new Home();

            try
            {
                //セッションの確認
                if (Session[CommonConst.SessionName.PUB_C_FORM] != null && Session[CommonConst.SessionName.PUB_C_FORM] is Home.ContactItem)
                {
                    //セッションから情報取得
                    model.Contact = (Home.ContactItem)Session[CommonConst.SessionName.PUB_C_FORM];
                    if (!model.Contact.CheckComplete)
                    {
                        return Redirect(model.Contact.RedirectPage);
                    }
                    Session.Remove(CommonConst.SessionName.PUB_C_FORM);
                }
                else
                {
                    return Redirect("/contact/");
                }

                //メール送信
                if (!MailSendForAdmin(ref model))
                {
                    //エラーメッセージ？
                    //なにもしない。ログは表示されているため
                }

                //DB登録がエラーでもエラーページに遷移しません。メール送信完了しているため
                try
                {
                    //dbに登録する
                    //InsertInquiry(ref model);
                    int cid = Insertdoc(ref model);
                    
                    foreach(var item in model.Contact.HOBBIES)
                    {
                        Inserthobbies(ref model, cid, item);                        
                    }

                }
                catch (Exception ex)
                {
                    //エラーログ出力
                    ErrorMng.ErrorOutPut(MvcApplication.GLOBAL_SYSTEM_SETTING, ex);
                }

                //パンくず設定
                MEB_BREADCRUMB.Add("/contact/", "問い合わせ入力");
                MEB_BREADCRUMB.Add("", "問い合わせ完了");

                return View(model);
            }
            catch (Exception ex)
            {
                //エラーログ出力
                ErrorMng.ErrorOutPut(MvcApplication.GLOBAL_SYSTEM_SETTING, ex);
                Response.StatusCode = 500;

                return Redirect(CommonConst.PUB_C_ERROR_URL);
            }
            finally
            {
                //インスタンスを破棄
                model.Dispose();
            }
        }

        #endregion

        #region "メソッド"

        #region "クラスモジュール"

        /// <summary>
        /// 管理者にメール送信する。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool MailSendForAdmin(ref Home model)
        {
            string mailTo = "";
            string title = "";
            string body = "";
            try
            {
                //管理者にメール送信する
                mailTo = MvcApplication.GLOBAL_SYSTEM_SETTING["SITE_ADMIN_MAIL_TO"].ToString();
                title = "【" + Request.Url.Host + "】" + model.Contact.SUBJECT;
                body = model.Contact.MESSAGE + Environment.NewLine + Environment.NewLine;
                body = body + "===================================================" + Environment.NewLine;
                body = body + "【NAME】  : " + model.Contact.NAME + Environment.NewLine;
                body = body + "【E-MAIL】: " + model.Contact.EMAIL + Environment.NewLine;

                //メール送信
                BaseMng.SendMail(MvcApplication.GLOBAL_SYSTEM_SETTING, MvcApplication.GLOBAL_SYSTEM_SETTING["SYSTEM_MAIL_FROM"].ToString(), mailTo,
                                title, body, MvcApplication.GLOBAL_SYSTEM_SETTING["SMTP_SERVER"].ToString(), "", "", "",
                                MvcApplication.GLOBAL_SYSTEM_SETTING["USER_NAME"].ToString(), MvcApplication.GLOBAL_SYSTEM_SETTING["PASSWORD"].ToString(), Convert.ToInt32(MvcApplication.GLOBAL_SYSTEM_SETTING["SMTP_PORT"].ToString()), true);

                return true;
            }
            catch (Exception ex)
            {
                //エラーログ出力
                ErrorMng.ErrorOutPut(MvcApplication.GLOBAL_SYSTEM_SETTING, ex);
                ErrorMng.LogOutPut(MvcApplication.GLOBAL_SYSTEM_SETTING, mailTo + ">>" + title + ">>" + body, mailSendFlg: false);
                return false;
            }

        }

        #endregion

        #region "チェック"
        #endregion

        #region "SQL"

        #region "SELECT"

        /// <summary>
        /// 発注社の件数取得
        /// </summary>
        /// <param name="con">DB接続オブジェクト</param>
        /// <returns>件数</returns>
        private int GetCompanyCount(ref DataBaseUtil con)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            try
            {
                //SQL文生成
                sqlBuilder.AppendLine(" SELECT ");
                sqlBuilder.AppendLine("      COUNT(COMPANY_ID) CNT");
                sqlBuilder.AppendLine(" FROM");
                sqlBuilder.AppendLine("     T_COMPANY");

                //SQL文設定
                con.Sql = sqlBuilder.ToString();

                return Convert.ToInt32(con.Execute(MEB_LOG_INFO, "").Tables[0].Rows[0]["CNT"]);
            }
            finally
            {
                //インスタンスを破棄
                sqlBuilder.Clear();
            }
        }

        #endregion

        #region "INSERT"

        /// <summary>
        /// 問い合わせ情報を登録
        /// </summary>
        /// <param name="model">情報格納用モデル</param>
        /// <returns>True:処理成功、False:処理失敗</returns>
        private bool InsertInquiry(ref Home model)
        {
            DataBaseUtil con = new DataBaseUtil();
            StringBuilder sqlBuilder;

            try
            {
                //DB接続
                con.Connect(MvcApplication.GLOBAL_DB_CONNECT);

                //SQL生成
                sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine(" INSERT INTO T_INQUIRY");
                sqlBuilder.AppendLine("     (");
                sqlBuilder.AppendLine("          SITE");
                sqlBuilder.AppendLine("         ,COMPANY_URL_KEY");
                sqlBuilder.AppendLine("         ,NAME");
                sqlBuilder.AppendLine("         ,EMAIL");
                sqlBuilder.AppendLine("         ,SUBJECT");
                sqlBuilder.AppendLine("         ,MESSAGE");
                sqlBuilder.AppendLine("         ,ADDRESS");
                
                sqlBuilder.AppendLine("         ,GENDER");
                sqlBuilder.AppendLine("         ,BACKSTORY");
                sqlBuilder.AppendLine("         ,COURSE");
                sqlBuilder.AppendLine("         ,DESTINATION_EMAIL");
                sqlBuilder.AppendLine("         ,REG_USER");
                sqlBuilder.AppendLine("         ,REG_DTM");
                sqlBuilder.AppendLine("     )");
                sqlBuilder.AppendLine(" VALUES");
                sqlBuilder.AppendLine("     (");
                sqlBuilder.AppendLine("          @SITE");
                sqlBuilder.AppendLine("         ,@COMPANY_URL_KEY");
                sqlBuilder.AppendLine("         ,@NAME");
                sqlBuilder.AppendLine("         ,@EMAIL");
                sqlBuilder.AppendLine("         ,@SUBJECT");
                sqlBuilder.AppendLine("         ,@MESSAGE");
                sqlBuilder.AppendLine("         ,@ADDRESS");
               
                sqlBuilder.AppendLine("         ,@GENDER");
                sqlBuilder.AppendLine("         ,@BACKSTORY");
                sqlBuilder.AppendLine("         ,@COURSE");
                sqlBuilder.AppendLine("         ,@DESTINATION_EMAIL");
                sqlBuilder.AppendLine("         ,@REG_USER");
                sqlBuilder.AppendLine("         ,GETDATE()");
                sqlBuilder.AppendLine("     )");

                //SQL文設定
                con.Sql = sqlBuilder.ToString();

                //パラメータ設定
                con.ParamAppend("SITE", BaseMng.StringSubstring(Request.Url.Host, 100));
                con.ParamAppend("COMPANY_URL_KEY", "CompanySite");
                con.ParamAppend("NAME", BaseMng.StringSubstring(model.Contact.NAME, 100));
                con.ParamAppend("EMAIL", BaseMng.StringSubstring(model.Contact.EMAIL, 100));
                con.ParamAppend("SUBJECT", BaseMng.StringSubstring(model.Contact.SUBJECT, 200));
                con.ParamAppend("MESSAGE", BaseMng.StringSubstring(model.Contact.MESSAGE, 3000));
              
                con.ParamAppend("HOBBIES", BaseMng.StringSubstring(model.Contact.HOBBIES, 100));
                con.ParamAppend("GENDER", BaseMng.StringSubstring(model.Contact.GENDER, 100));
                con.ParamAppend("BACKSTORY", BaseMng.StringSubstring(model.Contact.BACKSTORY, 3000));
                con.ParamAppend("COURSE", BaseMng.StringSubstring(model.Contact.COURSE, 100));
                con.ParamAppend("DESTINATION_EMAIL", BaseMng.StringSubstring(MvcApplication.GLOBAL_SYSTEM_SETTING["SITE_ADMIN_MAIL_TO"].ToString(), 200));
                con.ParamAppend("REG_USER", BaseMng.StringSubstring(MEB_LOG_INFO["LOG_REMOTE_ADDR"].ToString(), 50));

                //SQL発行
                con.Execute(MEB_LOG_INFO);

                return true;
            }
            catch (Exception ex)
            {
                //エラーログ出力
                ErrorMng.ErrorOutPut(MvcApplication.GLOBAL_SYSTEM_SETTING, ex);
                return false;
            }
            finally
            {
                //DB切断
                con.DisConnect();
                con.Dispose();
            }
        }
        private int Insertdoc(ref Home model)
        {
            DataBaseUtil con = new DataBaseUtil();
            StringBuilder sqlBuilder;

            try
            {
                //DB接続
                con.Connect(MvcApplication.GLOBAL_DB_CONNECT);

                //SQL生成
                sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine(" INSERT INTO T_CONTACT");
                sqlBuilder.AppendLine("     (");
                sqlBuilder.AppendLine("          NAME");
                sqlBuilder.AppendLine("         ,EMAIL");
                sqlBuilder.AppendLine("         ,SUBJECT");
                sqlBuilder.AppendLine("         ,MESSAGE");
                sqlBuilder.AppendLine("         ,ADDRESS");
               
                sqlBuilder.AppendLine("         ,GENDER");
                sqlBuilder.AppendLine("         ,BACKSTORY"); 
                sqlBuilder.AppendLine("         ,COURSE");
                sqlBuilder.AppendLine("         ,FILENAME");
                sqlBuilder.AppendLine("         ,FILEEXTENSION");
                sqlBuilder.AppendLine("         ,FILEPATH");
                sqlBuilder.AppendLine("         ,PICTURE");
                sqlBuilder.AppendLine("         ,PICTUREEXTENSION");
                sqlBuilder.AppendLine("         ,PICTUREPATH");
                sqlBuilder.AppendLine("         )");
                sqlBuilder.AppendLine(" VALUES");
                sqlBuilder.AppendLine("     (");
                sqlBuilder.AppendLine("         @NAME");
                sqlBuilder.AppendLine("         ,@EMAIL");
                sqlBuilder.AppendLine("         ,@SUBJECT");
                sqlBuilder.AppendLine("         ,@MESSAGE");
                sqlBuilder.AppendLine("         ,@ADDRESS");
               
                sqlBuilder.AppendLine("         ,@GENDER");
                sqlBuilder.AppendLine("         ,@BACKSTORY");
                sqlBuilder.AppendLine("         ,@COURSE");
                sqlBuilder.AppendLine("         ,@FILENAME");
                sqlBuilder.AppendLine("         ,@FILEEXTENSION");
                sqlBuilder.AppendLine("         ,@FILEPATH");
                sqlBuilder.AppendLine("         ,@PICTURE");
                sqlBuilder.AppendLine("         ,@PICTUREEXTENSION");
                sqlBuilder.AppendLine("         ,@PICTUREPATH");
                sqlBuilder.AppendLine("         )");

                //SQL文設定
                con.Sql = sqlBuilder.ToString();
                con.ParamAppend("NAME", BaseMng.StringSubstring(model.Contact.NAME, 100));
                con.ParamAppend("EMAIL", BaseMng.StringSubstring(model.Contact.EMAIL, 100));
                con.ParamAppend("SUBJECT", BaseMng.StringSubstring(model.Contact.SUBJECT, 200));
                con.ParamAppend("MESSAGE", BaseMng.StringSubstring(model.Contact.MESSAGE, 3000));
                con.ParamAppend("ADDRESS", BaseMng.StringSubstring(model.Contact.ADDRESS, 100));
                
                con.ParamAppend("GENDER", BaseMng.StringSubstring(model.Contact.GENDER, 100));
                con.ParamAppend("BACKSTORY", BaseMng.StringSubstring(model.Contact.BACKSTORY, 3000));
                con.ParamAppend("COURSE", BaseMng.StringSubstring(model.Contact.COURSE, 100));
                con.ParamAppend("FILENAME", BaseMng.StringSubstring(model.Contact.FILE, 100));
                con.ParamAppend("FILEEXTENSION", BaseMng.StringSubstring(model.Contact.FILEEXT, 100));
                con.ParamAppend("FILEPATH", BaseMng.StringSubstring(model.Contact.FILEPATH, 100));
                con.ParamAppend("PICTURE", BaseMng.StringSubstring(model.Contact.PICTURE, 100));
                con.ParamAppend("PICTUREEXTENSION", BaseMng.StringSubstring(model.Contact.PICTUREEXT, 100));
                con.ParamAppend("PICTUREPATH", BaseMng.StringSubstring(model.Contact.PICTUREPATH, 100));


                //con.ParamAppend("DESTINATION_EMAIL", BaseMng.StringSubstring(MvcApplication.GLOBAL_SYSTEM_SETTING["SITE_ADMIN_MAIL_TO"].ToString(), 200));
                //con.ParamAppend("REG_USER", BaseMng.StringSubstring(MEB_LOG_INFO["LOG_REMOTE_ADDR"].ToString(), 50));

                //SQL発行
                con.Execute(MEB_LOG_INFO);
                int id = con.LastInsertID();
                return id;
            }
            finally
            {
                //DB切断
                con.DisConnect();
                con.Dispose();
            }
        }
        private bool Inserthobbies(ref Home model,int id, string HID)
        {
            DataBaseUtil con = new DataBaseUtil();
            StringBuilder sqlBuilder;

            try
            {
                //DB接続
                con.Connect(MvcApplication.GLOBAL_DB_CONNECT);

                //SQL生成
                sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine(" INSERT INTO T2_CONTACT");
                sqlBuilder.AppendLine("     (");
                sqlBuilder.AppendLine("          C_ID");
                sqlBuilder.AppendLine("         ,HOBBIES");
               
                sqlBuilder.AppendLine("         )");
                sqlBuilder.AppendLine(" VALUES");
                sqlBuilder.AppendLine("     (");
                sqlBuilder.AppendLine("          @C_ID");
                sqlBuilder.AppendLine("         ,@HOBBIES");
              
                sqlBuilder.AppendLine("         )");

                       
                    //SQL文設定  
                    con.Sql = sqlBuilder.ToString();
                    con.ParamAppend("C_ID", id);
                    con.ParamAppend("HOBBIES", HID);
                    //SQL発行
                    con.Execute(MEB_LOG_INFO);
                
                
                

               

                return true;
            }
            catch (Exception ex)
            {
                //エラーログ出力
                ErrorMng.ErrorOutPut(MvcApplication.GLOBAL_SYSTEM_SETTING, ex);
                return false;
            }
            finally
            {
                //DB切断
                con.DisConnect();
                con.Dispose();
            }
        }


        



        #endregion

        #region "UPDATE"
        #endregion

        #region "DELETE"
        #endregion

        #endregion

        #endregion
    }
}