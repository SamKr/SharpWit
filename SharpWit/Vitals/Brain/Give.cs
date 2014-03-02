using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpWit.Vitals.Brain
{
    class Give
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

                sentence += "I think you want something from me.";

                // Try {} catch {} are quick fixes to exceptions, code should be made more robust to handle
                // the various types

                // This is also the place to add your custom code to the intent, ie. add the appointment or process the action

                try
                {
                    sentence += Environment.NewLine + "You want a " + o_NLP.outcome.entities._object[0].value + ".";
                }
                catch { }


                try
                {
                    sentence += " You want it on " + o_NLP.outcome.entities.datetime[0].value.from.DayOfWeek + ".";
                }
                catch { }

                sentence += " I'm " + conf.ToString() + "% sure of this.";

                return sentence;
            }
            catch (Exception ex)
            {
                return "Uh oh, something went wrong: " + ex.Message;
            }
        }
    }
}
