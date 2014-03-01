using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpWit.Objects
{
    // Fill this class with the corresponding objects, the names should correspond with the json names
    // The structure should also correspond with the json structure or they won't be cast

    // A cleaner solution would be to make a class per response type, only takes minimal code adjustments

    class O_NLP
    {
        // Default classes
        public class RootObject
        {
            public string msg_id { get; set; }
            public string msg_body { get; set; }            
            public _Outcome outcome { get; set; }            
        }

        public class _Outcome
        {
            public string intent { get; set; }
            public _Entities entities { get; set; }            
            public double confidence { get; set; }
        }

        // You should add every custom entity here
        // Can be an array or single
        public class _Entities
        {
            public _DateTime datetime { get; set; }
            public _Object _object { get; set; }
            public _WolframAlpha wolfram_search_query { get; set; }
            public _Math_Expression math_expression { get; set; }
            public _Agenda_Entry agenda_entry { get; set; }
            public _On_Off on_off { get; set; }
            public _Question question { get; set; }
        }

        public class _DateTime
        {
            public int end { get; set; }
            public int start { get; set; }
            public _Value value { get; set; }
            public string body { get; set; }            
        }

        public class _Value
        {
            public DateTime from { get; set; }
            public DateTime to { get; set; }
        }

        public class _Object
        {
            public int end { get; set; }
            public int start { get; set; }
            public string value { get; set; }
            public string body { get; set; }
        }

        // Custom entities
        public class _Question
        {
            public int end { get; set; }
            public int start { get; set; }
            public string value { get; set; }
            public string body { get; set; }
        }

        public class _WolframAlpha
        {
            public string body { get; set; }
        }

        public class _Math_Expression
        {
            public int end { get; set; }
            public int start { get; set; }
            public string value { get; set; }
            public string body { get; set; }
            public bool suggested { get; set; }
        }

        public class _Agenda_Entry
        {
            public int end { get; set; }
            public int start { get; set; }
            public string value { get; set; }
            public string body { get; set; }
            public bool suggested { get; set; }
        }

        public class _On_Off
        {
            public string value { get; set; }
        }
    }
}
