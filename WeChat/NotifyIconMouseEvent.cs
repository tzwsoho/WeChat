using System;
using System.Drawing;
using System.Windows.Forms;

namespace WeChat
{
    public class NotifyIconMouseEvent
    {
        #region 定义

        public delegate void OnMouseEnter(Point ptInScreen, object param);
        public delegate void OnMouseLeave(Point ptInScreen, object param);

        #endregion

        #region 属性

        private Form m_frmSplash = null;
        private NotifyIcon m_niTray = null;
        private Point m_ptMouse = Point.Empty;
        private Timer m_tmrMouseTracer = null;
        private object m_oEnterParam = null;
        private OnMouseEnter m_OnMouseEnter = null;
        private object m_oLeaveParam = null;
        private OnMouseLeave m_OnMouseLeave = null;

        #endregion

        #region 方法

        public NotifyIconMouseEvent(NotifyIcon ni, Form frm)
        {
            m_frmSplash = frm;

            m_niTray = ni;
            ni.MouseMove += new MouseEventHandler(ni_MouseMove);

            m_tmrMouseTracer = new Timer();
            m_tmrMouseTracer.Interval = 100;
            m_tmrMouseTracer.Tick += new EventHandler(m_tmrMouseTracer_Tick);
        }

        public NotifyIconMouseEvent(NotifyIcon ni, OnMouseEnter enter, OnMouseLeave leave)
        {
            m_OnMouseEnter = enter;
            m_OnMouseLeave = leave;

            m_niTray = ni;
            ni.MouseMove += new MouseEventHandler(ni_MouseMove);

            m_tmrMouseTracer = new Timer();
            m_tmrMouseTracer.Interval = 100;
            m_tmrMouseTracer.Tick += new EventHandler(m_tmrMouseTracer_Tick);
        }

        public NotifyIconMouseEvent(NotifyIcon ni, OnMouseEnter enter, OnMouseLeave leave, object enterParam, object leaveParam)
            : this(ni, enter, leave)
        {
            m_oEnterParam = enterParam;
            m_oLeaveParam = leaveParam;
        }

        public void BindForm(Form frm)
        {
            m_frmSplash = frm;
        }

        void ni_MouseMove(object sender, MouseEventArgs e)
        {
            Point pt = Control.MousePosition;
            if (m_ptMouse.IsEmpty)
            {
                if (null == m_frmSplash)
                {
                    m_OnMouseEnter(pt, m_oEnterParam);
                }
                else
                {
                    Rectangle rDesktop = Screen.PrimaryScreen.WorkingArea;
                    if (pt.X + m_frmSplash.Width / 2 >= rDesktop.Right)
                    {
                        m_frmSplash.Location = new Point(
                            rDesktop.Right - m_frmSplash.Width,
                            rDesktop.Bottom - m_frmSplash.Height);
                    }
                    else
                    {
                        m_frmSplash.Location = new Point(
                            pt.X - m_frmSplash.Width / 2,
                            rDesktop.Bottom - m_frmSplash.Height);
                    }

                    m_frmSplash.BringToFront();
                    m_frmSplash.Show();
                }
            }

            m_ptMouse = pt;
            m_tmrMouseTracer.Start();
        }

        void m_tmrMouseTracer_Tick(object sender, EventArgs e)
        {
            Point pt = Control.MousePosition;
            if (m_ptMouse != pt)
            {
                if (null == m_frmSplash)
                {
                    m_OnMouseLeave(pt, m_oLeaveParam);
                }
                else
                {
                    if (m_frmSplash.Bounds.Contains(pt))
                    {
                        m_ptMouse = pt;
                        return;
                    }

                    m_frmSplash.Hide();
                }

                m_ptMouse = Point.Empty;
                m_tmrMouseTracer.Stop();
            }
        }

        #endregion
    }
}
