﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance_Tracker.Models
{
    public class ExpenseIncome
    {
        public long Id { get; set; }
        public float Amount { get; set; }
        public DateTime Time { get; set; }
        public string Description{ get; set; }
        public string TypeName { get; set; }
        public string TagName { get; set; }
        public string CategoryName { get; set; }
        public string MethodName { get; set; }
    }
}
