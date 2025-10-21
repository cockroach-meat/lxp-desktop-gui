using System;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;

namespace LxpGUI {
    class Program {
        public static void Main(){
            try{
                ConfigHelper.ReadConfig();
            }catch(Exception ex){
                MessageBox.Show("Ошибка", "Не удалось прочитать файл конфигурации: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }
    }
}