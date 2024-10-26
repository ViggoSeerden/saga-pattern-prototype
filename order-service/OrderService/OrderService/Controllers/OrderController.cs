using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("orders")]
public class OrderController : ControllerBase
{
    private static readonly List<Order> Orders = [new Order(3)];

    [HttpGet("")]
    public List<Order> GetAllOrders()
    {
        return Orders;
    }

    private readonly CheckCredit _check = new();

    [HttpPost("")]
    public string CreateOrder(int userId)
    {
        Console.WriteLine(userId);
        _check.Produce(userId);
        
        bool? hasCredit = CheckCreditResponse.GetCreditCheckResult(userId);

        if (hasCredit == null)
        {
            return "Credit check result not available yet.";
        }
        
        if (!hasCredit.Value) return "Order cancelled: You don't have enough credit.";
        
        Orders.Add(new Order(userId));
        return "Order placed successfully.";
    }
}