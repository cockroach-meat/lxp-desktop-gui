using System;
using System.Drawing;
using System.Windows.Forms;

namespace LxpGUI {
    class LoginForm : Form {
        string _email = string.Empty;
        string _password = string.Empty;
        bool _enteredData = false;

        TableLayoutPanel tlpMain = new();
        Label lblEmail = new();
        Label lblPassword = new();
        TextBox tbxEmail = new();
        TextBox tbxPassword = new();
        Button btnLogin = new();

        public LoginForm(){
            CreateLayout();
        }

        void CreateLayout(){
            ClientSize = new Size(400, 150);
            Text = "Вход в LXP";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            
            tlpMain.RowCount = 3;
            tlpMain.ColumnCount = 3;
            tlpMain.Dock = DockStyle.Fill;

            lblEmail.AutoSize = true;
            lblEmail.Dock = DockStyle.Fill;
            lblEmail.Text = "E-mail";
            tlpMain.SetCellPosition(lblEmail, new TableLayoutPanelCellPosition(0, 0));

            lblPassword.AutoSize = true;
            lblPassword.Dock = DockStyle.Fill;
            lblPassword.Text = "Пароль";
            tlpMain.SetCellPosition(lblPassword, new TableLayoutPanelCellPosition(0, 1));

            tbxEmail.AutoSize = true;
            tbxEmail.Dock = DockStyle.Fill;
            tlpMain.SetCellPosition(tbxEmail, new TableLayoutPanelCellPosition(1, 0));
            tlpMain.SetColumnSpan(tbxEmail, 2);

            tbxPassword.AutoSize = true;
            tbxPassword.Dock = DockStyle.Fill;
            tbxPassword.PasswordChar = '*';
            tlpMain.SetCellPosition(tbxPassword, new TableLayoutPanelCellPosition(1, 1));
            tlpMain.SetColumnSpan(tbxPassword, 2);

            btnLogin.AutoSize = true;
            btnLogin.Text = "Войти";
            btnLogin.Anchor = AnchorStyles.Top;
            tlpMain.SetCellPosition(btnLogin, new TableLayoutPanelCellPosition(0, 2));
            tlpMain.SetColumnSpan(btnLogin, 3);

            btnLogin.Click += (s, e)=>{
                _email = tbxEmail.Text;
                _password = tbxPassword.Text;
                _enteredData = true;
                Close();
            };

            tlpMain.Controls.AddRange(new Control[]{ lblEmail, lblPassword, tbxEmail, tbxPassword, btnLogin });
            Controls.Add(tlpMain);
        }

        public LoginData GetLoginData(){
            ShowDialog();
            return new LoginData(){ email = _email, password = _password, enteredData = _enteredData };
        }
    }

    class LoginData {
        public string email = string.Empty;
        public string password = string.Empty;
        public bool enteredData = false;
    }
}