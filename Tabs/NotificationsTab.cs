using System;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using LxpAPI;
using LxpAPI.DataClasses;

namespace LxpGUI.Tabs {
    class NotificationsTab : ITab {
        TabPage parent;
        LxpClient client = null;

        TableLayoutPanel tlpMain = new();
        TableLayoutPanel tlpHeader = new();
        FlowLayoutPanel flpHeader = new();
        Panel pNotifications = new();
        TableLayoutPanel tlpNotifications = new();
        Button btnUpdate = new();

        public void Load(TabPage page, LxpClient clnt){
            parent = page;
            client = clnt;

            Task.Run(LoadNotifications);

            tlpMain.Dock = DockStyle.Fill;
            tlpMain.RowCount = 4;
            tlpMain.SetRowSpan(tlpNotifications, 3);

            flpHeader.AutoSize = true;
            flpHeader.WrapContents = false;
            flpHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpHeader.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;

            tlpNotifications.Dock = DockStyle.Fill;

            pNotifications.AutoScroll = true;
            pNotifications.Dock = DockStyle.Fill;

            btnUpdate.AutoSize = true;
            btnUpdate.Text = "Обновить";
            btnUpdate.Click += (s, e)=>{
                LoadNotifications();
            };

            flpHeader.Controls.Add(btnUpdate);

            pNotifications.Controls.Add(tlpNotifications);

            tlpMain.Controls.Add(flpHeader);
            tlpMain.Controls.Add(pNotifications);

            parent.Controls.Add(tlpMain);
        }

        void LoadNotifications(){
            tlpNotifications.Controls.Clear();

            Task.Run(()=>{
                var notifications = client.GetNotifications();

                if(notifications.Count == 0){
                    Label lbl = new();
                    lbl.Text = "Нет уведомлений";
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    lbl.Dock = DockStyle.Fill;

                    if(tlpNotifications.InvokeRequired){
                        tlpNotifications.Invoke(()=>{
                            tlpNotifications.Controls.Add(lbl);
                        });
                    }

                    return;
                }

                foreach(var notification in notifications){
                    TableLayoutPanel panel = new();
                    panel.RowCount = 3;
                    panel.ColumnCount = 1;
                    panel.BackColor = Color.FromArgb(255, 121, 222, 247);
                    panel.Dock = DockStyle.Fill;
                    panel.Padding = new Padding(10);
                    panel.Margin = new Padding(SystemInformation.VerticalScrollBarWidth, 0, SystemInformation.VerticalScrollBarWidth, 0);

                    Label name = new();
                    name.AutoSize = true;
                    name.Text = notification.title;
                    name.Font = new Font(name.Font, FontStyle.Bold);
                    name.Dock = DockStyle.Fill;

                    var bodyText = Regex.Replace(notification.body, @"<.+?>", "");
                    if(bodyText.Length > 70) bodyText = bodyText.Substring(0, 70) + "...";

                    Label body = new();
                    body.Text = bodyText;
                    body.Dock = DockStyle.Fill;
                    panel.SetRowSpan(body, 2);

                    var clickHandler = new EventHandler((s, e)=>{
                        var thread = new Thread(()=>{
                            var webView = new WebView(notification.title, notification.body, 700, 500);
                            webView.ShowDialog();
                        });

                        thread.SetApartmentState(ApartmentState.STA);
                        thread.Start();
                    });

                    foreach(var c in new Control[]{ name, body, panel }) c.Click += clickHandler;

                    panel.Controls.AddRange(new Control[]{ name, body });

                    if(tlpNotifications.InvokeRequired){
                        tlpNotifications.Invoke(()=>{
                            tlpNotifications.Controls.Add(panel);
                        });
                    }
                }
            });
        }
    }
}