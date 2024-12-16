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
    public partial class frmPhieuNhapDetails : Form
    {
        DatabaseConnection db;
        int id;

        public frmPhieuNhapDetails(int id)
        {
            InitializeComponent();
            db = new DatabaseConnection();
            this.id = id;
        }

        void dgv_Load()
        {
            string sql =
                $@"SELECT SanPham.MaSanPham, TenSanPham, DonGia, SoLuong,
	                (DonGia*SoLuong) ThanhTien FROM ChiTietPhieuNhap
                JOIN SanPham ON SanPham.MaSanPham = ChiTietPhieuNhap.MaSanPham
                WHERE MaPhieuNhap = {id}";
            dgv.DataSource = db.GetDataTable(sql);
        }

        private void frmPhieuNhapDetails_Load(object sender, EventArgs e)
        {
            dgv_Load();
        }
    }
}
