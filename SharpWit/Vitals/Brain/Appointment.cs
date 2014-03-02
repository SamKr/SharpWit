using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpWit.Vitals.Brain
{
    class Appointment
    {
        private Objects.O_NLP.RootObject o_NLP = new Objects.O_NLP.RootObject();
        double conf = 0D;

        public string makeSentence(Objects.O_NLP.RootObject _o_NLP)
        {
            try
            {
                // Bind to the wit.ai NLP response class
                o_NLP = _o_NLP;
                conf = (o_NLP.outcome.confidence * 100);

                string sentence = "";

                sentence += "I'm " + conf.ToString() + "% sure you are talking about appointments.";

                // Try {} catch {} are quick fixes to exceptions, code should be made more robust to handle
                // the various types

                // This is also the place to add your custom code to the intent, ie. add the appointment or process the action

                try
                {
                    string appointment = o_NLP.outcome.entities.agenda_entry[0].value;
                    sentence += Environment.NewLine + "You want to make this appointment: " + appointment;

                    try
                    {
                        string startdate = o_NLP.outcome.entities.datetime[0].value.from.ToString();
                        sentence += Environment.NewLine + "You want it: " + startdate;
                        try
                        {
                            string enddate = o_NLP.outcome.entities.datetime[0].value.to.ToString();
                            sentence += Environment.NewLine + "You want it 'till: " + enddate;

                        }
                        catch { }

                    }
                    catch { }
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
