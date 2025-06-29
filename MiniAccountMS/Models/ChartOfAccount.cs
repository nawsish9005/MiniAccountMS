namespace MiniAccountMS.Models
{
    public class ChartOfAccount
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }
        public string ParentName { get; set; }
    }
}
