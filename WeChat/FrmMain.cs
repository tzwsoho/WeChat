using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Media;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Windows.Forms;

using QuartzTypeLib;

namespace WeChat
{
    public partial class FrmMain : Form
    {
        #region 微信参数

        private CookieContainer m_cookies = new CookieContainer();
        public CookieContainer Cookies
        {
            get { return m_cookies; }
            set { m_cookies = value; }
        }

        private string m_skey = "";
        public string skey
        {
            get { return m_skey; }
            set { m_skey = value; }
        }

        private string m_wxsid = "";
        public string wxsid
        {
            get { return m_wxsid; }
            set { m_wxsid = value; }
        }

        private long m_wxuin = 0;
        public long wxuin
        {
            get { return m_wxuin; }
            set { m_wxuin = value; }
        }

        private string m_device_id = "";
        public string device_id
        {
            get { return m_device_id; }
            set { m_device_id = value; }
        }

        private string m_pass_ticket = "";
        public string pass_ticket
        {
            get { return m_pass_ticket; }
            set { m_pass_ticket = value; }
        }

        #endregion

        #region 公共方法

        const uint SND_ASYNC = 0x0001;
        const uint SND_FILENAME = 0x00020000;

        [DllImport("winmm.dll", EntryPoint = "mciSendStringA")]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, IntPtr hwndCallback);

        private bool m_sync = false;
        private long m_last_sync = 0;

        private string m_wx_host = "wx.qq.com";
        private string m_wx_init_post = "";
        private string m_wx_status_notify_post = "";
        private string m_wx_sync_post = "";
        private string m_wx_batch_contacts_post = "";
        private string m_wx_send_msg_post = "";
        private string m_wx_logout_post = "";

        private WXInit m_wx_init = null;
        private WXSyncKey m_wx_sync_key = null;
        private WXSyncKey m_wx_sync_check_key = null;

        private Dictionary<string, string> m_wx_names = null;
        private Dictionary<string, WXContact> m_wx_contacts = null;
        private Dictionary<string, List<WXMsg>> m_wx_msg = null;
        private Dictionary<string, string> m_wx_voices = null;
        private Dictionary<string, string> m_wx_videos = null;

        /*
         * {"Ret": 0, "ErrMsg": ""} 成功
         * {"Ret": -14, "ErrMsg": ""} ticket 错误
         * {"Ret": 1, "ErrMsg": ""} 传入参数 错误
         * {"Ret": 1100, "ErrMsg": ""} 未登录提示
         * {"Ret": 1101, "ErrMsg": ""}（可能：未检测到登陆？）
         * {"Ret": 1102, "ErrMsg": ""}（可能：cookie值无效？）
        selector:
            0 正常
            2 新的消息
            4 通过时发现
            6 删除时发现，对方通过好友验证
            7 进入/离开聊天界面
        */

        public void AddLog(string log)
        {
            string tm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            txtLog.AppendText(tm + " " + log + "\r\n");
        }

        public void OnLogin(string jump_url)
        {
            miSwitch.Enabled = true;
            miRefreshQRCode.Enabled = false;

            GotoURI(jump_url);
            m_nime.BindForm(this);

            m_frm_qrcode.Close();
            m_frm_qrcode = null;
        }

        private void OnTimeout()
        {
            AddLog("发送请求超时！");

            OnLogout(null, null);
        }

        private void OnLogout(HttpWebRequest req, HttpWebResponse res)
        {
            if (null != res)
            {
                StreamReader sr = new StreamReader(res.GetResponseStream());
                string str_ret = sr.ReadToEnd();
                sr.Close();

                System.Diagnostics.Debug.Print(str_ret);
            }

            AddLog("已成功登出微信！");

            if (null != m_requestor)
            {
                m_requestor.Dispose();
                m_requestor = null;
            }

            m_sync = false;
            miSwitch.Enabled = false;
            miRefreshQRCode.Enabled = true;
            m_requestor = new WebRequestor(this);
            this.Hide();

            m_frm_qrcode = new FrmQRCode();
            m_frm_qrcode.BindForm(this);
            m_frm_qrcode.ReloadQRCode();
            m_frm_qrcode.Show();

            m_nime.BindForm(m_frm_qrcode);
        }

