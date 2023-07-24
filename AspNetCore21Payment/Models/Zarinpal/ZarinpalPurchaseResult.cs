using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetCore21Payment.Models.Zarinpal
{
    /// <summary>
    /// ویو مدل برای دریافت اطلاعات ارسالی توسط بانک هنگام بازگشت 
    /// </summary>
    public class ZarinpalPurchaseResult
    {
        public string OrderId { get; set; }
        public string Authority { get; set; }
        public string Status { get; set; }
    }
}