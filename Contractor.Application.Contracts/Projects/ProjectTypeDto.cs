﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Projects
{
    public class ProjectTypeDto
    {
        public int Id { get; private set; }

        public string ArabicDescription { get; set; } = null!;

        public string EnglishDescription { get; set; } = null!;
    }
}
