using MiniAccountMS.Data;
using MiniAccountMS.Models;
using System.Data;
using System.Data.SqlClient;

namespace MiniAccountMS.Services
{
    public class VoucharService
    {
        private readonly ApplicationDbContext _context;

        public VoucharService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Voucher>> GetAllAsync()
        {
            var vouchers = new List<Voucher>();

            using (var conn = _context.GetConnection())
            {
                var cmd = new SqlCommand("SELECT * FROM Vouchers", conn);
                await conn.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    vouchers.Add(new Voucher
                    {
                        Id = (int)reader["Id"],
                        Type = reader["Type"].ToString(),
                        Date = (DateTime)reader["Date"],
                        ReferenceNo = reader["ReferenceNo"].ToString(),
                        CreatedBy = reader["CreatedBy"].ToString()
                    });
                }
            }

            return vouchers;
        }

        public async Task<Voucher> GetByIdAsync(int id)
        {
            Voucher voucher = null;

            using (var conn = _context.GetConnection())
            {
                var cmd = new SqlCommand("SELECT * FROM Vouchers WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                await conn.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    voucher = new Voucher
                    {
                        Id = (int)reader["Id"],
                        Type = reader["Type"].ToString(),
                        Date = (DateTime)reader["Date"],
                        ReferenceNo = reader["ReferenceNo"].ToString(),
                        CreatedBy = reader["CreatedBy"].ToString()
                    };
                }
                reader.Close();

                // Load Voucher Items
                cmd = new SqlCommand("SELECT * FROM VoucherItems WHERE VoucherId = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                reader = await cmd.ExecuteReaderAsync();
                voucher.Items = new List<VoucherItem>();

                while (await reader.ReadAsync())
                {
                    voucher.Items.Add(new VoucherItem
                    {
                        Id = (int)reader["Id"],
                        AccountId = (int)reader["AccountId"],
                        Debit = (decimal)reader["Debit"],
                        Credit = (decimal)reader["Credit"],
                        Description = reader["Description"].ToString()
                    });
                }
            }

            return voucher;
        }

        public async Task CreateAsync(Voucher voucher)
        {
            using (var conn = _context.GetConnection())
            {
                await conn.OpenAsync();

                // Prepare Table-Valued Parameter
                var dt = new DataTable();
                dt.Columns.Add("AccountId", typeof(int));
                dt.Columns.Add("Debit", typeof(decimal));
                dt.Columns.Add("Credit", typeof(decimal));
                dt.Columns.Add("Description", typeof(string));

                foreach (var item in voucher.Items)
                {
                    dt.Rows.Add(item.AccountId, item.Debit, item.Credit, item.Description);
                }

                var cmd = new SqlCommand("sp_SaveVoucher", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Type", voucher.Type);
                cmd.Parameters.AddWithValue("@Date", voucher.Date);
                cmd.Parameters.AddWithValue("@ReferenceNo", voucher.ReferenceNo);
                cmd.Parameters.AddWithValue("@CreatedBy", voucher.CreatedBy);

                var tvp = cmd.Parameters.AddWithValue("@VoucherItems", dt);
                tvp.SqlDbType = SqlDbType.Structured;
                tvp.TypeName = "dbo.TVP_VoucherItem";

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var conn = _context.GetConnection())
            {
                var cmd = new SqlCommand("DELETE FROM Vouchers WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
