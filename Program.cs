using System;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;

using LxpAPI;

namespace LxpGUI {
    class Program {
        public static void Main(){
            try{
                ConfigHelper.ReadConfig();
            }catch(Exception ex){
                MessageBox.Show("Ошибка", "Не удалось прочитать файл конфигурации: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;

            var client = new LxpClient(ConfigHelper.Config.email, ConfigHelper.Config.password);
            LoginData loginData = null;

            if(!client.LogIn()){
                do {
                    loginData = new LoginForm().GetLoginData();
                    if(!loginData.enteredData) Environment.Exit(1);

                    client = new LxpClient(loginData.email, loginData.password);
                } while(!client.LogIn());
            }

            if(loginData != null){
                ConfigHelper.Config.email = loginData.email;
                ConfigHelper.Config.password = loginData.password;
                ConfigHelper.WriteConfig();
            }

            Application.EnableVisualStyles();
            Application.Run(new MainForm(client));
        }
    }
}