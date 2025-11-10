using System.Windows.Forms;
using LxpAPI;

namespace LxpGUI {
    interface ITab {
        public void Load(TabPage page, LxpClient client);
    }
}