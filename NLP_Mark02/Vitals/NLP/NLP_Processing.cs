using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace NLP_Mark02.Vitals.NLP
{
    class NLP_Processing
    {
        public Task<string> ProcessNLP(string text)
        {
            return Task.Run(() =>
            {
                return ProcessText(text);
            });
        }

        // Send the text to the wit API
        private string ProcessText(string text)
        {
            string url = "https://api.wit.ai/message?q=" + text;

            WebRequest request = WebRequest.Create(url);

            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer paste_your_access_token_here";

            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            string serverResponse = reader.ReadToEnd();
            
            return serverResponse;
        }
    }
}
