using System;
using System.Drawing;
using System.Windows.Forms;

namespace LxpGUI {
    class WebView : Form {
        WebBrowser webBrowser = new();
        string html = string.Empty;
        int width = 0;
        int height = 0;

        public WebView(string title, string data, int _width, int _height){
            Load += OnLoad;
            Shown += OnShown;

            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Text = title;

            html = data;
            width = _width;
            height = _height;

            webBrowser.Dock = DockStyle.Fill;
            webBrowser.AllowNavigation = false;
            webBrowser.AllowWebBrowserDrop = false;
            webBrowser.WebBrowserShortcutsEnabled = false;
            webBrowser.IsWebBrowserContextMenuEnabled = false;

            Controls.Add(webBrowser);
        }

        void OnLoad(object sender, EventArgs ea){
            Size = new Size(width, height);
            Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - Size.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - Size.Height / 2);
        }

        void OnShown(object sender, EventArgs ea){
            webBrowser.DocumentText = html;
        }
    }
}