        private void Logout(bool wait_res)
        {
            AddLog("登出微信...");

            string url = "https://" + m_wx_host + "/cgi-bin/mmwebwx-bin/webwxlogout?redirect=0&type=0" +
                "&skey=" + WebUtility.UrlEncode(m_skey);

            if (wait_res)
            {
                m_requestor.DoPostRequest(
                    url: url,
                    post_data: string.Format("sid={0}&uin={1}", m_wxsid, m_wxuin),
                    content_type: "application/x-www-form-urlencoded",
                    OnTimeout: OnTimeout,
                    OnResponse: OnLogout);
            }
            else
            {
                m_requestor.DoPostRequest(
                    url: url,
                    post_data: string.Format("sid={0}&uin={1}", m_wxsid, m_wxuin),
                    content_type: "application/x-www-form-urlencoded");

                if (null != m_requestor)
                {
                    m_requestor.Dispose();
                    m_requestor = null;
                }

                tmrTimeout.Enabled = false;
                this.Close();
            }
        }

        private void OnMsgSent(HttpWebRequest req, HttpWebResponse res)
        {
            StreamReader sr = new StreamReader(res.GetResponseStream());
            string str_ret = sr.ReadToEnd();
            sr.Close();

            System.Diagnostics.Debug.Print(str_ret);
        }

        private void SendTextMsg(string to_user_name, string content)
        {
            string url = "https://" + m_wx_host + "/cgi-bin/mmwebwx-bin/webwxsendmsg" +
                "?sid=" + WebUtility.UrlEncode(m_wxsid) +
                "&skey=" + WebUtility.UrlEncode(m_skey) +
                "&r=" + Common.ReverseTimestamp() +
                "&pass_ticket=" + WebUtility.UrlEncode(m_pass_ticket);

            WXSendMsg sm = new WXSendMsg();
            sm.m_base_request = new WXBaseRequest();
            sm.m_base_request.m_device_id = m_device_id;
            sm.m_base_request.m_skey = m_skey;
            sm.m_base_request.m_uin = m_wxuin;
            sm.m_base_request.m_wxsid = m_wxsid;

            sm.m_msg = new WXMsgContent();
            sm.m_msg.m_client_msg_id = Common.UnixtimeStamp() + "1234";
            sm.m_msg.m_content = content;// "收到红包，请在手机上查看";
            sm.m_msg.m_from_user_name = m_wx_init.m_user.m_user_name;
            sm.m_msg.m_local_id = sm.m_msg.m_client_msg_id;
            sm.m_msg.m_to_user_name = to_user_name;
            sm.m_msg.m_type = 1;

            sm.m_scene = 0;

            m_requestor.DoPostRequest(
                url: url,
                post_data: Common.EncodeJson(sm),
                content_type: "application/json, text/plain, */*",
                OnTimeout: OnTimeout,
                OnResponse: OnMsgSent);
        }

        private void OnVideoGot(HttpWebRequest req, HttpWebResponse res)
        {
            string msg_id = HttpUtility.ParseQueryString(req.RequestUri.Query)["msgid"];
            if (0 == res.ContentLength)
            {
                return;
            }

            BinaryReader br = new BinaryReader(res.GetResponseStream());
            byte[] bytes_s = br.ReadBytes((int)res.ContentLength);
            br.Close();

            // 将 MP4 视频写入临时文件
            string video_file = Path.GetTempFileName();
            FileStream fs = new FileStream(video_file, FileMode.Create, FileAccess.ReadWrite);
            fs.Write(bytes_s, 0, bytes_s.Length);
            fs.Close();

            m_wx_videos[msg_id] = video_file;

            AddLog("已成功获取聊天视频！");
        }

        private void GetVideo(string msg_id)
        {
            AddLog("获取聊天视频...");

            string url = "https://" + m_wx_host + "/cgi-bin/mmwebwx-bin/webwxgetvideo" +
                "?msgid=" + WebUtility.UrlEncode(msg_id) +
                "&skey=" + WebUtility.UrlEncode(m_skey);
            m_requestor.DoGetRequest(
                url: url,
                range: 0,
                content_type: "video/mp4",
                OnTimeout: OnTimeout,
                OnResponse: OnVideoGot);
        }

