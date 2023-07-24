using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore21Payment.Models
{
   public interface IEpaymentRepository
    {
        bool Add(Epayment epayment);
        bool Update(Epayment epayment);
        Epayment Find(int id);
    }
}
