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
    public partial class frmLogin : Form
    {
        DatabaseConnection db;

        public frmLogin()
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (IsTextBoxEmpty(txtUserName, lblUserName))
                return;
            if (IsTextBoxEmpty(txtPassword, lblPassword))
                return;

            string sql = $"SELECT * FROM TaiKhoan WHERE TenDangNhap = '{txtUserName.Text}' AND MatKhau = '{txtPassword.Text}'";
            DataTable dt = db.GetDataTable(sql);
            if (dt.Rows.Count == 1)
            {
                txtUserName.Clear();
                txtPassword.Clear();
                lblError.Visible = false;
                MessageBox.Show("Đăng nhập thành công!\nXin chào bạn.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                UserSession.UserName = dt.Rows[0]["TenDangNhap"].ToString();
                UserSession.EmployeeID = (int)dt.Rows[0]["MaNhanVien"];
                UserSession.Role = (int)dt.Rows[0]["VaiTro"];

                frmMain frm = new frmMain(this);
                frm.Show();
                this.Hide();
            }
            else
                lblError.Visible = true;
            txtUserName.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDangNhap_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult r = MessageBox.Show("Bạn có chắc chắn thoát không?", "Question",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.No)
                e.Cancel = true;
        }
    }
}
