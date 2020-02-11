using System;
using System.Collections.Generic;
using System.Text;

namespace Konference.Models
{
    class AssetBinary
    {
        public string FileName { get; set; }
        public int ContentLength { get; set; }
        public string ContentType { get; set; }
        public byte[] Binary { get; set; }
    }
}
