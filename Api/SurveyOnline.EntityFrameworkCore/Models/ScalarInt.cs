﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyOnline.EntityFrameworkCore.Models
{
    public partial class ScalarInt
    {
        public int Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}