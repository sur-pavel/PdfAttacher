using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfAttacher
{
    class IrbisRequest
    {
        public string Author { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string[] TitleKeyWords { get; set; } = new string[] { };
        public string Year { get; set; } = string.Empty;
        public string VolNum { get; set; } = string.Empty;
        public Status FileStatus { get; set; }
        public string FileNameFromRecord { get; set; } = string.Empty;
        public enum Status
        {
            OneRecord = 1,
            NotFounded = 2,
            FewRecords = 3,
            Doublet = 4
        }
    }
}
