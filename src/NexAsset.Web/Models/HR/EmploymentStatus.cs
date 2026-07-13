using System.Collections.Generic;

namespace NexAsset.Web.Models.HR
{
    /// <summary>
    /// Mirrors NexAsset.Domain.Enums.EmploymentStatus. The API (default System.Text.Json, no
    /// string-enum converter) serializes this as its integer value, so the frontend sends/reads ints.
    /// </summary>
    public static class EmploymentStatus
    {
        public const int Active = 1;
        public const int Probation = 2;
        public const int NoticePeriod = 3;
        public const int Resigned = 4;
        public const int Terminated = 5;

        public static readonly IReadOnlyList<(int Value, string Label)> Options = new List<(int, string)>
        {
            (Active, "Active"),
            (Probation, "Probation"),
            (NoticePeriod, "Notice Period"),
            (Resigned, "Resigned"),
            (Terminated, "Terminated"),
        };

        public static string Label(int value) => value switch
        {
            Active => "Active",
            Probation => "Probation",
            NoticePeriod => "Notice Period",
            Resigned => "Resigned",
            Terminated => "Terminated",
            _ => "Unknown"
        };
    }
}
