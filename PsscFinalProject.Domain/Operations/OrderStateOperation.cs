using PsscFinalProject.Domain.Models;
using System;

namespace PsscFinalProject.Domain.Operations
{
    public class OrderStateOperation
    {
        public OrderState CalculateOrderState(DateTime orderDate)
        {
            var timeElapsed = DateTime.Now - orderDate;

            // Starea 1 - Pending
            if (timeElapsed.TotalMinutes < 5)
            {
                return OrderState.Pending;
            }

            // Starea 2 - Billed
            if (timeElapsed.TotalMinutes >= 5 && timeElapsed.TotalMinutes < 60)
            {
                return OrderState.Billed;
            }

            // Starea 3 - Delivered
            return OrderState.Delivered;
        }
    }
}