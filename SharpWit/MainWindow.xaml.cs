using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SharpWit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // O_NLP.RootObject is a class that contains the data interpreted from wit.ai
        Objects.O_NLP.RootObject oNLP = new Objects.O_NLP.RootObject();

        // NLP_Processing is the code that processes the response from wit.ai
        Vitals.NLP.NLP_Processing vitNLP = new Vitals.NLP.NLP_Processing();

        // Winmm.dll is used for recording speech
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        // Variables used for speech recording
        private bool recording = false;
        private string speechfilename = "";
        // Set a timer to make sure recording doesn't exceed 10 seconds
        private System.Timers.Timer speechTimer = new System.Timers.Timer();

        public MainWindow()
        {
            InitializeComponent();

            tbYou.Focus();

            speechTimer = new System.Timers.Timer();
            speechTimer.Elapsed += new ElapsedEventHandler(OnTimedSpeechEvent);
            speechTimer.Interval = 10000; //10 seconds
        }

        private void tbYou_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && tbYou.Text.Length > 0)
            {
                btnRecord.IsEnabled = false;
                tbYou.IsEnabled = false;

                StartProcessing(tbYou.Text, 0);

                lblYou.Content = "You said: " + tbYou.Text;
                tbYou.Text = "";
                tbI.Text = "Hold on..";
                tbYou.Focus();
            }
        }

        // Async + wait keeps the GUI thread responsive
        public async void StartProcessing(string text, int type)
        {
            try
            {
                string modtext = Vitals.NLP.Pre_NLP_Processing.preprocessText(text);

                string nlp_text = "";

                if (type == 0)
                {
                    nlp_text = await vitNLP.ProcessWrittenText(modtext);
                }
                else
                {
                    nlp_text = await vitNLP.ProcessSpokenText(text);
                }

                // If the audio file doesn't contain anything, or wit.ai doesn't understand it, a code 400 will be returned
                if (nlp_text.Contains("The remote server returned an error: (400) Bad Request"))
                {
                    tbI.Text = "Sorry, didn't get that. Could you please repeat yourself?";
                    btnRecord.IsEnabled = true;
                    tbYou.IsEnabled = true;
                    tbRaw.Text = nlp_text;
                    return;
                }

                tbRaw.Text = nlp_text;

                oNLP = Vitals.NLP.Post_NLP_Processing.ParseData(nlp_text);                

                // This codeblock dynamically casts the intent to the corresponding class
                // Check README.txt in Vitals.Brain
                Assembly objAssembly;
                objAssembly = Assembly.GetExecutingAssembly();

                Type classType = objAssembly.GetType("SharpWit.Vitals.Brain." + oNLP.outcome.intent);

                object obj = Activator.CreateInstance(classType);

                MethodInfo mi = classType.GetMethod("makeSentence");

                object[] parameters = new object[1];
                parameters[0] = oNLP;

                mi = classType.GetMethod("makeSentence");
                string sentence = "";
                sentence = (string)mi.Invoke(obj, parameters);

                // Show what was deducted from the sentence
                tbI.Text = sentence;

                btnRecord.IsEnabled = true;
                tbYou.IsEnabled = true;
            }
            catch (Exception ex)
            {
                btnRecord.IsEnabled = true;
                tbYou.IsEnabled = true;

                tbI.Text = "Sorry, no idea what's what. Try again later please!" + Environment.NewLine + Environment.NewLine + "I bumped onto this error: " + ex.Message;
            }
        }

        private void btnShutdown_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            if (!recording)
            {
                recording = true;
                string tempfile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), RandomString(8));

                speechfilename = @tempfile;

                mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
                mciSendString("record recsound", "", 0, 0);
                btnRecord.Content = "stop";
                speechTimer.Enabled = true;
            }
            else
            {
                recording = false;
                speechTimer.Enabled = false;
                mciSendString("save recsound " + speechfilename, "", 0, 0);
                mciSendString("close recsound ", "", 0, 0);
                btnRecord.Content = "record";
                btnRecord.IsEnabled = false;
                tbYou.IsEnabled = false;

                StartProcessing(speechfilename, 1);

                lblYou.Content = "";
                tbYou.Text = "";
                tbI.Text = "Hold on..";
                tbYou.Focus();
            }
        }

        // After 10 seconds, this timer gets called if the user doesn't click 'stop'
        private void OnTimedSpeechEvent(object source, ElapsedEventArgs e)
        {
            speechTimer.Enabled = false;

            // Replace with this.InvokeRequired for WinForms
            // Contact me at sam@tabnw.org if you need help
            if (!CheckAccess())
            {
                Dispatcher.Invoke(() => OnTimedSpeechEvent(source, e));
                return;
            }
            else
            {
                recording = false;
                mciSendString("save recsound " + speechfilename, "", 0, 0);
                mciSendString("close recsound ", "", 0, 0);
                btnRecord.Content = "record";
                btnRecord.IsEnabled = false;
                tbYou.IsEnabled = false;

                StartProcessing(speechfilename, 1);

                lblYou.Content = "";
                tbYou.Text = "";
                tbI.Text = "Hold on..";
                tbYou.Focus();
            }            
        }

        // Generate temp random string name
        // Thanks RCIX @ stackoverflow.com :)
        private static Random random = new Random((int)DateTime.Now.Ticks);
        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}
