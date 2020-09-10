using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfAttacher
{
    public interface IStemmer
    {
        string Stem(string s);
    }
}
