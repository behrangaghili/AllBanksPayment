using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetCore21Payment.Models.Melli
{
    /// <summary>
    /// ویو مدل برای دیافت اطلاعات تایید پرداخت
    /// </summary>
    public class VerifyResultData
    {
        public string ResCode { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string RetrivalRefNo { get; set; }
        public string SystemTraceNo { get; set; }
        public string OrderId { get; set; }
    }
}