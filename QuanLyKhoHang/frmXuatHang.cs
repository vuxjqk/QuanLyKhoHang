using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhoHang
{
    public partial class frmXuatHang : Form
    {
        DatabaseConnection db;

        public frmXuatHang()
        {
            InitializeComponent();
            db = new DatabaseConnection();
        }

        void UpdateStockQuantity(int id, int quantity)
        {
            string sql = $"UPDATE SanPham SET SoLuongTon = SoLuongTon - {quantity} WHERE MaSanPham = {id}";
            db.GetNonQuery(sql);
        }

        void ShowResult(int rowsAffected)
        {
            if (rowsAffected > 0)
            {
                MessageBox.Show("Đã thành công!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                XuatHang.items.Clear();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
                MessageBox.Show("Đã thất bại!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void GetCart()
        {
            DataTable dt = new DataTable()
            {
                Columns =
                {
                    new DataColumn("MaSanPham", typeof(int)),
                    new DataColumn("TenSanPham", typeof(string)),
                    new DataColumn("DonGia", typeof(decimal)),
                    new DataColumn("SoLuong", typeof(int)),
                    new DataColumn("ThanhTien", typeof(decimal))
                }
            };

            foreach (Item item in XuatHang.items)
            {
                DataRow dr = dt.NewRow();
                dr[0] = item.ProductID;
                dr[1] = item.ProductName;
                dr[2] = item.UnitPrice;
                dr[3] = item.Quantity;
                dr[4] = item.Amount;
                dt.Rows.Add(dr);
            }
            dgv.DataSource = dt;
            txtTotal.Text = XuatHang.TotalAmount().ToString();
        }

        void lst_Load()
        {
            lst.SelectedIndexChanged -= lst_SelectedIndexChanged;
            string sql = "SELECT * FROM SanPham";
            lst.DataSource = db.GetDataTable(sql);
            lst.ValueMember = "MaSanPham";
            lst.DisplayMember = "TenSanPham";
            lst.SelectedIndexChanged += lst_SelectedIndexChanged;
        }

        void cboCustomer_Load()
        {
            string sql = "SELECT *, CONCAT(SoDienThoai, ' - ', TenKhachHang) KhachHang FROM KhachHang";
            cboCustomer.DataSource = db.GetDataTable(sql);
            cboCustomer.ValueMember = "MaKhachHang";
            cboCustomer.DisplayMember = "KhachHang";
        }

        private void frmXuatHang_Load(object sender, EventArgs e)
        {
            GetCart();
            lst_Load();
            cboCustomer_Load();
            lst_SelectedIndexChanged(sender, e);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string sql = $"SELECT * FROM SanPham WHERE TenSanPham LIKE N'%{txtSearch.Text}%'";
            lst.DataSource = db.GetDataTable(sql);
            lst.ValueMember = "MaSanPham";
            lst.DisplayMember = "TenSanPham";
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dgv.CurrentCell.RowIndex;
            if (i >= 0)
            {
                lst.SelectedValue = dgv.Rows[i].Cells[0].Value;
                txtPrice.Text = dgv.Rows[i].Cells[2].Value.ToString();
                nud.Value = (int)dgv.Rows[i].Cells[3].Value;
            }
        }

        private void dgv_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dgv.ClearSelection();
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                cms.Show(dgv, dgv.PointToClient(Cursor.Position));
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string sql = $"SELECT * FROM SanPham WHERE MaSanPham = {lst.SelectedValue}";
            DataRow dr = db.GetDataTable(sql).Rows[0];

            XuatHang.AddItem((int)lst.SelectedValue, dr[1].ToString(),
                decimal.Parse(txtPrice.Text), (int)nud.Value);
            GetCart();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = dgv.CurrentCell.RowIndex;
            XuatHang.RemoveItem((int)dgv.Rows[i].Cells[0].Value);
            GetCart();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (XuatHang.TotalQuantity() == 0)
            {
                MessageBox.Show("Giỏ hàng rỗng.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = $"INSERT INTO PhieuXuat OUTPUT Inserted.MaPhieuXuat VALUES ({cboCustomer.SelectedValue}, {UserSession.EmployeeID}, GETDATE(), {txtTotal.Text}, N'Chờ xử lý')";
            int id = (int)db.GetScalar(sql);

            sql = "SELECT * FROM ChiTietPhieuXuat";
            DataTable dt = db.GetDataTable(sql);
            foreach (Item item in XuatHang.items)
            {
                DataRow dr = dt.NewRow();
                dr[0] = id;
                dr[1] = item.ProductID;
                dr[2] = item.UnitPrice;
                dr[3] = item.Quantity;
                dt.Rows.Add(dr);

                UpdateStockQuantity(item.ProductID, item.Quantity);
            }
            ShowResult(db.UpdateDataTable(sql, dt));
        }

        private void lst_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = $"SELECT * FROM SanPham WHERE MaSanPham = {lst.SelectedValue}";
            DataRow dr = db.GetDataTable(sql).Rows[0];
            txtPrice.Text = dr[3].ToString();
        }
    }
}
