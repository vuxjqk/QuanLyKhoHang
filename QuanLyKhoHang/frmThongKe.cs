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
    public partial class frmThongKe : Form
    {
        DatabaseConnection db;

        public frmThongKe()
        {
            InitializeComponent();
            db = new DatabaseConnection();
        }

        private void frmThongKe_Load(object sender, EventArgs e)
        {
            cboPhieu.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dtpFrom.Value > dtpTo.Value)
            {
                MessageBox.Show("Vui lòng chọn thời gian hợp lệ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql;
            if (cboPhieu.SelectedIndex == 0)
            {
                sql =
                $@"SELECT MaPhieuNhap, TenNhaCungCap, CONCAT(NhanVien.MaNhanVien, ' - ', TenNhanVien) NhanVien,
	                NgayNhap, TongTien, TrangThai FROM PhieuNhap
                    JOIN NhaCungCap ON NhaCungCap.MaNhaCungCap = PhieuNhap.MaNhaCungCap
                    JOIN NhanVien ON NhanVien.MaNhanVien = PhieuNhap.MaNhanVien
				    WHERE NgayNhap BETWEEN '{dtpFrom.Value}' AND '{dtpTo.Value}'";
            }
            else
            {
                sql =
                $@"SELECT MaPhieuXuat, CONCAT(KhachHang.SoDienThoai, ' - ', TenKhachHang) KhachHang,
	                CONCAT(NhanVien.MaNhanVien, ' - ', TenNhanVien) NhanVien,
	                NgayXuat, TongTien, TrangThai FROM PhieuXuat
                    JOIN KhachHang ON KhachHang.MaKhachHang = PhieuXuat.MaKhachHang
                    JOIN NhanVien ON NhanVien.MaNhanVien = PhieuXuat.MaNhanVien
				    WHERE NgayXuat BETWEEN '{dtpFrom.Value}' AND '{dtpTo.Value}'";
            }
            dgv.DataSource = db.GetDataTable(sql);

            if (cboPhieu.SelectedIndex == 0)
                sql = $"SELECT SUM(TongTien) FROM dbo.PhieuNhap WHERE NgayNhap BETWEEN '{dtpFrom.Value}' AND '{dtpTo.Value}'";
            else
                sql = $"SELECT SUM(TongTien) FROM dbo.PhieuXuat WHERE NgayXuat BETWEEN '{dtpFrom.Value}' AND '{dtpTo.Value}'";
            lblTotal.Text = ((decimal)db.GetScalar(sql)).ToString("N0");
        }
    }
}
