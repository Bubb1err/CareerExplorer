﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Entities
{
    public class SkillsTag
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AdminId { get; set; }
        public virtual Admin Admin { get; set; }

    }
}
