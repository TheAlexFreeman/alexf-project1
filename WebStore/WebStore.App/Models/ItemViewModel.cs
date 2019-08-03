﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using WebStore.BLL;

namespace WebStore.App.Models
{
    public class ItemViewModel
    {
        public ItemViewModel(Item item)
        {
            Id = item.Id;
            Name = item.Name;
            Cost = item.Cost;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        [Range(0, 65535)]
        public double Cost { get; set; }

    }
}
