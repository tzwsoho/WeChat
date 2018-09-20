using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WeChat
{
    public partial class FrmQRCode : Form
    {
        private int m_tip = 1;
        private string m_uuid = "";
        private WebRequestor m_requestor = null;
        private FrmMain m_frm_main = null;

        #region 公共方法

        private void OnTimeout()
        {
            ReloadQRCode();
        }

        private void OnScanned(Image avatar)
        {
            if (null != picQRCode.BackgroundImage)
            {
                picQRCode.BackgroundImage.Dispose();
            }

            picQRCode.BackgroundImage = avatar;
            picQRCode.BackgroundImageLayout = ImageLayout.Center;

            m_tip = 0;
            CheckLoginStatus();
        }

        private void OnLoginOK(string jump_url)
        {
            Invoke(new Action<string>(m_frm_main.OnLogin), jump_url);
        }

        private void OnLoginStatusGot(HttpWebRequest req, HttpWebResponse res)
        {
            StreamReader sr = new StreamReader(res.GetResponseStream());
            string str_ret = sr.ReadToEnd();
            Regex re_code = new Regex(@"(?<=window\.code\s*\=\s*)\d+", RegexOptions.IgnoreCase);
            string code = re_code.Match(str_ret).Value;
            sr.Close();

            try
            {
                switch (code)
                {
                    case "201": // 扫描后未确定登录
                        Regex re_avatar = new Regex(@"(?<=window\.userAvatar\s*\=\s*[\""|'][^,]+,)[^\""']+(?=[\""|'])", RegexOptions.IgnoreCase);
                        string avatar = re_avatar.Match(str_ret).Value;

                        Image img_avatar = Image.FromStream(new MemoryStream(Convert.FromBase64String(avatar)));
                        Invoke(new Action<Image>(OnScanned), img_avatar);
                        break;

                    case "200": // 已确认登录
                        Regex re_uri = new Regex(@"(?<=window\.redirect_uri\s*\=\s*[\""|'])[^\""]+(?=[\""|'])", RegexOptions.IgnoreCase);
                        string jump_url = re_uri.Match(str_ret).Value;

                        Invoke(new Action<string>(OnLoginOK), jump_url);
                        break;

                    case "408": // 扫描超时，重新获取二维码
                    default:
                        ReloadQRCode();
                        break;
                }
            }
            catch (Exception)
            {

            }
        }

        private void CheckLoginStatus()
        {
            string url = "https://login.weixin.qq.com/cgi-bin/mmwebwx-bin/login?loginicon=true" +
                "&uuid=" + WebUtility.UrlEncode(m_uuid) +
                "&tip=" + m_tip +
                "&r=" + Common.UnixtimeStamp();

            m_requestor.DoGetRequest(url: url, OnTimeout: OnTimeout, OnResponse: OnLoginStatusGot);
        }

        private void OnQRCodeGot(HttpWebRequest req, HttpWebResponse res)
        {
            Image img_qr_code = Image.FromStream(res.GetResponseStream());
            //Invoke(new Action<Image>(OnGotQRCode), img_qr_code);
            if (null != picQRCode.BackgroundImage)
            {
                picQRCode.BackgroundImage.Dispose();
            }

            picQRCode.BackgroundImage = img_qr_code;
            picQRCode.BackgroundImageLayout = ImageLayout.Zoom;

            m_tip = 1;
            CheckLoginStatus();
        }

        private void GetQRCode()
        {
            string url = "https://login.weixin.qq.com/qrcode/" + m_uuid + "?t=webwx";
            m_requestor.DoGetRequest(url: url, OnTimeout: OnTimeout, OnResponse: OnQRCodeGot);
        }

        private void OnQRCodeReloaded(HttpWebRequest req, HttpWebResponse res)
        {
            StreamReader sr = new StreamReader(res.GetResponseStream());
            Regex re_uuid = new Regex(@"(?<=window\.QRLogin\.uuid\s*\=\s*[\""|']).+(?=[\""|'])", RegexOptions.IgnoreCase);
            m_uuid = re_uuid.Match(sr.ReadToEnd()).Value;
            sr.Close();

            GetQRCode();
        }

        public void ReloadQRCode()
        {
            string url = "https://wx.qq.com/";
            m_requestor.DoGetRequest(url: url, OnTimeout: OnTimeout);

            url = "https://login.weixin.qq.com/jslogin?appid=wx782c26e4c19acffb&fun=new&lang=zh_CN&_=" + Common.UnixtimeStamp();
            m_requestor.DoGetRequest(url: url, OnTimeout: OnTimeout, OnResponse: OnQRCodeReloaded);
        }

        public void BindForm(FrmMain frm)
        {
            m_frm_main = frm;
        }

        #endregion

        #region 界面

        public FrmQRCode()
        {
            InitializeComponent();

            m_requestor = new WebRequestor(this);
        }

        protected override void WndProc(ref Message m)
        {
            const int HTCLIENT = 1;
            const int HTCAPTION = 2;
            const int WM_NCHITTEST = 0x84;

            if (m.Msg == WM_NCHITTEST)
            {
                this.DefWndProc(ref m);

                if (m.Result.ToInt32() == HTCLIENT)
                {
                    m.Result = new IntPtr(HTCAPTION);
                    return;
                }
            }

            base.WndProc(ref m);
        }

        private void FrmQRCode_Shown(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        #endregion
    }
}
