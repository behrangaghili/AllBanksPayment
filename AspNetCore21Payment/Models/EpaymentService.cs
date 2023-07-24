using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore21Payment.Models
{
    public class EpaymentService : IEpaymentRepository
    {
        private readonly ApplicationDbContext _ctx;
        public EpaymentService(ApplicationDbContext ctx)
        {
            this._ctx = ctx;
        }
        public bool Add(Epayment epayment)
        {
            _ctx.Epayments.Add(epayment);

            return Convert.ToBoolean(_ctx.SaveChanges());
        }

        public Epayment Find(int id)
        {
            return _ctx.Epayments.Find(id);
        }

        public bool Update(Epayment epayment)
        {
            _ctx.Epayments.Update(epayment);

            return Convert.ToBoolean(_ctx.SaveChanges());
        }
    }
}
