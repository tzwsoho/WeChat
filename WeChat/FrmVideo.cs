using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

using QuartzTypeLib;

namespace WeChat
{
    public partial class FrmVideo : Form
    {
        //private const int WM_APP = 0x8000;
        //private const int WM_GRAPHNOTIFY = WM_APP + 1;

        string m_file = null;
        public string FileName
        {
            get { return m_file; }
            set { m_file = value; }
        }

        FilgraphManager m_fm = null;
        public FilgraphManager FM
        {
            get { return m_fm; }
            set { m_fm = value; }
        }

        private int m_state = 0;
        private int m_org_width = 0;
        private int m_org_height = 0;

        public FrmVideo()
        {
            InitializeComponent();
        }

        private void Stop()
        {
            if (null == m_fm) return;

            try
            {
                IMediaControl mc = m_fm as IMediaControl;
                mc.Stop();

                //int lEventCode, lParam1, lParam2;
                //IMediaEventEx mee = m_fm as IMediaEventEx;
                //mee.SetNotifyWindow(0, 0, 0);
                //mee.GetEvent(out lEventCode, out lParam1, out lParam2, 0);
                //mee.FreeEventParams(lEventCode, lParam1, lParam2);

                IVideoWindow vw = m_fm as IVideoWindow;
                vw.Visible = 0;
                vw.Owner = 0;
            }
            catch (Exception)
            {
            }
        }

        private void FrmVideo_Shown(object sender, EventArgs e)
        {
            if (null == m_fm)
            {
                this.Close();
                return;
            }

            try
            {
                IBasicVideo bv = m_fm as IBasicVideo;
                m_org_width = bv.SourceWidth;
                m_org_height = bv.SourceHeight;
                int h = bv.SourceHeight;
                this.ClientSize = new Size(bv.SourceWidth, h < ts.Height ? 1 : h - ts.Height);

                this.SetBounds(
                    (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                    (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2,
                    0, 0, BoundsSpecified.Location);

                const int WS_CHILD = 0x40000000;
                const int WS_CLIPCHILDREN = 0x2000000;

                IVideoWindow vw = m_fm as IVideoWindow;
                vw.Owner = pnVideo.Handle.ToInt32();
                vw.WindowStyle = WS_CHILD | WS_CLIPCHILDREN;
                vw.SetWindowPosition(
                    pnVideo.Left,
                    pnVideo.Top,
                    pnVideo.Width,
                    pnVideo.Height);

                //IMediaEventEx mee = m_fm as IMediaEventEx;
                //mee.SetNotifyWindow(this.Handle.ToInt32(), WM_GRAPHNOTIFY, 0);

                IMediaControl mc = m_fm as IMediaControl;
                mc.Run();
            }
            catch (Exception)
            {
            }
        }

        private void FrmVideo_Resize(object sender, EventArgs e)
        {
            if (null == m_fm) return;

            try
            {
                IVideoWindow vw = m_fm as IVideoWindow;
                vw.SetWindowPosition(
                    this.ClientRectangle.Left,
                    this.ClientRectangle.Top,
                    this.ClientRectangle.Width,
                    this.ClientRectangle.Height);
            }
            catch (Exception)
            {
            }
        }

        private void FrmVideo_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (null == m_fm) return;

            try
            {
                IMediaControl mc = m_fm as IMediaControl;
                m_state = 0;
                mc.Run();

                btnPlay.Enabled = false;
                btnPause.Enabled = true;
            }
            catch (Exception)
            {
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (null == m_fm) return;

            try
            {
                IMediaControl mc = m_fm as IMediaControl;
                m_state = 1;
                mc.Pause();

                btnPlay.Enabled = true;
                btnPause.Enabled = false;
            }
            catch (Exception)
            {
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (null == m_fm) return;

            try
            {
                IMediaControl mc = m_fm as IMediaControl;
                m_state = 2;
                mc.Stop();

                IMediaPosition mp = m_fm as IMediaPosition;
                mp.CurrentPosition = 0;

                btnPlay.Enabled = true;
                btnPause.Enabled = false;
            }
            catch (Exception)
            {
            }
        }

        private void btnSize_Click(object sender, EventArgs e)
        {
            if (null == m_fm) return;

            btnHalf.Checked = false;
            btnOriginal.Checked = false;
            btnTwice.Checked = false;
            (sender as ToolStripButton).Checked = true;

            try
            {
                IBasicVideo bv = m_fm as IBasicVideo;
                if (sender == btnHalf)
                {
                    int h = bv.SourceHeight / 2;
                    this.ClientSize = new Size(bv.SourceWidth / 2, h < ts.Height ? 1 : h - ts.Height);
                }
                else if (sender == btnOriginal)
                {
                    int h = bv.SourceHeight;
                    this.ClientSize = new Size(bv.SourceWidth, h < ts.Height ? 1 : h - ts.Height);
                }
                else if (sender == btnTwice)
                {
                    int h = bv.SourceHeight * 2;
                    this.ClientSize = new Size(bv.SourceWidth * 2, h < ts.Height ? 1 : h - ts.Height);
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK != sfd.ShowDialog())
            {
                return;
            }

            File.Copy(m_file, sfd.FileName);
            MessageBox.Show("已成功保存到：" + sfd.FileName);
        }
    }
}
