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
    public partial class frmUpdatePassword : Form
    {
        DatabaseConnection db;

        public frmUpdatePassword()
        {
            InitializeComponent();
            db = new DatabaseConnection();
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

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            string sql = $"SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = '{UserSession.UserName}' AND MatKhau = '{txtPassword.Text}'";
            if ((int)db.GetScalar(sql) == 1)
                txtNewPassword.Enabled
                    = txtConfirm.Enabled
                    = true;
            else
                txtNewPassword.Enabled
                    = txtConfirm.Enabled
                    = false;
        }

        private void txtNewPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtNewPassword.Text.Length == 0 || txtNewPassword.Text != txtConfirm.Text)
                btnUpdate.Enabled = false;
            else
                btnUpdate.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string sql = $"UPDATE TaiKhoan SET MatKhau = '{txtConfirm.Text}' WHERE TenDangNhap = '{UserSession.UserName}'";
            ShowResult(db.GetNonQuery(sql));
        }
    }
}
