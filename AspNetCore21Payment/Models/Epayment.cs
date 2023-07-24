using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore21Payment.Models
{
    public class Epayment
    {
        #region سازنده پیش فرض
        public Epayment()
        {
            InsertDatetime = System.DateTime.Now;
        }
        #endregion

        #region پراپرتی ها

        [Key]
        [Display(Name = "آی دی جدول")]
        public int PaymentId { get; set; }

        [Display(Name = "شماره مرجع")]
        [MaxLength(100)]
        //We Use RetrivalRefNo As Authority For Zarinpal bank
        public string RetrivalRefNo { get; set; }

        [Display(Name = "شماره پیگیری")]
        //We Use SystemTraceNo As TransId For Pay bank
        //We Use SystemTraceNo As RefId For Zarinpal bank
        public string SystemTraceNo { get; set; }

        [StringLength(150)]
        [Display(Name = "وضعیت پرداخت بانک ملی")]
        //We Use ResCode As ErrorCode For pay bank
        //We Use ResCode As Status For Zarinpal bank
        public string ResCode { get; set; }

        [StringLength(150)]
        [Display(Name = "توکن")]
        public string Token { get; set; }

        [StringLength(150)]
        [Display(Name = "توضیح")]
        //We Use Description As ErrorMessage For Pay bank 
        public string Description { get; set; }

        ////We Use Rrn As referenceNumber For Pasargad bank
        public long Rrn { get; set; }


        // فقط در صورتی که این فید ترو باشد پرداخت موفق بوده است
        [Display(Name = "وضعیت پرداخت نهایی")]
        public bool PaymentFinished { get; set; }

        [Display(Name = "مبلغ")]
        public long Amount { get; set; }

        [Display(Name = "نام بانک")]
        [MaxLength(50)]
        public string BankName { get; set; }

        [Display(Name = "آی دی کاربر")]
        public int UserId { get; set; }

        [Display(Name = "تاریخ خرید")]
        public DateTime InsertDatetime { get; set; }
        #endregion
    }
}
