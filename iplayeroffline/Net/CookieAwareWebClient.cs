
using System;
using System.Net;

namespace BigTed.OnTheTelly.Net
{

	/*
	 * Web client which is aware of the cookie container, so we can
	 * send back the same cookies each time
	 */ 

	public class CookieAwareWebClient : WebClient
    {

        private CookieContainer m_container = new CookieContainer();
		public CookieContainer CookieContainer
		{
			get { return m_container; }
			set { m_container = value; }
		}

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = m_container;
            }
			
            return request;
        }
		
		
    }
}
