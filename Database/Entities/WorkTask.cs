﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Database
{
    public class WorkTask
    {
        public int Id { get; set; }
        public DateTime? dateSelected { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
