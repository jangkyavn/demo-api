namespace DemoAPI.Helpers
{
    public class PagingParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public bool? FilterGender { get; set; }
    }
}
