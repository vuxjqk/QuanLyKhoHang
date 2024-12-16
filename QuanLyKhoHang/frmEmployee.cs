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
    public partial class frmEmployee : Form
    {
        DatabaseConnection db;

        public frmEmployee()
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
            string sql = "SELECT * FROM NhanVien";
            dgv.DataSource = db.GetDataTable(sql);
        }

        private void frmEmployee_Load(object sender, EventArgs e)
        {
            dgv_Load();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string sql = $"SELECT * FROM NhanVien WHERE TenNhanVien LIKE N'%{txtSearch.Text}%'";
            dgv.DataSource = db.GetDataTable(sql);
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dgv.CurrentCell.RowIndex;
            txtID.Text = dgv.Rows[i].Cells[0].Value.ToString();
            txtName.Text = dgv.Rows[i].Cells[1].Value.ToString();
            txtPhone.Text = dgv.Rows[i].Cells[2].Value.ToString();
            txtAddress.Text = dgv.Rows[i].Cells[3].Value.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "&Thêm")
            {
                btnAdd.Text = "&Huỷ";

                txtName.Enabled
                    = txtPhone.Enabled
                    = txtAddress.Enabled
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

                txtName.Enabled
                    = txtPhone.Enabled
                    = txtAddress.Enabled
                    = btnSave.Enabled
                    = false;

                btnEdit.Enabled
                    = btnDelete.Enabled
                    = true;

                dgv.CellClick += dgv_CellClick;
            }
            txtID.Clear();
            txtName.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (IsTextBoxEmpty(txtID))
                return;

            if (btnEdit.Text == "&Sửa")
            {
                btnEdit.Text = "&Huỷ";

                txtName.Enabled
                    = txtPhone.Enabled
                    = txtAddress.Enabled
                    = btnSave.Enabled
                    = true;

                btnAdd.Enabled
                    = btnDelete.Enabled
                    = false;
            }
            else
            {
                btnEdit.Text = "&Sửa";

                txtName.Enabled
                    = txtPhone.Enabled
                    = txtAddress.Enabled
                    = btnSave.Enabled
                    = false;

                btnAdd.Enabled
                    = btnDelete.Enabled
                    = true;

                txtID.Clear();
                txtName.Clear();
                txtPhone.Clear();
                txtAddress.Clear();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (IsTextBoxEmpty(txtID))
                return;

            DialogResult r = MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                string sql = $"DELETE FROM NhanVien WHERE MaNhanVien = {txtID.Text}";
                ShowResult(db.GetNonQuery(sql));
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsTextBoxEmpty(txtName, lblName))
                return;
            if (IsTextBoxEmpty(txtPhone, lblPhone))
                return;
            if (IsTextBoxEmpty(txtAddress, lblAddress))
                return;

            string sql;
            if (btnAdd.Text == "&Huỷ")
            {
                sql = $"INSERT INTO NhanVien VALUES (N'{txtName.Text}', '{txtPhone.Text}', N'{txtAddress.Text}')";
                btnAdd_Click(sender, e);
            }
            else
            {
                sql = $"UPDATE NhanVien SET TenNhanVien = N'{txtName.Text}', SoDienThoai = '{txtPhone.Text}', DiaChi = N'{txtAddress.Text}' WHERE MaNhanVien = {txtID.Text}";
                btnEdit_Click(sender, e);
            }
            ShowResult(db.GetNonQuery(sql));
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
                e.Handled = true;
        }
    }
}
