using System.Collections.Generic;
using System.Threading.Tasks;
using NexAsset.Web.Models.Mock;

namespace NexAsset.Web.Infrastructure.Services
{
    public interface IFinanceApiClient
    {
        Task<List<InvoiceMock>> GetInvoicesAsync();
        Task<InvoiceMock?> GetInvoiceAsync(string id);
    }
}
