﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    public class WorkTaskViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? dateSelected { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
}
