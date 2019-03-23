using System;
using System.Windows.Forms;

namespace BrawlLib.LoopSelection
{
    public class CustomTrackBar : TrackBar
    {
        private bool _isScrolling = false;

        public event EventHandler UserSeek;

        new public int Value
        {
            get { return base.Value; }
            set { if (!_isScrolling) base.Value = value; }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (_isScrolling || (e.Button != MouseButtons.Left))
                return;

            int x = 12, w = Width - 15;
            int y = 4, h = 20;

            if ((e.X < x) || (e.X > w) || (e.Y < y) || (e.Y > h))
                return;

            float scale = ((float)e.X - x) / (w - x);
            int pos = (int)(Maximum * scale);

            _isScrolling = true;

            if ((base.Value > (pos - 10)) && (base.Value < (pos + 10)))
                return;

            base.Value = pos;

            //Send MouseDown message so we can capture the slider.
            Message msg = new Message();
            msg.HWnd = this.Handle;
            msg.Msg = 0x201;
            msg.WParam = (IntPtr)1;
            msg.LParam = (IntPtr)((e.Y & 0xFFFF) << 16 | (e.X & 0xFFFF));
            base.WndProc(ref msg);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (_isScrolling)
            {
                _isScrolling = false;
                if (UserSeek != null)
                    UserSeek(this, null);
            }
        }
    }
}
