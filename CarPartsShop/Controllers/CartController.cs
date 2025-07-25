﻿using CarPartsShop.Data;
using CarPartsShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarPartsShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(TempDb.CartItems.Where(ci => !ci.Deleted));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = TempDb.CartItems.FirstOrDefault(ci => ci.Id == id && !ci.Deleted);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public IActionResult AddToCart(CartItem item)
        {
            var product = TempDb.Products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product == null) return BadRequest("Invalid ProductId");

            item.Id = TempDb.CartItems.Count + 1;
            TempDb.CartItems.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateQuantity(int id, CartItem updated)
        {
            var item = TempDb.CartItems.FirstOrDefault(ci => ci.Id == id && !ci.Deleted);
            if (item == null) return NotFound();

            item.Quantity = updated.Quantity;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = TempDb.CartItems.FirstOrDefault(ci => ci.Id == id);
            if (item == null) return NotFound();

            item.Deleted = true;
            return NoContent();
        }

        [Authorize]
        [HttpPost("process")]
        public IActionResult ProcessCart()
        {
            var email = User.Identity?.Name;

            var cartItems = TempDb.CartItems
                .Where(c => c.CustomerEmail == email && !c.Processed && !c.Deleted)
                .ToList();

            if (!cartItems.Any())
                return BadRequest("Cart is empty or already processed.");

            var receipt = new Receipt
            {
                Id = TempDb.Receipts.Count + 1,
                CustomerEmail = email,
                Items = cartItems.Select(item => new CartItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    CustomerEmail = item.CustomerEmail
                }).ToList(),
                TotalAmount = cartItems.Sum(i => i.Price * i.Quantity),
                CreatedAt = DateTime.UtcNow
            };

            TempDb.Receipts.Add(receipt);

            foreach (var item in cartItems)
            {
                item.Processed = true;
            }

            //E-mail receipt simulation
            var receiptText = $"--- Receipt #{receipt.Id} ---\n" +
                              $"Date: {receipt.CreatedAt}\n" +
                              $"Client: {receipt.CustomerEmail}\n" +
                              $"Products:\n";

            foreach (var item in receipt.Items)
            {
                receiptText += $"- {item.ProductName} x{item.Quantity} = {item.Price * item.Quantity} zł\n";
            }

            receiptText += $"Sum: {receipt.TotalAmount} zł\n\n" +
                           $"(Simulation: a receipt was sent to {receipt.CustomerEmail})";

            // Output to console (simulating email sending)
            Console.WriteLine(receiptText);

            // Save receipt to a file
            var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "Receipts");
            Directory.CreateDirectory(dirPath);
            var filePath = Path.Combine(dirPath, $"receipt_{receipt.Id}.txt");
            System.IO.File.WriteAllText(filePath, receiptText);

            return Ok(receipt);
        }

    }

}

