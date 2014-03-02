using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpWit.Vitals.Brain
{
    class Action
    {
        private Objects.O_NLP.RootObject o_NLP = new Objects.O_NLP.RootObject();
        double conf = 0D;

        public string makeSentence(Objects.O_NLP.RootObject _o_NLP)
        {
            try
            {
                // Bind to the wit NLP respons class
                o_NLP = _o_NLP;
                conf = (o_NLP.outcome.confidence * 100);

                string sentence = "";

                sentence += "I'm " + conf.ToString() + "% sure you want me to do something.";

                // Try {} catch {} are quick fixes to exceptions, code should be made more robust to handle
                // the various types

                // This is also the place to add your custom code to the intent, ie. add the appointment or process the action

                try
                {
                    string obj = o_NLP.outcome.entities._object[0].value;
                    sentence += Environment.NewLine + "You want to interact with: " + obj;

                    try
                    {
                        string action = o_NLP.outcome.entities.on_off[0].value;
                        sentence += Environment.NewLine + "You want it: " + action;

                    }
                    catch { }
                }
                catch { }

                try
                {
                    try
                    {
                        string obj2 = o_NLP.outcome.entities.contact[0].value;
                        sentence += Environment.NewLine + "You want to send a message to: " + obj2;
                    }
                    catch { }

                    string obj = o_NLP.outcome.entities.message_body[0].value;
                    sentence += Environment.NewLine + "You want to send this message: " + obj;
                }
                catch { }

                return sentence;
            }
            catch (Exception ex)
            {
                return "Uh oh, something went wrong: " + ex.Message;
            }
        }
    }
}
