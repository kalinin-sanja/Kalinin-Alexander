namespace PhonebookApi.Models
{
    public class Filter
    {
        public string Query { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }

        public Filter()
        {
            Limit = 50;
        }
    }
}