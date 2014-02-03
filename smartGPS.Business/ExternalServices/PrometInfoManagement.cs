using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using smartGPS.Business.Models.PrometInfo;

namespace smartGPS.Business.ExternalServices
{
    public class PrometInfoManagement
    {
        public static PrometInfoResponse getInfo()
        {
            PrometInfoResponse model = null;
            String url = null;

            url = APICalls.getRoadConditionsUrl();

            WebRequest request = WebRequest.Create(url);

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        String responseString = new StreamReader(stream).ReadToEnd();
                        model = JsonConvert.DeserializeObject<PrometInfoResponse>(responseString);
                        return model;
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}