using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

using LxpAPI;
using LxpAPI.DataClasses;

namespace LxpGUI.Tabs {
    class ScheduleTab : ITab {
        TabPage parent;
        LxpClient client = null;

        TableLayoutPanel tlpMain = new();
        TableLayoutPanel tlpHeader = new();
        FlowLayoutPanel flpHeader = new();
        TableLayoutPanel tlpClasses = new();
        Button btnBack = new();
        Label lblCurrent = new();
        Button btnNext = new();
        DateTime dateTime = DateTime.Now;

        public void Load(TabPage page, LxpClient clnt){
            parent = page;
            client = clnt;

            Task.Run(LoadSchedule);

            tlpMain.Dock = DockStyle.Fill;
            tlpMain.RowCount = 4;
            tlpMain.SetRowSpan(tlpClasses, 3);

            flpHeader.AutoSize = true;
            flpHeader.WrapContents = false;
            flpHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpHeader.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;

            tlpClasses.Dock = DockStyle.Fill;
            tlpClasses.AutoScroll = true;

            btnBack.Text = "<<";
            btnBack.AutoSize = true;
            btnBack.Margin = new Padding(10);
            btnBack.Click += (s, e)=>{
                dateTime = dateTime.AddDays(-1);
                LoadSchedule();
            };

            lblCurrent.AutoSize = true;
            lblCurrent.Margin = new Padding(10);
            lblCurrent.TextAlign = ContentAlignment.MiddleCenter;

            btnNext.Text = ">>";
            btnNext.AutoSize = true;
            btnNext.Margin = new Padding(10);
            btnNext.Click += (s, e)=>{
                dateTime = dateTime.AddDays(1);
                LoadSchedule();
            };

            tlpClasses.RowCount = 6;

            flpHeader.Controls.AddRange(new Control[]{ btnBack, lblCurrent, btnNext });

            tlpMain.Controls.Add(flpHeader);
            tlpMain.Controls.Add(tlpClasses);

            parent.Controls.Add(tlpMain);
        }

        void LoadSchedule(){
            tlpClasses.Controls.Clear();
            lblCurrent.Text = dateTime.ToShortDateString();

            Task.Run(()=>{
                var classes = client.GetSchedule(dateTime, new DateTime(dateTime.Ticks).AddDays(1));

                if(classes.Count == 0){
                    Label lbl = new();
                    lbl.Text = "Нет занятий";
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    lbl.Dock = DockStyle.Fill;

                    if(tlpClasses.InvokeRequired){
                        tlpClasses.Invoke(()=>{
                            tlpClasses.Controls.Add(lbl);
                        });
                    }

                    return;
                }

                foreach(var @class in classes){
                    TableLayoutPanel panel = new();
                    panel.RowCount = 3;
                    panel.ColumnCount = 1;
                    panel.Padding = new Padding(10);
                    panel.BackColor = Color.FromArgb(255, 121, 222, 247);
                    panel.Dock = DockStyle.Fill;

                    Label lessonName = new();
                    lessonName.AutoSize = true;
                    lessonName.Text = @class.discipline.name;
                    lessonName.Font = new Font(lessonName.Font, FontStyle.Bold);

                    Label timeAndPlace = new();
                    timeAndPlace.AutoSize = true;
                    timeAndPlace.Text = $"{@class.classroom.name}, {DateTime.Parse(@class.from).ToShortTimeString()}-{DateTime.Parse(@class.to).ToShortTimeString()}";

                    Label teacher = new();
                    teacher.AutoSize = true;
                    teacher.Text = $"{@class.teachers[0].user.lastName} {@class.teachers[0].user.firstName} {@class.teachers[0].user.middleName}";

                    var clickHandler = new EventHandler((s, e)=>{
                        OpenLxpLink(@class);
                    });

                    var controls = new Control[]{ lessonName, timeAndPlace, teacher };
                    panel.Controls.AddRange(controls);
                    panel.Click += clickHandler;
                    foreach(var c in controls) c.Click += clickHandler;

                    if(tlpClasses.InvokeRequired){
                        tlpClasses.Invoke(()=>{
                            tlpClasses.Controls.Add(panel);
                        });
                    }
                }
            });
        }

        void OpenLxpLink(Class @class){
            ProcessStartInfo startInfo = new();
            startInfo.FileName = $"https://newlxp.ru/education/{@class.suborganizationId}/disciplines/{@class.discipline.id}";
            startInfo.UseShellExecute = true;
            Process.Start(startInfo);
        }
    }
}