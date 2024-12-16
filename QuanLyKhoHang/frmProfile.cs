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
    public partial class frmProfile : Form
    {
        DatabaseConnection db;

        public frmProfile()
        {
            InitializeComponent();
            db = new DatabaseConnection();
        }

        bool IsTextBoxEmpty(TextBox txt, Label lbl)
        {
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                MessageBox.Show($"Vui lòng nhập {lbl.Text}.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt.Focus();
                return true;
            }
            return false;
        }

        void ShowResult(int rowsAffected)
        {
            if (rowsAffected > 0)
            {
                MessageBox.Show("Đã thành công!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
                MessageBox.Show("Đã thất bại!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (IsTextBoxEmpty(txtName, lblName))
                return;
            if (IsTextBoxEmpty(txtPhone, lblPhone))
                return;
            if (IsTextBoxEmpty(txtAddress, lblAddress))
                return;

            string sql = $"UPDATE NhanVien SET TenNhanVien = N'{txtName.Text}', SoDienThoai = '{txtPhone.Text}', DiaChi = N'{txtAddress.Text}' WHERE MaNhanVien = {UserSession.EmployeeID}";
            ShowResult(db.GetNonQuery(sql));
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
                e.Handled = true;
        }
    }
}
