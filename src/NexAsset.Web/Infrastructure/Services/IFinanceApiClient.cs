using System.Collections.Generic;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.Models;

namespace NexAsset.Web.Infrastructure.Services
{
    public interface IFinanceApiClient
    {
        Task<List<InvoiceMock>> GetInvoicesAsync();
        Task<InvoiceMock?> GetInvoiceAsync(string id);
    }
}
