using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace CompanySite.Models
{
    /// <summary>
    /// トップモデル。
    /// </summary>
    public class Home : IDisposable
    {
        //共通項目
        public DataSet WorkDataSet = null;
        public DataSet TmpDataSet = null;
        public StreamReader StreamReader = null;
        public Dictionary<string, string> ErrorList = new Dictionary<string, string>();

        public int CompanyCount;

        /// <summary>問い合わせ情報の構造体格納用オブジェクト</summary>
        public ContactItem Contact = new ContactItem();

        //問い合わせ項目
        [Serializable]
        public struct ContactItem
        {
            /// <summary>
            /// 店舗ID
            /// </summary>
            public string SHOP_ID;
            /// <summary>
            /// 名前
            /// </summary>
            public string NAME;
            /// <summary>
            /// Eメール
            /// </summary>
            public string EMAIL;
            /// <summary>
            /// 件名
            /// </summary>
            public string SUBJECT;
            /// <summary>
            /// メッセージ
            /// </summary>
            public string MESSAGE;
           

            public string ADDRESS;

            public List<string> HOBBIES;
            //public String HOBBIES;

            public string GENDER;

            public string BACKSTORY;

            public string COURSE;

            public string FILE;
            public string FILEEXT;
            public string FILEPATH;

            public string PICTURE;
            public string PICTUREEXT;
            public string PICTUREPATH;
           
            /// <summary>
            /// チェック完了
            /// </summary>
            public bool CheckComplete;
            /// <summary>
            /// Redirectページ
            /// </summary>
            public string RedirectPage;



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
        ~Home()
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