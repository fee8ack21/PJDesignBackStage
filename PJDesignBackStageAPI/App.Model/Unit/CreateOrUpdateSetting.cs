﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class CreateOrUpdateSettingRequest : EditRequestBase
    {
        public int UnitId { get; set; }
        public object? Content { get; set; }
    }
}
