namespace TaskForAdditionalPoints
{
    public class OrderStageEventArgs : EventArgs
    {
        public string OrderId { get; }
        public string StageName { get; }

        public OrderStageEventArgs(string orderId, string stageName)
        {
            OrderId = orderId;
            StageName = stageName;
        }
    }
}