using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace CompanySite.Models
{
    public class Vacancy : Home
    {
        public Dictionary<string, string> ErrorList = new Dictionary<string, string>();
        public DataSet WorkDataSet = null;
        public DataSet TmpDataSet = null;
        public StreamReader StreamReader = null;

        public VacancyItem Item = new VacancyItem();
        public DataTable dataItem = new DataTable();
        public DataTable skillItem = new DataTable();
        public List<string> soleSkills = new List<string>();
        public DataTable soleVacancy = new DataTable();
        public Home.ContactItem contact = new ContactItem();
        private bool disposedValue;

        public struct VacancyItem
        {
            /// <summary>
            /// 名前
            /// </summary>
            public string NAME;
            /// <summary>
            /// Eメール
            /// </summary>
            public string EMAIL;
            /// <summary>
            /// MOBILE
            /// </summary>
            public string MOBILE;
            /// <summary>
            /// About yourself
            /// </summary>
            public string ABOUTYOURSELF;
            /// <summary>
            /// Gender
            /// </summary>
            public string GENDER;
            /// <summary>
            /// sport of choice
            /// </summary>
            public string AGE;
            /// <summary>
            /// interested
            /// </summary>
            public List<string> SKILLS;
            /// <summary>
            /// keyId
            /// </summary>
            public int KEYID;
            /// <summary>
            /// IMAGE
            /// </summary>
            public string PHOTO;
            /// <summary>
            /// File
            /// </summary>
            public string CV;
            /// <summary>
            /// IMAGENAME
            /// </summary>
            public string PHOTONAME;
            /// <summary>
            /// FileNAME
            /// </summary>
            public string CVNAME;
            /// <summary>
            /// メッセージ
            /// </summary>
            public string MESSAGE;
            /// <summary>
            /// Redirectページ
            /// </summary>
            public string RedirectPage;
            /// <summary>
            /// チェック完了
            /// </summary>
            public bool CheckComplete;
            /// <summary>
            /// チェック完了
            /// </summary>
            public string tempData;


        }

        #region Dispose Finalize パターン

        /// <summary>
        /// 既にDisposeメソッドが呼び出されているかどうかを表します。
        /// </summary>
        private bool disposed;

        /// <summary>
        /// ConsoleApplication1.DisposableClass1 によって使用されているすべてのリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        /// <summary>
        /// ConsoleApplication1.DisposableClass1 クラスのインスタンスがGCに回収される時に呼び出されます。
        /// </summary>
        ~Vacancy()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// ConsoleApplication1.DisposableClass1 によって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースとアンマネージ リソースの両方を解放する場合は true。アンマネージ リソースだけを解放する場合は false。 </param>
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

        /// <summary>
        /// 既にDisposeメソッドが呼び出されている場合、例外をスローします。
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">既にDisposeメソッドが呼び出されています。</exception>
        protected void ThrowExceptionIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        /// <summary>
        /// Dispose Finalize パターンに必要な初期化処理を行います。
        /// </summary>
        private void InitializeDisposeFinalizePattern()
        {
            this.disposed = false;
        }

        #endregion
    }
}