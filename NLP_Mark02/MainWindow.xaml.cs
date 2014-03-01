using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Reflection;

namespace NLP_Mark02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // O_NLP.RootObject is a respons from wit
        Objects.O_NLP.RootObject oNLP = new Objects.O_NLP.RootObject();

        // NLP_Processing is the code that processes the respons from wit
        Vitals.NLP.NLP_Processing vitNLP = new Vitals.NLP.NLP_Processing();


        public MainWindow()
        {
            InitializeComponent();

            tbYou.Focus();
        }

        private void tbYou_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && tbYou.Text.Length > 0)
            {
                StartProcessing(tbYou.Text);

                lblYou.Content = "You said: " + tbYou.Text;
                tbYou.Text = "";
                tbI.Text = "Hold on..";
                tbYou.Focus();                
            }
        }

        // Async + wait keeps the gui thread responsive
        public async void StartProcessing(string text)
        {
            try
            {
                string modtext = Vitals.NLP.Pre_NLP_Processing.preprocessText(text);

                string nlp_text = await vitNLP.ProcessNLP(modtext);
                tbRaw.Text = nlp_text;

                oNLP = Vitals.NLP.Post_NLP_Processing.ParseData(nlp_text);

                // This codeblock dynamically casts the intent to the corresponding class
                Assembly objAssembly;
                objAssembly = Assembly.GetExecutingAssembly();

                Type classType = objAssembly.GetType("NLP_Mark02.Vitals.Brain." + oNLP.outcome.intent);

                object obj = Activator.CreateInstance(classType);

                MethodInfo mi = classType.GetMethod("makeSentence");

                object[] parameters = new object[1];
                parameters[0] = oNLP;

                mi = classType.GetMethod("makeSentence");
                string sentence;
                sentence = (string)mi.Invoke(obj, parameters);

                // Show what was deducted from the sentence
                tbI.Text = sentence;
            }
            catch (Exception ex)
            {
                tbI.Text = "Sorry, no idea what's what. Try again later please!" + Environment.NewLine + "I bumped onto this error: " + ex.Message;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
