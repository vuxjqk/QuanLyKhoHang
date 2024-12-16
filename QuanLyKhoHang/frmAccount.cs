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
    public partial class frmAccount : Form
    {
        DatabaseConnection db;

        public frmAccount()
        {
            InitializeComponent();
            db = new DatabaseConnection();
        }

        bool IsTextBoxEmpty(TextBox txt)
        {
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                MessageBox.Show($"Vui lòng chọn một {lblTitle.Text}.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            return false;
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
                dgv_Load();
            }
            else
                MessageBox.Show("Đã thất bại!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void dgv_Load()
        {
            string sql = "SELECT * FROM TaiKhoan";
            dgv.DataSource = db.GetDataTable(sql);
        }

        void cboID_Load()
        {
            string sql = "SELECT *, CONCAT(MaNhanVien, ' - ', TenNhanVien) NhanVien FROM NhanVien";
            cboID.DataSource = db.GetDataTable(sql);
            cboID.ValueMember = "MaNhanVien";
            cboID.DisplayMember = "NhanVien";
        }

        private void frmAccount_Load(object sender, EventArgs e)
        {
            dgv_Load();
            cboID_Load();
            cboRole.SelectedIndex = 0;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string sql = $"SELECT * FROM TaiKhoan WHERE TenDangNhap LIKE '%{txtSearch.Text}%'";
            dgv.DataSource = db.GetDataTable(sql);
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dgv.CurrentCell.RowIndex;
            txtUserName.Text = dgv.Rows[i].Cells[0].Value.ToString();
            txtPassword.Text = dgv.Rows[i].Cells[1].Value.ToString();
            cboRole.SelectedIndex = (int)dgv.Rows[i].Cells[2].Value;
            cboID.SelectedValue = dgv.Rows[i].Cells[3].Value;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "&Thêm")
            {
                btnAdd.Text = "&Huỷ";

                txtUserName.Enabled
                    = txtPassword.Enabled
                    = cboRole.Enabled
                    = cboID.Enabled
                    = btnSave.Enabled
                    = true;

                btnEdit.Enabled
                    = btnDelete.Enabled
                    = false;

                dgv.CellClick -= dgv_CellClick;
            }
            else
            {
                btnAdd.Text = "&Thêm";

                txtUserName.Enabled
                    = txtPassword.Enabled
                    = cboRole.Enabled
                    = cboID.Enabled
                    = btnSave.Enabled
                    = false;

                btnEdit.Enabled
                    = btnDelete.Enabled
                    = true;

                dgv.CellClick += dgv_CellClick;
            }
            txtUserName.Clear();
            txtPassword.Clear();
            cboRole.SelectedIndex = 0;
            cboID.SelectedIndex = 0;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (IsTextBoxEmpty(txtUserName))
                return;

            if (btnEdit.Text == "&Sửa")
            {
                btnEdit.Text = "&Huỷ";

                txtPassword.Enabled
                    = cboRole.Enabled
                    = cboID.Enabled
                    = btnSave.Enabled
                    = true;

                btnAdd.Enabled
                    = btnDelete.Enabled
                    = false;
            }
            else
            {
                btnEdit.Text = "&Sửa";

                txtPassword.Enabled
                    = cboRole.Enabled
                    = cboID.Enabled
                    = btnSave.Enabled
                    = false;

                btnAdd.Enabled
                    = btnDelete.Enabled
                    = true;

                txtUserName.Clear();
                txtPassword.Clear();
                cboRole.SelectedIndex = 0;
                cboID.SelectedIndex = 0;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (IsTextBoxEmpty(txtUserName))
                return;

            DialogResult r = MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                string sql = $"DELETE FROM TaiKhoan WHERE TenDangNhap = '{txtUserName.Text}'";
                ShowResult(db.GetNonQuery(sql));
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsTextBoxEmpty(txtUserName, lblUserName))
                return;
            if (IsTextBoxEmpty(txtPassword, lblPassword))
                return;

            string sql;
            if (btnAdd.Text == "&Huỷ")
            {
                sql = $"INSERT INTO TaiKhoan VALUES ('{txtUserName.Text}', '{txtPassword.Text}', {cboRole.SelectedIndex}, {cboID.SelectedValue})";
                btnAdd_Click(sender, e);
            }
            else
            {
                sql = $"UPDATE TaiKhoan SET MatKhau = '{txtPassword.Text}', VaiTro = {cboRole.SelectedIndex}, MaNhanVien = {cboID.SelectedValue} WHERE TenDangNhap = '{txtUserName.Text}'";
                btnEdit_Click(sender, e);
            }
            ShowResult(db.GetNonQuery(sql));
        }
    }
}
