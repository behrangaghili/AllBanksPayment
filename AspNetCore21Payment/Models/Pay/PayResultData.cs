using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetCore21Payment.Models.Pay
{
    /// <summary>
    /// ویو مدل برای دریافت نتیجه درخواست پرداخت
    /// </summary>
    public class PayResultData
    {
        public int Status { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string TransId { get; set; }
        public int Amount { get; set; }
    }
}