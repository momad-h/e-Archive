using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_Archive.DataModels
{
    public class LogInfo
    {
        public string Message { get; set; }
        public string Level { get; set; }
        public string StackTrace { get; set; }
        public string ETC { get; set; }
        public string EC { get; set; }
        public string PersonnelID { get; set; }
        public string Category { get; set; }
        public string FileName { get; set; }
    }
}
