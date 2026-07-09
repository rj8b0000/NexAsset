using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.Models;
using NexAsset.Web.State;

namespace NexAsset.Web.Infrastructure.Services
{
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
