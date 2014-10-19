using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace SharpWit
{
    class Voice
    {
        public static void Say(string text)
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.Volume = 100;  // 0...100
            synthesizer.Rate = -2;     // -10...10

            // Synchronous
            synthesizer.Speak(text);
        }
    }
}
