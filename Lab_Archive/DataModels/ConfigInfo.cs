﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_Archive
{
    public static class ConfigInfo
    {
        public static string ConnectionStr { get; set; }
        public static string FarzinUrl { get; set; }
        public static string FarzinUsername { get; set; }
        public static string FarzinPassword { get; set; }
        public static int MasterETC { get; set; }
        public static int SlaveETC { get; set; }
        public static int WFID { get; set; }
        public static int Starter { get; set; }
        public static int MaxDegreeOfParallelism { get; set; }
        public static int NumberOfRecordsFetched { get; set; }
        public static string SubFormFileFieldName { get; set; }
        public static string WhereConditionFieldName { get; set; }

    }
}
