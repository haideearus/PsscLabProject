﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public record ValidatedShoppingCart(List<string> Items, decimal TotalPrice) : ICartState;
  
}
