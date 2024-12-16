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
    public partial class frmProduct : Form
    {
        DatabaseConnection db;

        public frmProduct()
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
            string sql = "SELECT * FROM SanPham";
            dgv.DataSource = db.GetDataTable(sql);
        }

        void cboCategory_Load()
        {
            string sql = "SELECT * FROM DanhMuc";
            cboCategory.DataSource = db.GetDataTable(sql);
            cboCategory.ValueMember = "MaDanhMuc";
            cboCategory.DisplayMember = "TenDanhMuc";
        }

        void cboFilter_Load()
        {
            cboFilter.SelectedIndexChanged -= cboFilter_SelectedIndexChanged;

            string sql = "SELECT * FROM DanhMuc";
            DataTable dt = db.GetDataTable(sql);

            DataRow dr = dt.NewRow();
            dr[0] = DBNull.Value;
            dr[1] = "Tất cả";
            dt.Rows.InsertAt(dr, 0);

            cboFilter.DataSource = dt;
            cboFilter.ValueMember = "MaDanhMuc";
            cboFilter.DisplayMember = "TenDanhMuc";

            cboFilter.SelectedIndexChanged += cboFilter_SelectedIndexChanged;
        }

        private void frmProduct_Load(object sender, EventArgs e)
        {
            dgv_Load();
            cboCategory_Load();
            cboFilter_Load();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string sql = $"SELECT * FROM SanPham WHERE TenSanPham LIKE N'%{txtSearch.Text}%'";
            dgv.DataSource = db.GetDataTable(sql);
        }

        private void cboFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFilter.SelectedValue == DBNull.Value)
                dgv_Load();
            else
            {
                string sql = $"SELECT * FROM SanPham WHERE MaDanhMuc = {cboFilter.SelectedValue}";
                dgv.DataSource = db.GetDataTable(sql);
            }
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dgv.CurrentCell.RowIndex;
            txtID.Text = dgv.Rows[i].Cells[0].Value.ToString();
            txtName.Text = dgv.Rows[i].Cells[1].Value.ToString();
            cboCategory.SelectedValue = dgv.Rows[i].Cells[2].Value;
            txtPrice.Text = dgv.Rows[i].Cells[3].Value.ToString();
            txtUnit.Text = dgv.Rows[i].Cells[5].Value.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "&Thêm")
            {
                btnAdd.Text = "&Huỷ";

                txtName.Enabled
                    = cboCategory.Enabled
                    = txtPrice.Enabled
                    = txtUnit.Enabled
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
                    = cboCategory.Enabled
                    = txtPrice.Enabled
                    = txtUnit.Enabled
                    = btnSave.Enabled
                    = false;

                btnEdit.Enabled
                    = btnDelete.Enabled
                    = true;

                dgv.CellClick += dgv_CellClick;
            }
            txtID.Clear();
            txtName.Clear();
            cboCategory.SelectedIndex = 0;
            txtPrice.Clear();
            txtUnit.Clear();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (IsTextBoxEmpty(txtID))
                return;

            if (btnEdit.Text == "&Sửa")
            {
                btnEdit.Text = "&Huỷ";

                txtName.Enabled
                    = cboCategory.Enabled
                    = txtPrice.Enabled
                    = txtUnit.Enabled
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
                    = cboCategory.Enabled
                    = txtPrice.Enabled
                    = txtUnit.Enabled
                    = btnSave.Enabled
                    = false;

                btnAdd.Enabled
                    = btnDelete.Enabled
                    = true;

                txtID.Clear();
                txtName.Clear();
                cboCategory.SelectedIndex = 0;
                txtPrice.Clear();
                txtUnit.Clear();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (IsTextBoxEmpty(txtID))
                return;

            DialogResult r = MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                string sql = $"DELETE FROM SanPham WHERE MaSanPham = {txtID.Text}";
                ShowResult(db.GetNonQuery(sql));
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsTextBoxEmpty(txtName, lblName))
                return;
            if (IsTextBoxEmpty(txtPrice, lblPrice))
                return;
            if (IsTextBoxEmpty(txtUnit, lblUnit))
                return;

            string sql;
            if (btnAdd.Text == "&Huỷ")
            {
                sql = $"INSERT INTO SanPham VALUES (N'{txtName.Text}', {cboCategory.SelectedValue}, {txtPrice.Text}, 0, N'{txtUnit.Text}')";
                btnAdd_Click(sender, e);
            }
            else
            {
                sql = $"UPDATE SanPham SET TenSanPham = N'{txtName.Text}', MaDanhMuc = {cboCategory.SelectedValue}, Gia = {txtPrice.Text}, DonVi = N'{txtUnit.Text}' WHERE MaSanPham = {txtID.Text}";
                btnEdit_Click(sender, e);
            }
            ShowResult(db.GetNonQuery(sql));
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
                e.Handled = true;
        }
    }
}
