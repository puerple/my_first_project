using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace S7DataExporter
{
    public class ReWinformLayoutHerlper
    {
        public static void setTag(Control cons)//设置控件的Tag属性
        {
            foreach (Control con in cons.Controls)//循环窗体中的控件
            {
                // 特殊处理状态标签
                //if (con == lblLastUpdate)
                //{
                //    con.Tag = $"200:{con.Height}:{con.Left}:{con.Top}:{con.Font.Size}:fixedwidth";
                //    continue;
                //}
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    setTag(con);
            }
        }
        public static void ReWinformLayout(Rectangle control, float x, float y, Control cn)
        {
            var newx = control.Width / x;
            var newy = control.Height / y;
            if (newx == 0) { newx = 0.01f; }
            if (newy == 0) { newy = 0.01f; }
            setControls(newx, newy, cn);
        }
        public static void setControls(float newx, float newy, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            // 只处理可见控件
            foreach (Control con in cons.Controls)
            {
                if (!con.Visible || con.Tag == null) continue;
                // 特殊处理固定宽度控件
                if (con.Tag?.ToString().Contains("fixedwidth") == true)
                {
                    var parts = con.Tag.ToString().Split(':');
                    con.Width = int.Parse(parts[0]); // 保持固定宽度
                    continue;
                }
                // 使用StringSplitOptions提高性能
                string[] mytag = con.Tag.ToString().Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (mytag.Length < 5) continue;

                // 一次性计算所有值
                int newWidth = (int)(Convert.ToSingle(mytag[0]) * newx);
                int newHeight = (int)(Convert.ToSingle(mytag[1]) * newy);
                int newLeft = (int)(Convert.ToSingle(mytag[2]) * newx);
                int newTop = (int)(Convert.ToSingle(mytag[3]) * newy);
                float newFontSize = Convert.ToSingle(mytag[4]) * Math.Min(newx, newy); // 使用最小比例防止字体过大

                // 只有值变化时才更新
                if (con.Width != newWidth) con.Width = newWidth;
                if (con.Height != newHeight) con.Height = newHeight;
                if (con.Left != newLeft) con.Left = newLeft;
                if (con.Top != newTop) con.Top = newTop;

                // 字体变化时才更新
                if (Math.Abs(con.Font.Size - newFontSize) > 0.1f)
                {
                    con.Font = new Font(con.Font.Name, newFontSize, con.Font.Style, con.Font.Unit);
                }

                if (con.Controls.Count > 0)
                {
                    setControls(newx, newy, con);
                }
            }

        }
    }
}
