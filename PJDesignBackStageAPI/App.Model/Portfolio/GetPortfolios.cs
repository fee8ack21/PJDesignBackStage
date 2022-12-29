﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class GetPortfoliosResponse : EditResponseBase
    {
        public int Id { get; set; }
        public bool IsBefore { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? Date { get; set; }
        public DateTime CreateDt { get; set; }
        public bool IsEnabled { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
    }
}
