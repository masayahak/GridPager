using System;
using System.Windows.Forms;

namespace GridPager
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool テスト = false;
            if(テスト)
            {
                Application.Run(new Formテスト());
            }
            else
            {
                Application.Run(new Form売上一覧());
            }
        }
    }
}
