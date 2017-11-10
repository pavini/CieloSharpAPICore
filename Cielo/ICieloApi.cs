using System;
using Cielo.Core.Models;

namespace Cielo.Core
{
    public interface ICieloApi
    {
        Transaction CreateTransaction(Guid requestId, Transaction transaction);
        Transaction GetTransaction(Guid requestId, Guid paymentId);
        ReturnStatus CancellationTransaction(Guid requestId, Guid paymentId, decimal? amount = null);
        ReturnStatus CaptureTransaction(Guid requestId, Guid paymentId, decimal? amount = null, decimal? serviceTaxAmount = null);
    }
}
