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
    public partial class frmPhieuXuat : Form
    {
        DatabaseConnection db;

        public frmPhieuXuat()
        {
            InitializeComponent();
            db = new DatabaseConnection();
        }

        void UpdateStockQuantity(int id)
        {
            string sql = $"SELECT * FROM ChiTietPhieuXuat WHERE MaPhieuXuat = {id}";
            DataTable dt = db.GetDataTable(sql);

            foreach (DataRow dr in dt.Rows)
            {
                sql = $"UPDATE SanPham SET SoLuongTon = SoLuongTon + {dr[3]} WHERE MaSanPham = {dr[1]}";
                db.GetNonQuery(sql);
            }
        }

        void ShowResult(int rowsAffected)
        {
            if (rowsAffected > 0)
            {
                MessageBox.Show("Đã thành công!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgv_Load();
            }
            else
                MessageBox.Show("Đã thất bại!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void dgv_Load()
        {
            string sql =
                @"SELECT MaPhieuXuat, CONCAT(KhachHang.SoDienThoai, ' - ', TenKhachHang) KhachHang,
	                CONCAT(NhanVien.MaNhanVien, ' - ', TenNhanVien) NhanVien,
	                NgayXuat, TongTien, TrangThai FROM PhieuXuat
                JOIN KhachHang ON KhachHang.MaKhachHang = PhieuXuat.MaKhachHang
                JOIN NhanVien ON NhanVien.MaNhanVien = PhieuXuat.MaNhanVien";
            dgv.DataSource = db.GetDataTable(sql);
        }

        private void frmPhieuXuat_Load(object sender, EventArgs e)
        {
            dgv_Load();
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                cms.Show(dgv, dgv.PointToClient(Cursor.Position));
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmXuatHang frm = new frmXuatHang();
            if (frm.ShowDialog() == DialogResult.OK)
                dgv_Load();
        }

        private void detailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = dgv.CurrentCell.RowIndex;
            frmPhieuXuatDetails frm = new frmPhieuXuatDetails((int)dgv.Rows[i].Cells[0].Value);
            frm.ShowDialog();
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = dgv.CurrentCell.RowIndex;
            if (dgv.Rows[i].Cells[5].Value.ToString() != "Chờ xử lý")
            {
                MessageBox.Show("Không thể huỷ đơn hàng này!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult r = MessageBox.Show("Bạn có chắc chắn muốn huỷ không?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                UpdateStockQuantity((int)dgv.Rows[i].Cells[0].Value);
                string sql = $"UPDATE PhieuXuat SET TrangThai = N'Đã huỷ' WHERE MaPhieuXuat = {dgv.Rows[i].Cells[0].Value}";
                ShowResult(db.GetNonQuery(sql));
            }
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = dgv.CurrentCell.RowIndex;
            string status = dgv.Rows[i].Cells[5].Value.ToString(), sql;

            if (status == "Đã xử lý" || status == "Đã huỷ")
                return;
            else
            {
                DialogResult r = MessageBox.Show("Bạn có chắc chắn muốn cập nhật trạng thái không?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    if (status == "Chờ xử lý")
                        status = "Đang xử lý";
                    else
                        status = "Đã xử lý";

                    sql = $"UPDATE PhieuXuat SET TrangThai = N'{status}' WHERE MaPhieuXuat = {dgv.Rows[i].Cells[0].Value}";
                    ShowResult(db.GetNonQuery(sql));
                }
            }
        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = dgv.CurrentCell.RowIndex;
            //frmPhieuXuatReport frm = new frmPhieuXuatReport((int)dgv.Rows[i].Cells[0].Value);
            //frm.ShowDialog();
        }
    }
}
