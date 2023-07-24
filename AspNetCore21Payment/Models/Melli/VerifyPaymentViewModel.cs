using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore21Payment.Models.Melli
{
    /// <summary>
    /// از این کلاس برای دریافت اطلاعات ارسالی از سمت درگاه استفاده می شود
    /// </summary>
    public class VerifyPaymentViewModel
    {
        // شماره فاکتور
        // paymentId
        public string In { get; set; }

        // تاریخ فاکتور
        //invoiceDate
        public string Id { get; set; }

        // شماره مرجع
        //TransactionReferenceId
        public string Tref { get; set; }
    }
}
