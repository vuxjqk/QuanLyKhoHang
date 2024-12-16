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
    public partial class frmPhieuNhap : Form
    {
        DatabaseConnection db;

        public frmPhieuNhap()
        {
            InitializeComponent();
            db = new DatabaseConnection();
        }

        void UpdateStockQuantity(int id)
        {
            string sql = $"SELECT * FROM ChiTietPhieuNhap WHERE MaPhieuNhap = {id}";
            DataTable dt = db.GetDataTable(sql);

            foreach (DataRow dr in dt.Rows)
            {
                sql = $"UPDATE SanPham SET SoLuongTon = SoLuongTon - {dr[3]} WHERE MaSanPham = {dr[1]}";
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
                @"SELECT MaPhieuNhap, TenNhaCungCap, CONCAT(NhanVien.MaNhanVien, ' - ', TenNhanVien) NhanVien,
	                NgayNhap, TongTien, TrangThai FROM PhieuNhap
                JOIN NhaCungCap ON NhaCungCap.MaNhaCungCap = PhieuNhap.MaNhaCungCap
                JOIN NhanVien ON NhanVien.MaNhanVien = PhieuNhap.MaNhanVien";
            dgv.DataSource = db.GetDataTable(sql);
        }

        private void frmPhieuNhap_Load(object sender, EventArgs e)
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
            frmNhapHang frm = new frmNhapHang();
            if (frm.ShowDialog() == DialogResult.OK)
                dgv_Load();
        }

        private void detailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = dgv.CurrentCell.RowIndex;
            frmPhieuNhapDetails frm = new frmPhieuNhapDetails((int)dgv.Rows[i].Cells[0].Value);
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
                string sql = $"UPDATE PhieuNhap SET TrangThai = N'Đã huỷ' WHERE MaPhieuNhap = {dgv.Rows[i].Cells[0].Value}";
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

                    sql = $"UPDATE PhieuNhap SET TrangThai = N'{status}' WHERE MaPhieuNhap = {dgv.Rows[i].Cells[0].Value}";
                    ShowResult(db.GetNonQuery(sql));
                }
            }
        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = dgv.CurrentCell.RowIndex;
            //frmPhieuNhapReport frm = new frmPhieuNhapReport((int)dgv.Rows[i].Cells[0].Value);
            //frm.ShowDialog();
        }
    }
}
