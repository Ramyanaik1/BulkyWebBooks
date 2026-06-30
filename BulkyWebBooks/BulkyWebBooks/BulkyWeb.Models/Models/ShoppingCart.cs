using BulkyWeb.Models.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ShoppingCart
{
    [Key]
    public int ShoppingCartId { get; set; }

    public string ApplicationUserId { get; set; }

    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }

    public int ProductId { get; set; }

    // ✅ Fix: point to ProductId, not "Id"
    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product Product { get; set; }

    [Range(1, 1000, ErrorMessage = "Please enter value between 1-1000")]
    public int Count { get; set; }

    [NotMapped]
    public double price { get; set; }

    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    public DateTime UpdatedDateTime { get; set; } = DateTime.Now;
    public bool IsOrderPlaced { get; set; } = false;

}