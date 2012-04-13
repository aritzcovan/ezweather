namespace ezweather.Messages
{
    public class RefreshMessage
    {
        public RefreshMessage(bool shouldrefresh)
        {
            ShouldRefresh = shouldrefresh;
        }

        public bool ShouldRefresh { get; set; }
    }
}