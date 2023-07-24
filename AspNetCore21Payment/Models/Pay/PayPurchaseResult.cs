using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetCore21Payment.Models.Pay
{
    /// <summary>
    /// ویو مدل برای دریافت اطلاعات ارسالی توسط بانک هنگام بازگشت 
    /// </summary>
    public class PayPurchaseResult
    {
        public int? Status { get; set; }
        public int? TransId { get; set; }
        public int? Mobile { get; set; }
        public string FactorNumber { get; set; }
        public string CardNumber { get; set; }
        public string Message { get; set; }
    }
}