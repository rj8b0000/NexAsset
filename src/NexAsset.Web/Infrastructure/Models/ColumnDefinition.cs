using System;
using Microsoft.AspNetCore.Components;

namespace NexAsset.Web.Infrastructure.Models
{
    public class ColumnDefinition<TItem>
    {
        public string Header { get; set; } = "";
        public Func<TItem, string>? PropertySelector { get; set; }
        public RenderFragment<TItem>? Template { get; set; }
        public bool Sortable { get; set; } = true;
        public bool Visible { get; set; } = true;
        public string SortColumn { get; set; } = "";
    }
}
