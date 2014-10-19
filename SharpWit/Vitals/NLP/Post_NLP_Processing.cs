using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Data;

namespace SharpWit.Vitals.NLP
{
    static class Post_NLP_Processing
    {
        public static Objects.O_NLP.RootObject ParseData(string text)
        {
            // HTML-decode the string, in case it has been HTML encoded
            string jsonText = System.Web.HttpUtility.HtmlDecode(text);

            //Since object is reserved, put a _ in front
            jsonText = jsonText.Replace("\"object\" : {", "\"_object\" : {");

            //Deserialize into our class
            var rootObject = JsonConvert.DeserializeObject<Objects.O_NLP.RootObject>(jsonText);

            // Let it work for voice and speech
            if (rootObject.outcomes == null)
            {
                rootObject.outcomes = new List<Objects.O_NLP._Outcome>();
                rootObject.outcomes.Add(rootObject.outcome);
            }
            else rootObject.outcome = rootObject.outcomes[0];

            return rootObject;
        }
    }
}
