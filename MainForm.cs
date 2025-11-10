using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using LxpAPI;
using LxpGUI.Tabs;

namespace LxpGUI {
    partial class MainForm : Form {
        TabControl tabControl = new();
        LxpClient client = null;

        Dictionary<string, ITab> tabs = new(){
            {"Расписание", new ScheduleTab() },
            {"Уведомления", new NotificationsTab() },
        };

        public MainForm(LxpClient clnt){
            client = clnt;
            CreateLayout();
        }

        private void CreateLayout(){
            SuspendLayout();

            ClientSize = new Size(600, 600);
            Text = "LXP emae";

            tabControl.Location = new Point(0, 0);
            tabControl.Dock = DockStyle.Fill;

            foreach(var tab in tabs){
                var page = new TabPage();
                page.Text = tab.Key;

                tab.Value.Load(page, client);
                tabControl.Controls.Add(page);
            }

            Controls.Add(tabControl);
            ResumeLayout();
        }
    }
}