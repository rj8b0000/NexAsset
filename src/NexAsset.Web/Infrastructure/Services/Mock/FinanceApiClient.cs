using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexAsset.Web.Models.Mock;
using NexAsset.Web.Infrastructure.Services;
using NexAsset.Web.State;

namespace NexAsset.Web.Infrastructure.Services.Mock
{
    /// <summary>
    /// In-memory implementation of <see cref="IFinanceApiClient"/> backed by <see cref="MockDatabaseService"/>.
    /// Temporary placeholder pending migration to the real NexAsset.API HTTP client.
    /// </summary>
    public class FinanceApiClient : IFinanceApiClient
    {
        private readonly MockDatabaseService _db;
        private readonly NotificationState _notifications;

        public FinanceApiClient(MockDatabaseService db, NotificationState notifications)
        {
            _db = db;
            _notifications = notifications;
        }

        public Task<List<InvoiceMock>> GetInvoicesAsync()
        {
            return Task.FromResult(_db.Invoices.ToList());
        }

        public Task<InvoiceMock?> GetInvoiceAsync(string id)
        {
            return Task.FromResult(_db.Invoices.FirstOrDefault(i => i.Id == id));
        }
    }
}
