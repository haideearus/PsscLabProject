using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    // Assuming these records are already defined as per your structure:

    public interface ICartState
    {
        List<string> Items { get; }
    }

    public class ShoppingCart
    {
        private ICartState _currentState;
        internal IEnumerable<object> Items;

        public ShoppingCart()
        {
            _currentState = new EmptyShoppingCart();
            Items = new List<object>(); // Initialize the Items field
        }

        public ICartState GetState()
        {
            return _currentState;
        }

        // Funcție pentru schimbarea stării coșului
        public void ChangeState(ICartState newState)
        {
            _currentState = newState;
        }

        // Funcție pentru adăugarea unui articol în coș
        public void AddItem(string item)
        {
            if (_currentState is EmptyShoppingCart)
            {
                _currentState = new UnvalidatedShoppingCart(new List<string> { item });
            }
            else if (_currentState is UnvalidatedShoppingCart unvalidatedCart)
            {
                unvalidatedCart.Items.Add(item);
            }
            else
            {
                Console.WriteLine("Nu mai poți adăuga articole în coș în această stare.");
            }
        }

        // Funcție pentru ștergerea unui articol din coș
        public void RemoveItem(string item)
        {
            if (_currentState is UnvalidatedShoppingCart unvalidatedCart)
            {
                if (unvalidatedCart.Items.Contains(item))
                {
                    unvalidatedCart.Items.Remove(item);
                    Console.WriteLine($"Articolul {item} a fost eliminat din coș.");
                }
                else
                {
                    Console.WriteLine($"Articolul {item} nu există în coș.");
                }
            }
            else
            {
                Console.WriteLine("Nu poți elimina articole din coș în această stare.");
            }
        }

        // Funcție pentru validarea coșului
        public void ValidateCart()
        {
            if (_currentState is UnvalidatedShoppingCart unvalidatedCart)
            {
                decimal totalPrice = CalculateTotalPrice(unvalidatedCart.Items);
                ChangeState(new ValidatedShoppingCart(unvalidatedCart.Items, totalPrice));
                Console.WriteLine("Coșul a fost validat.");
            }
            else
            {
                Console.WriteLine("Coșul nu poate fi validat deoarece nu este în stare nevalidată.");
            }
        }

        // Funcție pentru plata coșului
        public void PayCart()
        {
            if (_currentState is ValidatedShoppingCart validatedCart)
            {
                string paymentDetails = "Plătit prin card"; // Detaliu de plată simplificat
                ChangeState(new PaidShoppingCart(validatedCart.Items, validatedCart.TotalPrice, paymentDetails));
                Console.WriteLine("Plata a fost efectuată.");
            }
            else
            {
                Console.WriteLine("Coșul nu poate fi plătit deoarece nu este validat.");
            }
        }

        // Funcție pentru vizualizarea articolelor din coș
        public void ViewItems()
        {
            if (_currentState is ICartState state)
            {
                if (state.Items.Any())
                {
                    Console.WriteLine("Articolele din coș:");
                    foreach (var item in state.Items)
                    {
                        Console.WriteLine($"- {item}");
                    }
                }
                else
                {
                    Console.WriteLine("Coșul este gol.");
                }
            }
        }

        // Funcție pentru restaurarea coșului la starea inițială
        public void ResetCart()
        {
            _currentState = new EmptyShoppingCart();
            Console.WriteLine("Coșul a fost resetat la starea inițială.");
        }

        // Funcție pentru calcularea prețului total
        public decimal CalculateTotalPrice(List<string> items)
        {
            // Presupunem că fiecare produs are un preț fix de 100 pentru exemplu
            return items.Count * 100;
        }
    }
}