        private void OnVoiceGot(HttpWebRequest req, HttpWebResponse res)
        {
            string msg_id = HttpUtility.ParseQueryString(req.RequestUri.Query)["msgid"];

            BinaryReader br = new BinaryReader(res.GetResponseStream());
            byte[] bytes_s = br.ReadBytes((int)res.ContentLength);
            br.Close();

            // 将 MP3 音频写入临时文件
            string voice_file = Path.GetTempFileName();
            FileStream fs = new FileStream(voice_file, FileMode.Create, FileAccess.ReadWrite);
            fs.Write(bytes_s, 0, bytes_s.Length);
            fs.Close();

            m_wx_voices[msg_id] = voice_file;

            AddLog("已成功获取聊天语音！");
        }

        private void GetVoice(string msg_id)
        {
            AddLog("获取聊天语音...");

            string url = "https://" + m_wx_host + "/cgi-bin/mmwebwx-bin/webwxgetvoice" +
                "?msgid=" + WebUtility.UrlEncode(msg_id) +
                "&skey=" + WebUtility.UrlEncode(m_skey);
            m_requestor.DoGetRequest(
                url: url,
                content_type: "audio/mp3",
                OnTimeout: OnTimeout,
                OnResponse: OnVoiceGot);
        }

        private void OnImageGot(HttpWebRequest req, HttpWebResponse res)
        {
            Image img = Image.FromStream(res.GetResponseStream());

            Clipboard.Clear();
            Clipboard.SetImage(img);

            rtbContent.Select(rtbContent.TextLength, 0);
            rtbContent.Paste();
            rtbContent.AppendText("\r\n\r\n");

            AddLog("已成功获取聊天图片！");
        }

        private void GetImage(string msg_id)
        {
            AddLog("获取聊天图片...");

            string url = "https://" + m_wx_host + "/cgi-bin/mmwebwx-bin/webwxgetmsgimg" + // 缩略图加参数 type=slave
                "?MsgID=" + WebUtility.UrlEncode(msg_id) +
                "&skey=" + WebUtility.UrlEncode(m_skey);
            m_requestor.DoGetRequest(
                url: url,
                OnTimeout: OnTimeout,
                OnResponse: OnImageGot);
        }

        private void OnHeadImgGot(HttpWebRequest req, HttpWebResponse res)
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(req.Address.Query);
            if (null != nvc["username"])
            {
                imgHeads.Images.Add(nvc["username"], Image.FromStream(res.GetResponseStream()));
            }

