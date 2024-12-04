namespace Database
{
    /// <summary>
    /// Sample Sales Data for test connection to clickhouse
    /// </summary>
    public class SalesItem
    {
        public string Country { get; set; }
        public string Company { get; set; }
        public string Region { get; set; }

        public decimal? Sales { get; set; }
    }
}
