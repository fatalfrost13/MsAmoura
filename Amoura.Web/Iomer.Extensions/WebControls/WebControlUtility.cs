using System.Web.UI.WebControls;

namespace Iomer.Extensions.WebControls
{
    public static class WebControlUtility
    {
        public static void SelectedIndex(this ListBox lbx, string key)
        {
            lbx.SelectedIndex = lbx.Items.IndexOf(lbx.Items.FindByValue(key));
        }
        public static void SelectedIndex(this DropDownList ddl, string key)
        {
            ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(key));
        }
        public static void AddConfirm(this Button cmd, string msg)
        {
            const string FormatString = "if (confirm('{0}')) return true; else return false;";
            cmd.OnClientClick = string.Format(FormatString, msg);
        }

        public static void AddConfirm(this LinkButton cmd, string msg)
        {
            const string FormatString = "if (confirm('{0}')) return true; else return false;";
            cmd.OnClientClick = string.Format(FormatString, msg);
        }

        public static void AddConfirm(this ImageButton cmd, string msg)
        {
            const string FormatString = "if (confirm('{0}')) return true; else return false;";
            cmd.OnClientClick = string.Format(FormatString, msg);
        }
    }
}
