using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetCore21Payment.Models.Melli
{
    /// <summary>
    /// ویو مدل برای دریافت اطلاعات ارسالی توسط بانک هنگام بازگشت 
    /// </summary>
    public class PurchaseResult
    {
        public string OrderId { get; set; }
        public string Token { get; set; }
        public string ResCode { get; set; }
    }
}