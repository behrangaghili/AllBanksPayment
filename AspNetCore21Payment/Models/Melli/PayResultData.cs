using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetCore21Payment.Models.Melli
{
    /// <summary>
    /// ویو مدل برای دریافت نتیجه درخواست پرداخت
    /// </summary>
    public class PayResultData
    {
        public string ResCode { get; set; }
        public string Token { get; set; }
        public string Description { get; set; }
    }
}