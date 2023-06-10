using CompanySite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APP_COMMON;
using System.Text;
using System.IO;
using System.Data;

namespace CompanySite.Controllers
{
    #region メンバ変数

    #endregion

    #region アクション
    public class VacancyController : BaseController
    {
        #region メンバ変数

        #endregion

        #region アクション
        
        public ActionResult Index()
        {
            Vacancy model = new Vacancy();
            if (TempData[CommonConst.TempDataName.PUB_C_ERROR_MSG] != null)
            {
                model.ErrorList = (Dictionary<string, string>)TempData[CommonConst.TempDataName.PUB_C_ERROR_MSG];
            }
            if (Session[CommonConst.SessionName.PUB_C_FORM] != null && Session[CommonConst.SessionName.PUB_C_FORM] is Vacancy.VacancyItem)
            {
                model.Item = (Vacancy.VacancyItem)Session[CommonConst.SessionName.PUB_C_FORM];
            }
            model.dataItem = GetAllVacancy();
            model.skillItem = GetAllSkills();
            return View("~/Views/Vacancy/Index.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VacancyConfirm(HttpPostedFileBase image, HttpPostedFileBase file)
        {
            Vacancy model = new Vacancy();
            string message1 = "{0}を入力してください。";
            string message2 = "{0}の入力値が不正です。";

            try
            {
                
                //画面の値取得
                model.Item.NAME = BaseMng.GetRequestFormParam(Request, "name");
                model.Item.EMAIL = BaseMng.GetRequestFormParam(Request, "email");
                model.Item.MOBILE = BaseMng.GetRequestFormParam(Request, "mobile");
                model.Item.AGE = BaseMng.GetRequestFormParam(Request, "age");
                model.Item.ABOUTYOURSELF = BaseMng.GetRequestFormParam(Request, "aboutYourself");
                model.Item.GENDER = BaseMng.GetRequestFormParam(Request, "gender");

                if (image != null)
                {
                    String imageExtension = System.IO.Path.GetExtension(image.FileName);
                    String imageName = System.IO.Path.GetFileName(image.FileName);
                    model.Item.PHOTO = imageExtension;
                    model.Item.PHOTONAME = imageName;
                    string pathImage = Path.Combine(Server.MapPath("~/Content/img/vacancy"), Path.GetFileName(image.FileName));
                    image.SaveAs(pathImage);
                }
                else
                {
                    model.ErrorList.Add("txtPhoto", string.Format(message1, "企業名(名前)"));
                }

                if (file != null)
                {
                    String fileExtension = System.IO.Path.GetExtension(file.FileName);
                    String fileName = System.IO.Path.GetFileName(file.FileName);
                    model.Item.CV = fileExtension;
                    model.Item.CVNAME = fileName;
                    string pathFile = Path.Combine(Server.MapPath("~/Content/img/vacancy"), Path.GetFileName(file.FileName));
                    file.SaveAs(pathFile);
                }
                else
                {
                    model.ErrorList.Add("txtCV", string.Format(message1, "企業名(名前)"));
                }

                //checkbox
                model.Item.SKILLS = BaseMng.GetRequestFormParam(Request, "skill").Split(',').ToList();

                model.Item.RedirectPage = "/vacancy/";

                //企業名(名前)
                if (string.IsNullOrEmpty(model.Item.NAME))
                {
                    model.ErrorList.Add("txtName", string.Format(message1, "企業名(名前)"));
                }
                else if (model.Item.NAME.Length > 20)
                {
                    model.ErrorList.Add("txtName", string.Format(message1, "enter upto 20 characters only!"));
                }

                //Eメール
                if (string.IsNullOrEmpty(model.Item.EMAIL))
                {
                    model.ErrorList.Add("txtEmail", string.Format(message1, "Eメール"));
                }

                if (string.IsNullOrEmpty(model.Item.MOBILE))
                {
                    model.ErrorList.Add("txtMobile", string.Format(message1, "Eメール"));
                }

                //件名
                if (string.IsNullOrEmpty(model.Item.AGE))
                {
                    model.ErrorList.Add("txtAge", string.Format(message1, "Age"));
                }

                //aboutYourself
                if (string.IsNullOrEmpty(model.Item.ABOUTYOURSELF))
                {
                    model.ErrorList.Add("txtAboutYourself", string.Format(message1, "about yourself"));
                }


                //Skills
                foreach (String item in model.Item.SKILLS)
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        model.ErrorList.Add("txtSkills", string.Format(message1, "skills"));
                    }
                    else if (item != "1" && item != "2" && item != "3" && item != "4" && item != "5")
                    {
                        model.ErrorList.Add("txtSkill", string.Format(message1, "skill"));
                    }
                }

                //gender
                if (model.Item.GENDER != "1" && model.Item.GENDER != "2")
                {
                    model.ErrorList.Add("txtGender", string.Format(message1, "gender"));
                }

                if (model.ErrorList.Count >= 1)
                {
                    TempData[CommonConst.TempDataName.PUB_C_ERROR_MSG] = model.ErrorList;
                    Session[CommonConst.SessionName.PUB_C_FORM] = model.Item;
                    return Redirect("/vacancy/");
                }

                model.Item.CheckComplete = true;
                Session[CommonConst.SessionName.PUB_C_FORM] = model.Item;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VacancyComplete()
        {
            Vacancy model = new Vacancy();
            try
            {
                //セッションの確認
                if (Session[CommonConst.SessionName.PUB_C_FORM] != null && Session[CommonConst.SessionName.PUB_C_FORM] is Vacancy.VacancyItem)
                {
                    //セッションから情報取得
                    model.Item = (Vacancy.VacancyItem)Session[CommonConst.SessionName.PUB_C_FORM];
                    if (!model.Item.CheckComplete)
                    {
                        return Redirect(model.Item.RedirectPage);
                    }
                    Session.Remove(CommonConst.SessionName.PUB_C_FORM);
                }
                else
                {
                    return Redirect("/vacancy/");
                }

                //メール送信

                //DB登録がエラーでもエラーページに遷移しません。メール送信完了しているため
                try
                {
                    //dbに登録する
                    if(model.Item.tempData == "update")
                    {
                        UpdateVacancyDB(ref model);
                    }
                    else
                    {
                    InsertVacancy(ref model);
                    }
                }
                catch (Exception ex)
                {
                    //エラーログ出力
                    ErrorMng.ErrorOutPut(MvcApplication.GLOBAL_SYSTEM_SETTING, ex);
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

        public ActionResult DeleteVacancy(int id)
        {
            bool result = DeleteVacancyDB(id);
            if (result)
            {
                return Redirect("/vacancy/");

            }
            else
            {
                return Redirect("/vacancy/");

            }
        }

        public ActionResult ViewVacancy(int id)
        {
            Vacancy model = new Vacancy();
            model.soleVacancy = GetSingleVacancy(id);
            model.skillItem = GetSkillsById(id);
            foreach(DataRow item in model.skillItem.Rows)
            {
                model.soleSkills.Add(item["VALUE"].ToString());
            }
            return View(model);

        } 
        
        public ActionResult EditVacancy(int id)
        {
            Vacancy model = new Vacancy();
            model.soleVacancy = GetSingleVacancy(id);
            model.skillItem = GetSkillsById(id);
            foreach(DataRow item in model.skillItem.Rows)
            {
                model.soleSkills.Add(item["VALUE"].ToString());
            }
            return View(model);

        } 


       

        #endregion

        #region "メソッド"

        #region "クラスモジュール"

        private void SaveImage(ref Vacancy model)
        {
            string test = "~/Content/img/vacancy/" + model.Item.KEYID;
            if (!Directory.Exists(test))
            {
                Directory.CreateDirectory(Server.MapPath(test));
            }
            if (model.Item.CVNAME != null)
            {
                System.IO.File.Move("D:/PROJECT/CompanySite1 orginal/CompanySite/Content/img/vacancy/" + model.Item.CVNAME, "D:/PROJECT/CompanySite1 orginal/CompanySite/Content/img/vacancy/" + model.Item.KEYID + "/" + model.Item.CVNAME);
            }
            if (model.Item.PHOTONAME != null)
            {
                System.IO.File.Move("D:/PROJECT/CompanySite1 orginal/CompanySite/Content/img/vacancy/" + model.Item.PHOTONAME, "D:/PROJECT/CompanySite1 orginal/CompanySite/Content/img/vacancy/" + model.Item.KEYID + "/" + model.Item.PHOTONAME);
            }
        }

        #endregion

        #region "SQL"

        #region "SELECT"

        private DataTable GetAllSkills()
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
                sqlBuilder.AppendLine(" SELECT * FROM T_VACANCY INNER JOIN T_SKILLS ON T_VACANCY.VACANCY_KEY_ID = T_SKILLS.VACANCY_ID");
                con.Sql = sqlBuilder.ToString();
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

        private DataTable GetAllVacancy()
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
                sqlBuilder.AppendLine(" SELECT * FROM T_VACANCY");
                con.Sql = sqlBuilder.ToString();
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

        private DataTable GetSingleVacancy(int id)
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
                sqlBuilder.AppendLine(" SELECT * FROM T_VACANCY WHERE VACANCY_KEY_ID = @VACANCY_ID");
                con.Sql = sqlBuilder.ToString();

                con.ParamAppend("VACANCY_ID", id);

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

        private DataTable GetSkillsById(int id)
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
                sqlBuilder.AppendLine(" SELECT * FROM T_SKILLS WHERE VACANCY_ID = @VACANCY_ID");
                con.Sql = sqlBuilder.ToString();

                con.ParamAppend("VACANCY_ID", id);

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

        #endregion

        #region "INSERT"

        private bool InsertVacancy(ref Vacancy model)
        {
            DataBaseUtil con = new DataBaseUtil();
            StringBuilder sqlBuilder;

            try
            {
                //DB接続
                con.Connect(MvcApplication.GLOBAL_DB_CONNECT);

                //SQL生成
                sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine(" INSERT INTO T_VACANCY");
                sqlBuilder.AppendLine("     (");
                sqlBuilder.AppendLine("          NAME");
                sqlBuilder.AppendLine("         ,EMAIL");
                sqlBuilder.AppendLine("         ,MOBILE");
                sqlBuilder.AppendLine("         ,ABOUT_YOURSELF");
                sqlBuilder.AppendLine("         ,GENDER");
                sqlBuilder.AppendLine("         ,AGE");
                sqlBuilder.AppendLine("     )");
                sqlBuilder.AppendLine(" VALUES");
                sqlBuilder.AppendLine("     (");
                sqlBuilder.AppendLine("          @NAME");
                sqlBuilder.AppendLine("         ,@EMAIL");
                sqlBuilder.AppendLine("         ,@MOBILE");
                sqlBuilder.AppendLine("         ,@ABOUT_YOURSELF");
                sqlBuilder.AppendLine("         ,@GENDER");
                sqlBuilder.AppendLine("         ,@AGE");

                sqlBuilder.AppendLine("     )");

                //SQL文設定
                con.Sql = sqlBuilder.ToString();

                //パラメータ設定
                con.ParamAppend("NAME", model.Item.NAME);
                con.ParamAppend("EMAIL", BaseMng.StringSubstring(model.Item.EMAIL, 200));
                con.ParamAppend("MOBILE", model.Item.MOBILE);
                con.ParamAppend("ABOUT_YOURSELF", model.Item.ABOUTYOURSELF);
                con.ParamAppend("GENDER", model.Item.GENDER);
                con.ParamAppend("AGE", model.Item.AGE);
                //SQL発行
                con.Execute(MEB_LOG_INFO);

                int keyId = con.LastInsertID();
                model.Item.KEYID = keyId;
                InsertSkills(ref model);
                InsertImage(ref model);
                InsertFile(ref model);
                SaveImage(ref model);


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

        private bool InsertSkills(ref Vacancy model)
        {
            DataBaseUtil con = new DataBaseUtil();
            StringBuilder sqlBuilder;

            try
            {
                //DB接続
                con.Connect(MvcApplication.GLOBAL_DB_CONNECT);

                //SQL生成
                sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine(" INSERT INTO T_SKILLS");
                sqlBuilder.AppendLine("     (");
                sqlBuilder.AppendLine("          VACANCY_ID");
                sqlBuilder.AppendLine("          ,VALUE");

                sqlBuilder.AppendLine("     )");
                sqlBuilder.AppendLine(" VALUES");
                sqlBuilder.AppendLine("     (");
                sqlBuilder.AppendLine("          @VACANCY_ID");
                sqlBuilder.AppendLine("          ,@VALUE");


                sqlBuilder.AppendLine("     )");

                //SQL文設定
                con.Sql = sqlBuilder.ToString();

                //パラメータ設定
                //con.ParamCreate();

                foreach (String item in model.Item.SKILLS)
                {
                    con.Sql = sqlBuilder.ToString();
                    con.ParamAppend("VACANCY_ID", model.Item.KEYID);
                    con.ParamAppend("VALUE", item);

                    //SQL発行
                    con.Execute(MEB_LOG_INFO);
                }
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

        private bool InsertImage(ref Vacancy model)
        {
            DataBaseUtil con = new DataBaseUtil();
            StringBuilder sqlBuilder;

            try
            {
                //DB接続
                con.Connect(MvcApplication.GLOBAL_DB_CONNECT);

                //SQL生成
                sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine(" INSERT INTO T_PHOTO");
                sqlBuilder.AppendLine("     (");
                sqlBuilder.AppendLine("          VACANCY_ID");
                sqlBuilder.AppendLine("          ,PHOTO_NAME");

                sqlBuilder.AppendLine("     )");
                sqlBuilder.AppendLine(" VALUES");
                sqlBuilder.AppendLine("     (");
                sqlBuilder.AppendLine("          @VACANCY_ID");
                sqlBuilder.AppendLine("          ,@PHOTO_NAME");


                sqlBuilder.AppendLine("     )");

                //SQL文設定
                con.Sql = sqlBuilder.ToString();

                //パラメータ設定
                //con.ParamCreate();
                string imageName = model.Item.KEYID + model.Item.PHOTO;

                con.ParamAppend("VACANCY_ID", model.Item.KEYID);
                con.ParamAppend("PHOTO_NAME", imageName);

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

        private bool InsertFile(ref Vacancy model)
        {
            DataBaseUtil con = new DataBaseUtil();
            StringBuilder sqlBuilder;

            try
            {
                //DB接続
                con.Connect(MvcApplication.GLOBAL_DB_CONNECT);

                //SQL生成
                sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine(" INSERT INTO T_VFILE");
                sqlBuilder.AppendLine("     (");
                sqlBuilder.AppendLine("          VACANCY_ID");
                sqlBuilder.AppendLine("          ,FILE_NAME");

                sqlBuilder.AppendLine("     )");
                sqlBuilder.AppendLine(" VALUES");
                sqlBuilder.AppendLine("     (");
                sqlBuilder.AppendLine("          @VACANCY_ID");
                sqlBuilder.AppendLine("          ,@FILE_NAME");


                sqlBuilder.AppendLine("     )");

                //SQL文設定
                con.Sql = sqlBuilder.ToString();

                //パラメータ設定
                //con.ParamCreate();
                string fileName = model.Item.KEYID + model.Item.CV;

                con.ParamAppend("VACANCY_ID", model.Item.KEYID);
                con.ParamAppend("FILE_NAME", fileName);

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

        private bool UpdateVacancyDB(ref Vacancy model)
        {
            DataBaseUtil con = new DataBaseUtil();
            StringBuilder sqlBuilder;

            try
            {
                //DB接続
                con.Connect(MvcApplication.GLOBAL_DB_CONNECT);

                //SQL生成
                sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine(" UPDATE T_VACANCY");
                sqlBuilder.AppendLine("     SET");
                sqlBuilder.AppendLine("          NAME = @NAME");
                sqlBuilder.AppendLine("         ,EMAIL = @EMAIL");
                sqlBuilder.AppendLine("         ,MOBILE = @MOBILE");
                sqlBuilder.AppendLine("         ,ABOUT_YOURSELF = @ABOUT_YOURSELF");
                sqlBuilder.AppendLine("         ,GENDER = @GENDER");
                sqlBuilder.AppendLine("         ,AGE = @AGE");
                sqlBuilder.AppendLine("     WHERE VACANCY_KEY_ID = @ID");


                //SQL文設定
                con.Sql = sqlBuilder.ToString();

                //パラメータ設定
                con.ParamAppend("NAME", model.Item.NAME);
                con.ParamAppend("EMAIL", BaseMng.StringSubstring(model.Item.EMAIL, 200));
                con.ParamAppend("MOBILE", model.Item.MOBILE);
                con.ParamAppend("ABOUT_YOURSELF", model.Item.ABOUTYOURSELF);
                con.ParamAppend("GENDER", model.Item.GENDER);
                con.ParamAppend("AGE", model.Item.AGE);
                con.ParamAppend("ID", model.Item.KEYID);
                //SQL発行
                con.Execute(MEB_LOG_INFO);

                UpdateSkills(ref model);
                //InsertImage(ref model);
                //InsertFile(ref model);
                //SaveImage(ref model);


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

        private bool UpdateSkills(ref Vacancy model)
        {
            DataBaseUtil con = new DataBaseUtil();
            StringBuilder sqlBuilder;

            try
            {
                //DB接続
                con.Connect(MvcApplication.GLOBAL_DB_CONNECT);

                //SQL生成
                //sqlBuilder = new StringBuilder();
                //sqlBuilder.AppendLine(" UPDATE T_SKILLS");
                //sqlBuilder.AppendLine("     SET");
                //sqlBuilder.AppendLine("          VACANCY_ID = @VACANCY_ID");
                //sqlBuilder.AppendLine("         ,VALUE = @VALUE");
                //sqlBuilder.AppendLine("     WHERE VACANCY_ID = @ID");

                ////SQL文設定
                //con.Sql = sqlBuilder.ToString();

                ////パラメータ設定
                ////con.ParamCreate();

                //foreach (String item in model.Item.SKILLS)
                //{
                //    con.Sql = sqlBuilder.ToString();
                //    con.ParamAppend("VACANCY_ID", model.Item.KEYID);
                //    con.ParamAppend("VALUE", item);

                //    //SQL発行
                //    con.Execute(MEB_LOG_INFO);
                //}
                DeleteSkills(Convert.ToInt32(model.Item.KEYID));
                InsertSkills(ref model);

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

        #region "DELETE"

        private bool DeleteVacancyDB(int id)
        {
            DataBaseUtil con = new DataBaseUtil();
            StringBuilder sqlBuilder;

            try
            {
                //DB接続
                con.Connect(MvcApplication.GLOBAL_DB_CONNECT);

                //SQL生成
                sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine(" DELETE FROM T_VACANCY WHERE VACANCY_KEY_ID = @ID");

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
                sqlBuilder.AppendLine(" DELETE FROM T_SKILLS WHERE VACANCY_ID = @ID");

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

        #endregion

        #endregion

        #endregion
        #endregion

    }


}