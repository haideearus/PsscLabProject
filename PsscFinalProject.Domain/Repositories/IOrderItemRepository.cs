﻿using PsscFinalProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IOrderItemRepository
    {
            Task<List<ValidatedProduct>> GetOrderItemsAsync();
            Task SaveOrderItemsAsync(IEnumerable<ValidatedProduct> orderItems);
            Task DeleteOrderItemAsync(int orderItemId);
            Task AddOrderItemAsync(int orderId, string productCode, decimal price);
            Task AddOrderItemsAsync(IEnumerable<ValidatedProduct> orderItems);
    }
}
