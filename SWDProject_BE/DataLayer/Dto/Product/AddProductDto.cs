﻿using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Dto.Product
{
    public class AddProductDto
    {

        public int UserId { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? UrlImg { get; set; }

        public int StockQuantity { get; set; }

        public bool Status { get; set; }


        public AddProductDto(int userId, int categoryId, string name, string? description, string? urlImg, int stockQuantity, bool status)
        {
            UserId = userId;
            CategoryId = categoryId;
            Name = name;
            Description = description;
            UrlImg = urlImg;
            StockQuantity = stockQuantity;
            Status = status;

        }
    }
}
