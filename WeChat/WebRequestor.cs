using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace WeChat
{
    public class WebRequestor : IDisposable
    {
        private const int TIMEOUT = 30000;

        private class RequestInfo
        {
            internal HttpWebRequest m_req;
            internal Action m_func_timeout;
            internal Action<HttpWebRequest, HttpWebResponse> m_func_res;
            internal string m_post_data;

            public RequestInfo(
                HttpWebRequest req,
                Action func_timeout,
                Action<HttpWebRequest, HttpWebResponse> func_res,
                string post_data = "",
                object param = null)
            {
                m_req = req;
                m_func_timeout = func_timeout;
                m_func_res = func_res;
                m_post_data = post_data;
            }
        }

        private CookieContainer m_cookies = null;
        public CookieContainer Cookies
        {
            get { return m_cookies; }
        }

        private Form m_owner = null;
        public Form Owner
        {
            get { return m_owner; }
            set { m_owner = value; }
        }

        public WebRequestor(Form frm)
        {
            m_owner = frm;
            m_cookies = new CookieContainer();
        }

        public void Dispose()
        {
            m_owner = null;
            m_cookies = null;
        }

        private void GetResponseAR(IAsyncResult ar)
        {
            RequestInfo info = ar.AsyncState as RequestInfo;
            HttpWebRequest req = info.m_req;
            HttpWebResponse res = (HttpWebResponse)req.EndGetResponse(ar);
            foreach (Cookie cookie in res.Cookies)
            {
                m_cookies.Add(cookie);
            }

            if (null != info.m_func_res)
            {
                m_owner.Invoke(info.m_func_res, req, res);
            }

            res.Close();
        }

        public void DoGetRequest(
            string url,
            string content_type = "",
            long range = 0,
            int timeout = TIMEOUT,
            Action OnTimeout = null,
            Action<HttpWebRequest, HttpWebResponse> OnResponse = null)
        {
            System.Diagnostics.Debug.Print(DateTime.Now.ToString("HH:mm:ss") + " get " + url);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Proxy = null;
            req.Timeout = timeout;
            req.ReadWriteTimeout = timeout;
            req.CookieContainer = m_cookies;
            req.Method = WebRequestMethods.Http.Get;

            if (range >= 0)
            {
                req.AddRange(range);
            }

            try
            {
                RequestInfo info = new RequestInfo(req, OnTimeout, OnResponse);
                req.BeginGetResponse(new AsyncCallback(GetResponseAR), info);
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.Print(DateTime.Now.ToString("HH:mm:ss") + " get timeout " + url);

                if (null != OnTimeout)
                {
                    m_owner.Invoke(OnTimeout);
                }
            }
        }

        private void GetRequestStreamAsync(IAsyncResult ar)
        {
            RequestInfo info = ar.AsyncState as RequestInfo;
            HttpWebRequest req = info.m_req;
            Stream s = req.EndGetRequestStream(ar);

            if ("" != info.m_post_data)
            {
                byte[] bytes_init = Encoding.UTF8.GetBytes(info.m_post_data);
                s.Write(bytes_init, 0, bytes_init.Length);
            }

            s.Close();

            try
            {
                req.BeginGetResponse(new AsyncCallback(GetResponseAR), info);
            }
            catch (Exception)
            {
                if (null != info.m_func_timeout)
                {
                    m_owner.Invoke(info.m_func_timeout);
                }
            }
        }

        public void DoPostRequest(
            string url,
            string post_data,
            string content_type = "",
            int timeout = TIMEOUT,
            Action OnTimeout = null,
            Action<HttpWebRequest, HttpWebResponse> OnResponse = null)
        {
            System.Diagnostics.Debug.Print(DateTime.Now.ToString("HH:mm:ss") + " post " + url);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Proxy = null;
            req.Timeout = timeout;
            req.ReadWriteTimeout = timeout;
            req.CookieContainer = m_cookies;
            req.Method = WebRequestMethods.Http.Post;

            req.ContentType = "application/json; charset=UTF-8; text/plain; *.*";
            req.ContentLength = Encoding.UTF8.GetByteCount(post_data);

            RequestInfo info = new RequestInfo(req, OnTimeout, OnResponse, post_data);
            try
            {
                req.BeginGetRequestStream(new AsyncCallback(GetRequestStreamAsync), info);
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.Print(DateTime.Now.ToString("HH:mm:ss") + " post timeout " + url);

                if (null != OnTimeout)
                {
                    m_owner.Invoke(OnTimeout);
                }
            }
        }
    }
}
