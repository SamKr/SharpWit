using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace SharpWit.Vitals.NLP
{
    class NLP_Processing
    {
        private string wit_ai_access_token = "replace_with_your_access_token";

        public Task<string> ProcessWrittenText(string text)
        {
            return Task.Run(() =>
            {
                return ProcessText(text);
            });
        }

        public Task<string> ProcessSpokenText(string file)
        {
            return Task.Run(() =>
            {
                return ProcessSpeech(file);
            });
        }

        // Send the text to the wit API
        private string ProcessText(string text)
        {
            string url = "https://api.wit.ai/message?q=" + text;

            WebRequest request = WebRequest.Create(url);

            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer " + wit_ai_access_token;

            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            string serverResponse = reader.ReadToEnd();
            
            return serverResponse;
        }

        // Send the wav file to the wit API
        private string ProcessSpeech(string file)
        {
            FileStream FS_Audiofile = new FileStream(file, FileMode.Open, FileAccess.Read);
            BinaryReader BR_Audiofile = new BinaryReader(FS_Audiofile);
            byte[] BA_AudioFile = BR_Audiofile.ReadBytes((Int32)FS_Audiofile.Length);
            FS_Audiofile.Close();
            BR_Audiofile.Close();

            HttpWebRequest _HWR_SpeechToText = null;

            _HWR_SpeechToText = (HttpWebRequest)WebRequest.Create("https://api.wit.ai/speech");

            _HWR_SpeechToText.Method = "POST";
            _HWR_SpeechToText.Headers["Authorization"] = "Bearer " + wit_ai_access_token;
            _HWR_SpeechToText.ContentType = "audio/wav";
            _HWR_SpeechToText.ContentLength = BA_AudioFile.Length;
            _HWR_SpeechToText.GetRequestStream().Write(BA_AudioFile, 0, BA_AudioFile.Length);

            // Delete the temp file
            try
            {
                File.Delete(file);
            }
            catch
            {
                MessageBox.Show("Unable to delete the temp file!" + Environment.NewLine + "Please do so yourself: " + file);
            }

            // Process the wit.ai response
            try
            {
                HttpWebResponse HWR_Response = (HttpWebResponse)_HWR_SpeechToText.GetResponse();
                if (HWR_Response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader SR_Response = new StreamReader(HWR_Response.GetResponseStream());
                    return SR_Response.ReadToEnd();
                }
                else
                {
                    return "Error: " + HWR_Response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }       
        }
    }
}
