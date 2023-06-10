using APP_COMMON;
using CompanySite.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;

namespace CompanySite.Controllers
{
    public class NewpageController : Controller
    {
        public Hashtable MEB_LOG_INFO { get; private set; }

        // GET: Newpage
        public ActionResult Test(string id)
        {

            Newpage model = new Newpage();

            try
            {
                //エラーメッセージ情報取得
                if (TempData[CommonConst.TempDataName.PUB_C_ERROR_MSG] != null)
                {
                    model.ErrorList = (Dictionary<string, string>)TempData[CommonConst.TempDataName.PUB_C_ERROR_MSG];
                }

                //セッションの確認
                if (Session[CommonConst.SessionName.PUB_C_FORM] != null && Session[CommonConst.SessionName.PUB_C_FORM] is Newpage.registerItem)
                {
                    //セッションから情報取得
                    model.Register = (Newpage.registerItem)Session[CommonConst.SessionName.PUB_C_FORM];
                    Session.Remove(CommonConst.SessionName.PUB_C_FORM);
                }

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
        public ActionResult Register(string id)
        {
            Newpage model = new Newpage();
            if (TempData[CommonConst.TempDataName.PUB_C_ERROR_MSG] != null)
            {
                model.ErrorList = (Dictionary<string, string>)TempData[CommonConst.TempDataName.PUB_C_ERROR_MSG];
            }

            //セッションの確認
            if (Session[CommonConst.SessionName.PUB_C_FORM] != null && Session[CommonConst.SessionName.PUB_C_FORM] is Newpage.registerItem)
            {
                //セッションから情報取得
                model.Register = (Newpage.registerItem)Session[CommonConst.SessionName.PUB_C_FORM];
                Session.Remove(CommonConst.SessionName.PUB_C_FORM);
            }
            model.Register.MODE = CommonConst.Mode.PUB_C_REGIST;

            Session[CommonConst.SessionName.PUB_C_FORM] = model.Register;
            //model.ErrorList = new Dictionary<string, string>();

            return View("register", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Submitconfirm(HttpPostedFileBase file, HttpPostedFileBase pic)
        {
            Newpage model = new Newpage();
            string message1 = "{0}を入力してください。";
            string message2 = "{0}の入力値が不正です。";
            string message3 = "{0}  needed";

            try
            {
                if (Session[CommonConst.SessionName.PUB_C_FORM] != null && Session[CommonConst.SessionName.PUB_C_FORM] is Newpage.registerItem)
                {
                    //セッションから情報取得
                    model.Register = (Newpage.registerItem)Session[CommonConst.SessionName.PUB_C_FORM];
                }
                //画面の値取得
                model.Register.FULL_NAME = BaseMng.GetRequestFormParam(Request, "name");
                model.Register.EMAIL = BaseMng.GetRequestFormParam(Request, "email");
                model.Register.CONTACT_NUMBER = BaseMng.GetRequestFormParam(Request, "number");
                model.Register.ADDRESS = BaseMng.GetRequestFormParam(Request, "address");
                model.Register.DATE_OF_BIRTH = BaseMng.GetRequestFormParam(Request, "date_of_birth");
                model.Register.GENDER = BaseMng.GetRequestFormParam(Request, "gender");
                model.Register.YOURSELF = BaseMng.GetRequestFormParam(Request, "yourself");
                //model.Register.C_ID = BaseMng.GetRequestFormParam(Request, "C_ID");
                //..........
                if (pic != null)
                {
                    String PictureName = System.IO.Path.GetFileName(pic.FileName);
                    String PictureExtension = System.IO.Path.GetExtension(pic.FileName);
                    String PicturePath = Server.MapPath("~/Files_Registered/Pic_Registered/");
                    model.Register.PICTURE = PictureName;
                    model.Register.PICTUREEXT = PictureExtension;
                    model.Register.PICTUREPATH = PicturePath;

                }
                else
                {
                    model.ErrorList.Add("pic", string.Format(message3, "Picture"));
                }
                if (file != null)
                {
                    String FileName = System.IO.Path.GetFileName(file.FileName);
                    String FileExtension = System.IO.Path.GetExtension(file.FileName);
                    String FileNamePath = Server.MapPath("~/Files_Registered/");
                    model.Register.FILE = FileName;
                    model.Register.FILEEXT = FileExtension;
                    model.Register.FILEPATH = FileNamePath;
                }
                else
                {
                    model.ErrorList.Add("file", string.Format(message3, "Resume"));
                }

                model.Register.SKILL = BaseMng.GetRequestFormParam(Request, "skill").Split(',').ToList();



                if (pic != null)
                {
                    string picname = Path.GetFileName(pic.FileName);
                    string picpath = Path.Combine(Server.MapPath("~/Files_Registered/Pic_Registered/"), picname);
                    pic.SaveAs(picpath);
                    System.Drawing.Image sourceimage = System.Drawing.Image.FromStream(pic.InputStream);

                }

                if (file != null)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string filepath = Path.Combine(Server.MapPath("~/Files_Registered/"), filename);
                    file.SaveAs(filepath);
                }

                Session[CommonConst.SessionName.PUB_C_FORM] = model.Register;

                model.Register.RedirectPage = "/newpage/Register/";


                if (string.IsNullOrEmpty(model.Register.FULL_NAME))
                {
                    model.ErrorList.Add("name", string.Format(message1, " 企業名(名前)"));
                }

                //Eメール
                if (string.IsNullOrEmpty(model.Register.EMAIL))
                {
                    model.ErrorList.Add("email", string.Format(message1, " Eメール"));
                }

                //件名
                if (string.IsNullOrEmpty(model.Register.CONTACT_NUMBER))
                {
                    model.ErrorList.Add("number", string.Format(message1, " 件名"));
                }

                //メッセージ
                if (string.IsNullOrEmpty(model.Register.ADDRESS))
                {
                    model.ErrorList.Add("address", string.Format(message1, " メッセージ"));
                }

                //Date of birth
                if (string.IsNullOrEmpty(model.Register.DATE_OF_BIRTH))
                {
                    model.ErrorList.Add("date_of_birth", string.Format(message3, " Date_of_birth)"));
                }
                //Gender

                if (model.Register.GENDER != "1" && model.Register.GENDER != "2")
                {
                    model.ErrorList.Add("txtGender", string.Format(message3, " Gender"));
                }
                //yourself
                if (string.IsNullOrEmpty(model.Register.YOURSELF))
                {
                    model.ErrorList.Add("yourself", string.Format(message3, " Yourself"));
                }
                //Skills
                foreach (String item in model.Register.SKILL)
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        model.ErrorList.Add("txtSkills", string.Format(message3, " skills"));
                    }
                    else if (item != "1" && item != "2" && item != "3" && item != "4")
                    {
                        model.ErrorList.Add("txtSkill", string.Format(message3, " skill"));
                    }
                }


                if (model.ErrorList.Count >= 1)
                {
                    TempData[CommonConst.TempDataName.PUB_C_ERROR_MSG] = model.ErrorList;
                    Session[CommonConst.SessionName.PUB_C_FORM] = model.Register;
                    return Redirect(model.Register.RedirectPage);
                }

                return View("submitconfirm", model);

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

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult SubmitComplete()
        {
            Newpage model = new Newpage();

            try
            {

                //model.Register = (Newpage.registerItem)Session[CommonConst.SessionName.PUB_C_FORM];

                //セッションの確認
                if (Session[CommonConst.SessionName.PUB_C_FORM] != null && Session[CommonConst.SessionName.PUB_C_FORM] is Newpage.registerItem)
                {
                    //セッションから情報取得
                    model.Register = (Newpage.registerItem)Session[CommonConst.SessionName.PUB_C_FORM];
                }
                else
                {
                    return Redirect("/Register/");

                }
                ExecuteRegister(model);
                Session.Remove(CommonConst.SessionName.PUB_C_FORM);
                return View("SubmitComplete", model);
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

        public ActionResult ShowRegister()
        {
            Newpage model = new Newpage();
            model.dataItem = GetAllRegister(model);
            model.skillItem = GetAllSkills(model);





            return View(model);
        }

        public ActionResult EditRegister(string id)
        {
            Newpage model = new Newpage();
            model.Register.SKILL = new List<string>();
            model.Register.MODE = CommonConst.Mode.PUB_C_UPDATE;
            if (string.IsNullOrEmpty(id))
            {
                return Redirect(model.Register.RedirectPage);
            }
            model.Register.C_ID = id;
            DataTable temp = GetAllRegister(model);
            DataRow tmpRow = temp.Rows[0];
            model.Register.FULL_NAME = tmpRow["FULL_NAME"].ToString();
            model.Register.EMAIL = tmpRow["EMAIL"].ToString();
            model.Register.CONTACT_NUMBER = tmpRow["CONTACT_NUMBER"].ToString();
            model.Register.ADDRESS = tmpRow["ADDRESS"].ToString();
            model.Register.DATE_OF_BIRTH = tmpRow["DATE_OF_BIRTH"].ToString();
            model.Register.GENDER = tmpRow["GENDER"].ToString();
            model.Register.YOURSELF = tmpRow["YOURSELF"].ToString();
            model.Register.C_ID = tmpRow["C_ID"].ToString();

            DataTable temps = GetAllSkills(model);


            foreach (DataRow item in temps.Rows)
            {
                model.Register.SKILL.Add(item["SKILLS"].ToString());
            }
            model.Register.TempData = id;
            //DBdelete(Convert.ToInt32(model.Register.C_ID));
            Session[CommonConst.SessionName.PUB_C_FORM] = model.Register;

            return View("Register", model);
        }


        public ActionResult DeleteRegister(string id)
        {
            bool result = DBdelete(Convert.ToInt32(id));
            if (result)
            {
                return Redirect("/newpage/ShowRegister/");

            }
            else
            {
                return Redirect("/newpage/ShowRegister/");

            }

        }


        /// <summary>
        /// 問い合わせ情報を登録
        /// </summary>
        /// <param name="model">情報格納用モデル</param>
        /// <returns>True:処理成功、False:処理失敗</returns>
        /// 

        private void ExecuteRegister(Newpage model)
        {
            if (CommonConst.Mode.PUB_C_REGIST.Equals(model.Register.MODE))
            {
                int cid = InsertRegister(ref model);
                foreach (var item in model.Register.SKILL)
                {
                    InsertSkill(ref model, cid, item);
                }
            }
            else if (CommonConst.Mode.PUB_C_UPDATE.Equals(model.Register.MODE))
            {
               UpdateRegister(ref model);

                DeleteSkills(Convert.ToInt32(model.Register.C_ID));
                GetAllSkills(model);


                //foreach (var item in model.Register.SKILL)
                //{
                //    InsertSkill(ref model,Convert.ToInt32(model.Register.C_ID), item);
                //}
            }
           
        }

        #region "SQL"
        //INSERT
        private int InsertRegister(ref Newpage model)
        {
            DataBaseUtil con = new DataBaseUtil();
            StringBuilder sqlBuilder;

            try
            {
                //DB接続
                con.Connect(MvcApplication.GLOBAL_DB_CONNECT);

                //SQL生成
                sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine(" INSERT INTO Reg_Table");
                sqlBuilder.AppendLine("     (");
                sqlBuilder.AppendLine("          FULL_NAME");
                sqlBuilder.AppendLine("         ,EMAIL");
                sqlBuilder.AppendLine("         ,CONTACT_NUMBER");
                sqlBuilder.AppendLine("         ,ADDRESS");
                sqlBuilder.AppendLine("         ,DATE_OF_BIRTH");

                sqlBuilder.AppendLine("         ,GENDER");
                sqlBuilder.AppendLine("         ,YOURSELF");
                sqlBuilder.AppendLine("         ,PICTURE_NAME");
                sqlBuilder.AppendLine("         ,PICTURE_EXTENSION");
                sqlBuilder.AppendLine("         ,PICTURE_PATH");
                sqlBuilder.AppendLine("         ,FILE_NAME");
                sqlBuilder.AppendLine("         ,FILE_EXTENSION");
                sqlBuilder.AppendLine("         ,FILE_PATH");
                sqlBuilder.AppendLine("         )");
                sqlBuilder.AppendLine(" VALUES");
                sqlBuilder.AppendLine("     (");
                sqlBuilder.AppendLine("         @FULL_NAME");
                sqlBuilder.AppendLine("         ,@EMAIL");
                sqlBuilder.AppendLine("         ,@CONTACT_NUMBER");
                sqlBuilder.AppendLine("         ,@ADDRESS");
                sqlBuilder.AppendLine("         ,@DATE_OF_BIRTH");

                sqlBuilder.AppendLine("         ,@GENDER");
                sqlBuilder.AppendLine("         ,@YOURSELF");
                sqlBuilder.AppendLine("         ,@PICTURE_NAME");
                sqlBuilder.AppendLine("         ,@PICTURE_EXTENSION");
                sqlBuilder.AppendLine("         ,@PICTURE_PATH");
                sqlBuilder.AppendLine("         ,@FILE_NAME");
                sqlBuilder.AppendLine("         ,@FILE_EXTENSION");
                sqlBuilder.AppendLine("         ,@FILE_PATH");
                sqlBuilder.AppendLine("         )");

                //SQL文設定
                con.Sql = sqlBuilder.ToString();
                con.ParamAppend("FULL_NAME", BaseMng.StringSubstring(model.Register.FULL_NAME, 100));
                con.ParamAppend("EMAIL", BaseMng.StringSubstring(model.Register.EMAIL, 100));
                con.ParamAppend("CONTACT_NUMBER", BaseMng.StringSubstring(model.Register.CONTACT_NUMBER, 200));
                con.ParamAppend("ADDRESS", BaseMng.StringSubstring(model.Register.ADDRESS, 3000));
                con.ParamAppend("DATE_OF_BIRTH", BaseMng.StringSubstring(model.Register.DATE_OF_BIRTH, 100));

                con.ParamAppend("GENDER", BaseMng.StringSubstring(model.Register.GENDER, 100));
                con.ParamAppend("YOURSELF", BaseMng.StringSubstring(model.Register.YOURSELF, 3000));
                con.ParamAppend("PICTURE_NAME", BaseMng.StringSubstring(model.Register.PICTURE, 100));
                con.ParamAppend("PICTURE_EXTENSION", BaseMng.StringSubstring(model.Register.PICTUREEXT, 100));
                con.ParamAppend("PICTURE_PATH", BaseMng.StringSubstring(model.Register.PICTUREPATH, 100));
                con.ParamAppend("FILE_NAME", BaseMng.StringSubstring(model.Register.FILE, 100));
                con.ParamAppend("FILE_EXTENSION", BaseMng.StringSubstring(model.Register.FILEEXT, 100));
                con.ParamAppend("FILE_PATH", BaseMng.StringSubstring(model.Register.FILEPATH, 100));




                //SQL発行
                con.Execute(MEB_LOG_INFO);
                int id = con.LastInsertID();
                return id;
            }
            //catch (Exception ex)
            //{
            //    ErrorMng.ErrorOutPut(MvcApplication.GLOBAL_SYSTEM_SETTING, ex);
            //    Response.StatusCode = 500;
            //    return false;

            //}

            finally
            {
                //DB切断
                con.DisConnect();
                con.Dispose();
            }
        }

        private bool InsertSkill(ref Newpage model, int id, string HID)
        {
            DataBaseUtil con = new DataBaseUtil();
            StringBuilder sqlBuilder;

            try
            {
                //DB接続
                con.Connect(MvcApplication.GLOBAL_DB_CONNECT);

                //SQL生成
                sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine(" INSERT INTO Reg_Skill");
                sqlBuilder.AppendLine("     (");
                sqlBuilder.AppendLine("          C_ID");
                sqlBuilder.AppendLine("         ,SKILLS");

                sqlBuilder.AppendLine("         )");
                sqlBuilder.AppendLine(" VALUES");
                sqlBuilder.AppendLine("     (");
                sqlBuilder.AppendLine("          @C_ID");
                sqlBuilder.AppendLine("         ,@SKILLS");

                sqlBuilder.AppendLine("         )");


                //SQL文設定  
                con.Sql = sqlBuilder.ToString();
                con.ParamAppend("C_ID", id);
                con.ParamAppend("SKILLS", HID);
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


        //SELECT
        private DataTable GetAllRegister(Newpage model)
        {
            DataBaseUtil con = new DataBaseUtil();
            StringBuilder sqlBuilder;
            DataTable data = new DataTable();

            try
            {
                //DB接続
                con.Connect(MvcApplication.GLOBAL_DB_CONNECT);

                //SQL生成
                sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine(" SELECT * FROM Reg_Table");
                if (!string.IsNullOrEmpty(model.Register.C_ID))
                {
                    sqlBuilder.AppendLine(" Where C_ID=@C_ID");
                }
                con.Sql = sqlBuilder.ToString();
                con.ParamAppend("C_ID", model.Register.C_ID);
                data = con.Execute(MEB_LOG_INFO, "").Tables[0];
                return data;
            }
            finally
            {
                //DB切断
                con.DisConnect();
                con.Dispose();
            }
        }

        private DataTable GetAllSkills(Newpage model)
        {
            DataBaseUtil con = new DataBaseUtil();
            StringBuilder sqlBuilder;
            DataTable data = new DataTable();

            try
            {
                //DB接続
                con.Connect(MvcApplication.GLOBAL_DB_CONNECT);

                //SQL生成
                sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine(" SELECT * FROM Reg_Skill");
                if (!string.IsNullOrEmpty(model.Register.C_ID))
                {
                    sqlBuilder.AppendLine(" Where C_ID = @C_ID");
                }
                con.Sql = sqlBuilder.ToString();
                con.ParamAppend("C_ID", model.Register.C_ID);
                data = con.Execute(MEB_LOG_INFO, "").Tables[0];
                return data;
            }
            finally
            {
                //DB切断
                con.DisConnect();
                con.Dispose();
            }
            #endregion
        }

        //UPDATE
        private bool UpdateRegister(ref Newpage model)
        {
            DataBaseUtil con = new DataBaseUtil();
            StringBuilder sqlBuilder;

            try
            {
                //DB接続
                con.Connect(MvcApplication.GLOBAL_DB_CONNECT);

                //SQL生成
                sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine(" UPDATE Reg_Table");
                sqlBuilder.AppendLine("     SET");
                sqlBuilder.AppendLine("          FULL_NAME = @FULL_NAME");
                sqlBuilder.AppendLine("         ,EMAIL = @EMAIL");
                sqlBuilder.AppendLine("         ,CONTACT_NUMBER = @CONTACT_NUMBER");
                sqlBuilder.AppendLine("         ,ADDRESS = @ADDRESS");
                sqlBuilder.AppendLine("         ,DATE_OF_BIRTH = @DATE_OF_BIRTH");
                sqlBuilder.AppendLine("         ,YOURSELF = @YOURSELF");

                sqlBuilder.AppendLine("     WHERE C_ID = @ID");


                //SQL文設定
                con.Sql = sqlBuilder.ToString();

                //パラメータ設定
                con.ParamAppend("FULL_NAME", model.Register.FULL_NAME);
                con.ParamAppend("EMAIL", BaseMng.StringSubstring(model.Register.EMAIL, 200));
                con.ParamAppend("CONTACT_NUMBER", model.Register.CONTACT_NUMBER);
                con.ParamAppend("ADDRESS", model.Register.ADDRESS);
                con.ParamAppend("DATE_OF_BIRTH", model.Register.DATE_OF_BIRTH);
                con.ParamAppend("YOURSELF", model.Register.YOURSELF);
                con.ParamAppend("ID", model.Register.C_ID);
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
        


        

        //DELETE
        private bool DBdelete(int id)
        {
            DataBaseUtil con = new DataBaseUtil();
            StringBuilder sqlBuilder;

            try
            {
                //DB接続
                con.Connect(MvcApplication.GLOBAL_DB_CONNECT);

                //SQL生成
                sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine(" DELETE FROM Reg_Table WHERE C_ID= @ID");

                //SQL文設定
                con.Sql = sqlBuilder.ToString();

                //パラメータ設定
                //con.ParamCreate();
                con.ParamAppend("ID", id);
                //SQL発行
                con.Execute(MEB_LOG_INFO);

                DeleteSkills(id);
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

        private bool DeleteSkills(int ID)
        {
            DataBaseUtil con = new DataBaseUtil();
            StringBuilder sqlBuilder;

            try
            {
                //DB接続
                con.Connect(MvcApplication.GLOBAL_DB_CONNECT);

                //SQL生成
                sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine(" DELETE FROM Reg_Skill WHERE C_ID = @ID");

                //SQL文設定
                con.Sql = sqlBuilder.ToString();

                //パラメータ設定
                //con.ParamCreate();
                con.ParamAppend("ID", ID);
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
    }
}





