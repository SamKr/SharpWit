using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace NLP_Mark02.Vitals.Brain
{
    class Question
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

                sentence += "I'm " + conf.ToString() + "% sure you posed a question.";

                // Try {} catch {} are quick fixes to exceptions, code should be made more robust to handle
                // the various types

                // This is also the place to add your custom code to the intent, ie. add the appointment or process the action

                try
                {
                    string math = o_NLP.outcome.entities.math_expression.value;
                    sentence += Environment.NewLine + "You want to know the outcome of " + math + ".";
                }
                catch
                {
                    try
                    {
                        string question = o_NLP.outcome.entities.question.value;
                        sentence += Environment.NewLine + "You want to know about: " + question + ".";
                    }
                    catch
                    {
                        sentence += Environment.NewLine + "Not sure how to answer it, though.";
                    }
                }


                return sentence;
            }
            catch (Exception ex)
            {
                return "Uh oh, something went wrong: " + ex.Message;
            }
        }
    }
}