            AddLog("已成功获取头像！");
        }

        private void GetHeadImg(string head_img_url)
        {
            AddLog("获取头像...");

            string url = "https://" + m_wx_host + head_img_url;
            m_requestor.DoGetRequest(
                url: url,
                OnTimeout: OnTimeout,
                OnResponse: OnHeadImgGot);
        }

        private string GetSyncKey(WXSyncKey sync_key)
        {
            StringBuilder sb_sync_key = new StringBuilder();
            for (int i = 0; i < sync_key.m_count; i++)
            {
                sb_sync_key.Append(sync_key.m_list[i].m_key);
                sb_sync_key.Append("_");
                sb_sync_key.Append(sync_key.m_list[i].m_val);
                sb_sync_key.Append("|");
            }

            return sb_sync_key.ToString().TrimEnd("|".ToCharArray());
        }

        private void OnSync(HttpWebRequest req, HttpWebResponse res)
        {
            SyncCheck();

            StreamReader sr = new StreamReader(res.GetResponseStream());
            string str_ret = sr.ReadToEnd();
            sr.Close();

            WXSync wxs = (WXSync)Common.DecodeJson(str_ret, typeof(WXSync));
            if (null == wxs ||
                0 != wxs.m_base_response.m_ret)
            {
                MessageBox.Show("同步消息发生错误：" + wxs.m_base_response.m_ret);
                Logout(false);
                return;
            }

            AddLog("已成功获取消息！");

            m_wx_sync_key = wxs.m_sync_key;
            m_wx_sync_check_key = wxs.m_sync_check_key;

            // 解析信息内容
            lvwContacts.BeginUpdate();

            if (wxs.m_add_msg_count > 0)
            {
                for (int i = 0; i < wxs.m_add_msg_count; i++)
                {
                    //string to_user = wxs.m_add_msg_list[i].m_to_user_name;
                    string from_user = wxs.m_add_msg_list[i].m_from_user_name;
                    if (!lvwContacts.Items.ContainsKey(from_user))
                    {
                        continue;
                    }

                    if (from_user == m_wx_init.m_user.m_user_name) // 自己在其他设备发给他人的信息，同步过来
                    {

                    }

                    if (!m_wx_msg.ContainsKey(from_user))
                    {
                        m_wx_msg.Add(from_user, new List<WXMsg>());
                    }

                    WXMsg msg = wxs.m_add_msg_list[i];
                    m_wx_msg[from_user].Add(msg);

                    string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ");
                    string from_nick_name = m_wx_names[from_user];
                    string show_title = "", content = "";
                    if (from_user.StartsWith("@@")) // 群组信息
                    {
                        Regex re_content = new Regex(@"(?<=(@(?:[0-9a-f]{32}|[0-9a-f]{64})):\<br/\>)(.+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        GroupCollection cc = re_content.Match(msg.m_content).Groups;
                        if (3 != cc.Count)
                        {
                            continue;
                        }

                        string member_user_name = cc[1].Value;
                        string member_name = m_wx_names[member_user_name];
                        content = cc[2].Value;
                        show_title = member_name + "(" + from_nick_name + ")" + " 发来";
                    }
                    else
                    {
                        content = msg.m_content;
                        show_title = from_nick_name + " 发来";
                    }

                    switch (msg.m_msg_type)
                    {
                        case 1: // 文本信息
                            rtbContent.Select(rtbContent.TextLength, 1);
                            rtbContent.AppendText(now + show_title + "文本信息：\r\n");
                            rtbContent.AppendText(content + "\r\n\r\n");
                            break;

                        case 3: // 图像
                            GetImage(msg.m_msg_id);

                            rtbContent.Select(rtbContent.TextLength, 1);
                            rtbContent.AppendText(now + show_title + "图片信息：\r\n");

                            System.Diagnostics.Debug.Print(now + show_title);
                            break;

                        case 34: // 语音
                            GetVoice(msg.m_msg_id);

                            TimeSpan ts_voice = new TimeSpan(msg.m_voice_length * TimeSpan.TicksPerMillisecond);
                            string str_voice = "播放语音(" + msg.m_msg_id + ")";
                            rtbContent.Select(rtbContent.TextLength, 1);
                            rtbContent.AppendText(now + show_title + "语音信息：\r\n");
                            rtbContent.AppendText("语音长度：" + ts_voice.ToString(@"mm\:ss\.fff") + "\r\n");
                            rtbContent.AppendText(str_voice + "\r\n\r\n");

                            RichTextBoxLinkCreater.CreateLink(rtbContent, rtbContent.TextLength - str_voice.Length - 2, str_voice.Length);
                            break;

                        case 43: // 视频
                            GetVideo(msg.m_msg_id);

                            TimeSpan ts_video = new TimeSpan(msg.m_play_length * TimeSpan.TicksPerSecond);
                            string str_video = "播放视频(" + msg.m_msg_id + ")";
                            rtbContent.Select(rtbContent.TextLength, 1);
                            rtbContent.AppendText(now + show_title + "视频信息：\r\n");
                            rtbContent.AppendText("视频长度：" + ts_video.ToString(@"hh\:mm\:ss") + "\r\n");
                            rtbContent.AppendText(str_video + "\r\n\r\n");

                            RichTextBoxLinkCreater.CreateLink(rtbContent, rtbContent.TextLength - str_video.Length - 2, str_video.Length);
                            break;

                        case 49: // 超链接
                            string url = WebUtility.UrlDecode(msg.m_url);

                            rtbContent.Select(rtbContent.TextLength, 1);
                            rtbContent.AppendText(now + show_title + "超链接：\r\n");
                            rtbContent.AppendText("标题：" + WebUtility.UrlDecode(msg.m_encry_file_name) + "\r\n");
                            rtbContent.AppendText("地址：" + url + "\r\n\r\n");

                            RichTextBoxLinkCreater.CreateLink(rtbContent, rtbContent.TextLength - url.Length - 2, url.Length);
                            break;

                        case 10000: // 红包！！！
                            if ("收到红包，请在手机上查看" == msg.m_content)
                            {
                                niTray.BalloonTipIcon = ToolTipIcon.Info;
                                niTray.BalloonTipTitle = "￥￥￥收到红包￥￥￥";
                                niTray.BalloonTipText = show_title + "红包！！！";
                                niTray.ShowBalloonTip(10000);
                            }

                            rtbContent.Select(rtbContent.TextLength, 1);
                            rtbContent.AppendText(now + show_title + "系统信息：\r\n");
                            rtbContent.AppendText(content + "\r\n\r\n");
                            break;

                        default:
                            rtbContent.Select(rtbContent.TextLength, 1);
                            rtbContent.AppendText(now + show_title + "未知信息：\r\n");
                            rtbContent.AppendText(Common.EncodeJson(msg) + "\r\n\r\n");
                            break;
                    }

                    m_sound.Play();
                    rtbContent.Select(rtbContent.TextLength, 0);
                }
            }

            if (wxs.m_mod_contact_count > 0)
            {
                GetContacts(0);
            }

            lvwContacts.EndUpdate();
        }

        private void OnSyncTimeout()
        {
            AddLog("获取消息超时！");

            Sync();
        }

        // 获取消息
        private void Sync()
        {
            AddLog("获取消息...");

            string url = "https://" + m_wx_host + "/cgi-bin/mmwebwx-bin/webwxsync?lang=zh_CN" +
                "&sid=" + WebUtility.UrlEncode(m_wxsid) +
                "&skey=" + WebUtility.UrlEncode(m_skey) +
                "&pass_ticket=" + WebUtility.UrlEncode(m_pass_ticket);

            WXSyncReq sc = new WXSyncReq();
            sc.m_base_request = new WXBaseRequest();
            sc.m_base_request.m_device_id = m_device_id;
            sc.m_base_request.m_skey = m_skey;
            sc.m_base_request.m_uin = m_wxuin;
            sc.m_base_request.m_wxsid = m_wxsid;
            sc.m_rr = Common.ReverseTimestamp();
            sc.m_sync_key = m_wx_sync_check_key;

            m_requestor.DoPostRequest(
                url: url,
                post_data: Common.EncodeJson(sc),
                content_type: "application/json; charset=UTF-8",
                OnTimeout: OnSyncTimeout,
                OnResponse: OnSync);
        }

        private void OnSyncChecked(HttpWebRequest req, HttpWebResponse res)
        {
            StreamReader sr = new StreamReader(res.GetResponseStream());
            string str_ret = sr.ReadToEnd();
            sr.Close();

            Regex re_retcode = new Regex(@"(?<=retcode\""?:\""?)\d+(?=\""?)", RegexOptions.IgnoreCase);
            Regex re_selector = new Regex(@"(?<=selector\""?:\""?)\d+(?=\""?)", RegexOptions.IgnoreCase);
            string retcode = re_retcode.Match(str_ret).Value;
            string selector = re_selector.Match(str_ret).Value;

            if ("0" == retcode)
            {
                AddLog("已成功获取轮询消息状态！");

                switch (selector)
                {
                    case "2":
                    case "6":
                        Sync();
                        break;
                }
            }
            else if ("1101" != retcode)
            {
                MessageBox.Show("轮询消息状态发生错误：" + retcode);
                AddLog("轮询消息状态发生错误：" + retcode);
                Logout(false);
            }
        }

        private void OnSyncCheckTimeout()
        {
            AddLog("轮询消息状态超时！");

            Sync();
        }

        // 轮询消息状态
        private void SyncCheck()
        {
            AddLog("轮询消息状态...");

            string url = "https://" + m_wx_host + "/cgi-bin/mmwebwx-bin/synccheck" +
                "?r=" + Common.ReverseTimestamp() +
                "&skey=" + WebUtility.UrlEncode(m_skey) +
                "&sid=" + WebUtility.UrlEncode(m_wxsid) +
                "&uin=" + m_wxuin +
                "&deviceid=" + WebUtility.UrlEncode(m_device_id) +
                "&synckey=" + WebUtility.UrlEncode(GetSyncKey(m_wx_sync_check_key)) +
                "&_=" + Common.ReverseTimestamp();

            m_requestor.DoGetRequest(
                url: url,
                OnTimeout: OnSyncCheckTimeout,
                OnResponse: OnSyncChecked);
        }

        private void OnBatchContactsGot(HttpWebRequest req, HttpWebResponse res)
        {
            if (!m_sync)
            {
                m_sync = true;
                tmrTimeout.Enabled = true;
            }

            StreamReader sr = new StreamReader(res.GetResponseStream());
            string str_ret = sr.ReadToEnd();
            sr.Close();

            WXBatchContactsRes bcr = (WXBatchContactsRes)Common.DecodeJson(str_ret, typeof(WXBatchContactsRes));
            if (null == bcr ||
                0 != bcr.m_base_response.m_ret)
            {
                MessageBox.Show("获取群组成员信息发生错误：" + bcr.m_base_response.m_ret);
                Logout(false);
                return;
            }

            foreach (WXBatchContactListItemRes bclis in bcr.m_contact_list)
            {
                foreach (WXBatchContact bc in bclis.m_member_list)
                {
                    if (null == bc.m_display_name || "" == bc.m_display_name)
                    {
                        if (null == bc.m_remark_name || "" == bc.m_remark_name)
                        {
                            m_wx_names[bc.m_user_name] = bc.m_nick_name;
                        }
                        else
                        {
                            m_wx_names[bc.m_user_name] = bc.m_remark_name;
                        }
                    }
                    else
                    {
                        m_wx_names[bc.m_user_name] = bc.m_display_name;
                    }
                }
            }

            AddLog("已成功获取群组列表人列表！");
        }

        private void GetBatchContacts(List<string> batch_contacts)
        {
            AddLog("获取群组联系人列表...");

            string url = "https://" + m_wx_host + "/cgi-bin/mmwebwx-bin/webwxbatchgetcontact?type=ex&lang=zh_CN" +
                "&r=" + Common.UnixtimeStamp() +
                "&pass_ticket=" + WebUtility.UrlEncode(m_pass_ticket);

            WXBatchContacts bcs = new WXBatchContacts();
            bcs.m_base_request = new WXBaseRequest();
            bcs.m_base_request.m_device_id = m_device_id;
            bcs.m_base_request.m_skey = m_skey;
            bcs.m_base_request.m_uin = m_wxuin;
            bcs.m_base_request.m_wxsid = m_wxsid;

            List<WXBatchContactListItem> lst_bcs = new List<WXBatchContactListItem>();
            foreach (string user_name in batch_contacts)
            {
                WXBatchContactListItem bcli = new WXBatchContactListItem();
                bcli.m_chat_room_id = "";
                bcli.m_user_name = user_name;
                lst_bcs.Add(bcli);
            }

            bcs.m_count = batch_contacts.Count;
            bcs.m_list = lst_bcs.ToArray();

            m_requestor.DoPostRequest(
                url: url,
                post_data: Common.EncodeJson(bcs),
                content_type: "application/json; charset=UTF-8",
                OnTimeout: OnTimeout,
                OnResponse: OnBatchContactsGot);
        }

        private void OnContactsGot(HttpWebRequest req, HttpWebResponse res)
        {
            StreamReader sr = new StreamReader(res.GetResponseStream());
            string str_ret = sr.ReadToEnd();
            sr.Close();

            WXContactsRes contacts = (WXContactsRes)Common.DecodeJson(str_ret, typeof(WXContactsRes));
            if (0 != contacts.m_base_response.m_ret)
            {
                MessageBox.Show("获取联系人列表失败：" + contacts.m_base_response.m_ret);
                Logout(false);
                return;
            }

            foreach (WXContact contact in contacts.m_member_list)
            {
                m_wx_contacts[contact.m_user_name] = contact;
            }

            if (0 == contacts.m_seq) // 联系人列表已获取完毕
            {
                lvwContacts.BeginUpdate();
                lvwContacts.Items.Clear();

                List<string> lst_user_names = new List<string>();
                foreach (KeyValuePair<string, WXContact> kv in m_wx_contacts)
                {
                    // 获取头像图片
                    GetHeadImg(kv.Value.m_head_img_url);

                    if (kv.Value.m_user_name.StartsWith("@@")) // 群组
                    {
                        lst_user_names.Add(kv.Value.m_user_name);
                    }

                    // 记录昵称
                    m_wx_names[kv.Value.m_user_name] = kv.Value.m_nick_name;

                    ListViewItem lvi = new ListViewItem("", kv.Value.m_user_name);
                    lvi.SubItems.Add(kv.Value.m_nick_name);
                    lvi.SubItems.Add(kv.Value.m_user_name);
                    lvi.Name = kv.Value.m_user_name;
                    lvwContacts.Items.Add(lvi);
                }

                lvwContacts.EndUpdate();

                // 获取所有群组里面的群员信息
                GetBatchContacts(lst_user_names);
            }
            else
            {
                GetContacts(contacts.m_seq);
            }

            AddLog("已成功获取列表人列表！");
        }

        private void GetContacts(int seq)
        {
            AddLog("获取联系人列表 " + seq + "...");

            string url = "https://" + m_wx_host + "/cgi-bin/mmwebwx-bin/webwxgetcontact?lang=zh_CN" +
                "&pass_ticket=" + WebUtility.UrlEncode(m_pass_ticket) +
                "&r=" + Common.UnixtimeStamp() +
                "&seq=" + seq +
                "&skey=" + WebUtility.UrlEncode(m_skey);
            m_requestor.DoGetRequest(
                url: url,
                OnTimeout: OnTimeout,
                OnResponse: OnContactsGot);
        }

        private void EnableStatusNotify()
        {
            AddLog("开启状态通知...");

            string url = "https://" + m_wx_host + "/cgi-bin/mmwebwx-bin/webwxstatusnotify";

            WXStatusNotify sn = new WXStatusNotify();
            sn.m_base_request = new WXBaseRequest();
            sn.m_base_request.m_device_id = m_device_id;
            sn.m_base_request.m_skey = m_skey;
            sn.m_base_request.m_uin = m_wxuin;
            sn.m_base_request.m_wxsid = m_wxsid;
            sn.m_client_msg_id = Common.UnixtimeStamp();
            sn.m_code = 3;
            sn.m_from_user_name = m_wx_init.m_user.m_user_name;
            sn.m_to_user_name = m_wx_init.m_user.m_user_name;

            m_requestor.DoPostRequest(
                url: url,
                post_data: Common.EncodeJson(sn),
                content_type: "application/json; charset=UTF-8",
                OnTimeout: OnTimeout);
        }

        private void OnInitGot(HttpWebRequest req, HttpWebResponse res)
        {
            StreamReader sr = new StreamReader(res.GetResponseStream());
            string str_ret = sr.ReadToEnd();
            sr.Close();

            m_wx_init = (WXInit)Common.DecodeJson(str_ret, typeof(WXInit));
            if (0 != m_wx_init.m_base_response.m_ret)
            {
                MessageBox.Show("获取初始化信息发生错误：" + m_wx_init.m_base_response.m_ret);
                Logout(false);
                return;
            }

            m_wx_msg = new Dictionary<string, List<WXMsg>>();
            m_wx_names = new Dictionary<string, string>();
            m_wx_contacts = new Dictionary<string, WXContact>();
            m_wx_sync_check_key = m_wx_init.m_sync_key;

            EnableStatusNotify();

            GetContacts(0);

            niTray.Text = m_wx_init.m_user.m_nick_name + "(" + m_wx_init.m_user.m_uin + ") 在线";
            this.Text = niTray.Text;

            AddLog("已成功初始化微信！");
        }

        private void InitWX()
        {
            AddLog("初始化微信...");

            string url = "https://" + m_wx_host + "/cgi-bin/mmwebwx-bin/webwxinit" +
                "?pass_ticket=" + WebUtility.UrlEncode(m_pass_ticket) +
                "&r=" + Common.ReverseTimestamp() +
                "&skey=" + WebUtility.UrlEncode(m_skey);

            WXInitReq ir = new WXInitReq();
            ir.m_base_request = new WXBaseRequest();
            ir.m_base_request.m_device_id = m_device_id;
            ir.m_base_request.m_skey = m_skey;
            ir.m_base_request.m_uin = m_wxuin;
            ir.m_base_request.m_wxsid = m_wxsid;

            m_requestor.DoPostRequest(
                url: url,
                post_data: Common.EncodeJson(ir),
                content_type: "application/json; charset=UTF-8",
                OnTimeout: OnTimeout,
                OnResponse: OnInitGot);
        }

        private void OnGotoURI(HttpWebRequest req, HttpWebResponse res)
        {
            XmlReader xml = XmlReader.Create(res.GetResponseStream());
            while (xml.Read())
            {
                if (XmlNodeType.Element == xml.NodeType)
                {
                    switch (xml.Name.ToLower())
                    {
                        case "ret":
                            xml.Read();
                            if ("0" != xml.Value)
                            {
                                xml.Dispose();
                                return;
                            }
                            break;

                        case "skey":
                            xml.Read();
                            m_skey = xml.Value;
                            break;

                        case "wxsid":
                            xml.Read();
                            m_wxsid = xml.Value;
                            break;

                        case "wxuin":
                            xml.Read();
                            m_wxuin = long.Parse(xml.Value);
                            break;

                        case "pass_ticket":
                            xml.Read();
                            m_pass_ticket = xml.Value;
                            break;
                    }
                }
            }

            xml.Dispose();

            AddLog("已成功跳转登录！");

            InitWX();
        }

        private void GotoURI(string jump_url)
        {
            AddLog("跳转登录...");

            UriBuilder ub = new UriBuilder(jump_url);
            m_wx_host = ub.Host;
            m_device_id = Common.DeviceID();

            string url = jump_url + "&fun=new&version=v2";
            m_requestor.DoGetRequest(
                url: url,
                OnTimeout: OnTimeout,
                OnResponse: OnGotoURI);
        }

        #endregion

        #region 界面

        private SoundPlayer m_sound = null;
        private FrmQRCode m_frm_qrcode = null;
        private NotifyIconMouseEvent m_nime = null;
        private WebRequestor m_requestor = null;

        public FrmMain()
        {
            InitializeComponent();

            m_requestor = new WebRequestor(this);
            m_wx_voices = new Dictionary<string, string>();
            m_wx_videos = new Dictionary<string, string>();
        }

        protected override void WndProc(ref Message m)
        {
            const int MM_MCINOTIFY = 0x3B9;
            const int MCI_NOTIFY_SUCCESSFUL = 0x1;

            if (MM_MCINOTIFY == m.Msg &&
                MCI_NOTIFY_SUCCESSFUL == m.WParam.ToInt32()) // 音频播放完成
            {
                System.Diagnostics.Debug.Print("done");
            }

            base.WndProc(ref m);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            niTray.Icon = WeChat.Properties.Resources.Main;
            niTray.Visible = true;

            m_nime = new NotifyIconMouseEvent(niTray, m_frm_qrcode);
            m_sound = new SoundPlayer(WeChat.Properties.Resources.msg);

            OnLogout(null, null);
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            niTray.Visible = false;
        }

        #endregion

        #region 信息

        private void tmrTimeout_Tick(object sender, EventArgs e)
        {
            if (!m_sync) return;

            long t_now = DateTime.Now.Ticks;
            if (t_now - m_last_sync > 35000 * TimeSpan.TicksPerMillisecond)
            {
                Sync();

                m_last_sync = t_now;
            }
        }

        private void lvwContacts_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            new ListViewSorter(lvwContacts, e.Column);
        }

        private void lvwContacts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (1 != lvwContacts.SelectedItems.Count)
            {
                return;
            }

            string str_content = Microsoft.VisualBasic.Interaction.InputBox("输入要发送的信息", "发送信息", "收到红包，请在手机上查看");
            if ("" == str_content) return;

            ListViewItem lvi = lvwContacts.SelectedItems[0];
            SendTextMsg(lvi.Name, str_content);
        }

        private void rtbContent_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Regex reg_voice = new Regex(@"(?<=播放语音\().+(?=\))");
            Regex reg_video = new Regex(@"(?<=播放视频\().+(?=\))");
            if (reg_voice.IsMatch(e.LinkText)) // 语音信息
            {
                FilgraphManager fm = new FilgraphManager();
                string msg_id = reg_voice.Match(e.LinkText).Value;
                string voice_file = m_wx_voices[msg_id];
                fm.RenderFile(voice_file);
                fm.Run();
            }
            else if (reg_video.IsMatch(e.LinkText)) // 视频信息
            {
                FilgraphManager fm = new FilgraphManager();
                string msg_id = reg_video.Match(e.LinkText).Value;
                string video_file = m_wx_videos[msg_id];
                fm.RenderFile(video_file);

                FrmVideo frm_video = new FrmVideo();
                frm_video.FM = fm;
                frm_video.FileName = video_file;
                frm_video.Show();
            }
            else // 普通超链接
            {
                Process.Start(e.LinkText);
            }
        }

        private void miSwitch_Click(object sender, EventArgs e)
        {
            Logout(true);
        }

        private void miRefreshQRCode_Click(object sender, EventArgs e)
        {
            m_frm_qrcode.ReloadQRCode();
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            Logout(false);
        }

        #endregion

        #region 日志

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
        }

        #endregion
    }
}
