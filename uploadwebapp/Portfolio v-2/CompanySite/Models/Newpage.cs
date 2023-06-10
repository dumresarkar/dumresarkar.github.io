using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;  
using System.Web;
using System.IO;

namespace CompanySite.Models
{
    public class Newpage
    {
        public DataSet WorkDataSet = null;
        public DataSet TmpDataSet = null;
        public StreamReader StreamReader = null;
        public Dictionary<string, string> ErrorList = new Dictionary<string, string>();

        public int CompanyCount;

         public registerItem Register = new registerItem();
        //public List<registerItem> RegisterDetails= new List<registerItem>();
        public DataTable dataItem = new DataTable();
        public DataTable skillItem = new DataTable();
        //public struct DataTable
        //{
        //    public string SKILLS;
        //}

        private bool disposed;

        //問い合わせ項目
        [Serializable]
        public struct registerItem
        {
            
            /// <summary>
            /// 名前
            /// </summary>
            public string FULL_NAME;
            /// <summary>
            /// Eメール
            /// </summary>
            public string EMAIL;
            /// <summary>
            /// 件名
            /// </summary>
            public string CONTACT_NUMBER;
            /// <summary>
            /// メッセージ
            /// </summary>
            public string ADDRESS;

            public string DATE_OF_BIRTH;

            public string GENDER;

            public string YOURSELF;

            public string PICTURE;
            public string PICTUREEXT;
            public string PICTUREPATH;

            public string FILE;
            public string FILEEXT;
            public string FILEPATH;
            public string MODE;

            
            public List<string> SKILL;

            /// <summary>
            /// チェック完了
            /// </summary>
            public bool CheckComplete;
            /// <summary>
            /// Redirectページ
            /// </summary>
            public string RedirectPage;
            public string TempData;
            public string tempData;

            public string C_ID;
        }
       
        public void Dispose()
        {
            GC.SuppressFinalize(this);

            this.Dispose(true);
        }


        protected virtual void Dispose(bool disposing = true)
        {
            if (this.disposed)
            {
                return;
            }
            this.disposed = true;

            if (disposing)
            {
                // マネージ リソースの解放処理をこの位置に記述します。
                if (WorkDataSet != null)
                {
                    WorkDataSet.Dispose();
                }
                if (TmpDataSet != null)
                {
                    TmpDataSet.Dispose();
                }
            }
            // アンマネージ リソースの解放処理をこの位置に記述します。
            if (StreamReader != null)
            {
                StreamReader.Dispose();
            }
        }

    }
}