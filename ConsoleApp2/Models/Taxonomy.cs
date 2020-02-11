using System;
using System.Collections.Generic;
using System.Text;

namespace Konference.Models
{
    class Taxonomy
    {
        public string name { get; set; }
        public string external_id { get; set; }
        public string codename { get; set; }
        public Term[] terms { get; set; }
    }

    class Term
    {
        public string name { get; set; }
        public string external_id { get; set; }
        public string codename { get; set; }
        public Term[] terms { get; set; }
    }
}
