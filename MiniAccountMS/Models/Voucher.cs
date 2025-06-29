namespace MiniAccountMS.Models
{
    public class Voucher
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public DateTime Date { get; set; }

        public string ReferenceNo { get; set; }

        public string CreatedBy { get; set; }

        public List<VoucherItem> Items { get; set; } = new List<VoucherItem>();
    }
}
