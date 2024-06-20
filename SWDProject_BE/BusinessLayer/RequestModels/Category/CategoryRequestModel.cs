﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModels.Category
{
    public class CategoryRequestModel
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
