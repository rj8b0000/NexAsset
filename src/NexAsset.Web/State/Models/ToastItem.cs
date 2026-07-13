namespace NexAsset.Web.State
{
    public class ToastItem
    {
        public string Id { get; set; } = "";
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
        public ToastType Type { get; set; } = ToastType.Success;
    }
}
