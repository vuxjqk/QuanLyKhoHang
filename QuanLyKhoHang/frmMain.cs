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
    public partial class frmMain : Form
    {
        frmLogin frm;

        public frmMain(frmLogin frm)
        {
            InitializeComponent();
            this.frm = frm;
        }

        public void ShowChildForm(Form childForm)
        {
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            pnl.Controls.Clear();
            pnl.Controls.Add(childForm);
            childForm.Show();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!frm.Visible)
            {
                frm.Close();
                if (!frm.IsDisposed)
                    e.Cancel = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm.Show();
            this.Close();
        }

        private void updatePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUpdatePassword frm = new frmUpdatePassword();
            ShowChildForm(frm);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            pnl.BackgroundImage = Image.FromFile("../../Images/background.jpg");
            if (UserSession.Role == 1)
            {
                supplierToolStripMenuItem.Visible
                    = PhieuNhapToolStripMenuItem.Visible
                    = nhapHangToolStripMenuItem.Visible
                    = true;
                Text = "Nhân viên nhập hàng";
            }
            else if (UserSession.Role == 2)
            {
                customerToolStripMenuItem.Visible
                    = PhieuXuatToolStripMenuItem.Visible
                    = xuatHangToolStripMenuItem.Visible
                    = true;
                Text = "Nhân viên xuất hàng";
            }
            else if (UserSession.Role == 3)
            {
                productToolStripMenuItem.Visible = true;
                Text = "Nhân viên quản lý kho";
            }
            else if (UserSession.Role == 4)
            {
                thongKeToolStripMenuItem.Visible = true;
                Text = "Nhân viên kế toán";
            }
            else
            {
                supplierToolStripMenuItem.Visible
                    = PhieuNhapToolStripMenuItem.Visible
                    = nhapHangToolStripMenuItem.Visible
                    = customerToolStripMenuItem.Visible
                    = PhieuXuatToolStripMenuItem.Visible
                    = xuatHangToolStripMenuItem.Visible
                    = productToolStripMenuItem.Visible
                    = thongKeToolStripMenuItem.Visible
                    = employeeToolStripMenuItem.Visible
                    = accountToolStripMenuItem.Visible
                    = true;
                Text = "Quản trị viên";
            }
        }

        private void productToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProduct frm = new frmProduct();
            ShowChildForm(frm);
        }

        private void supplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSupplier frm = new frmSupplier();
            ShowChildForm(frm);
        }

        private void PhieuNhapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPhieuNhap frm = new frmPhieuNhap();
            ShowChildForm(frm);
        }

        private void customerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCustomer frm = new frmCustomer();
            ShowChildForm(frm);
        }

        private void PhieuXuatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPhieuXuat frm = new frmPhieuXuat();
            ShowChildForm(frm);
        }

        private void employeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEmployee frm = new frmEmployee();
            ShowChildForm(frm);
        }

        private void accountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccount frm = new frmAccount();
            ShowChildForm(frm);
        }

        private void nhapHangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNhapHang frm = new frmNhapHang();
            ShowChildForm(frm);
        }

        private void xuatHangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmXuatHang frm = new frmXuatHang();
            ShowChildForm(frm);
        }

        private void thongKeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmThongKe frm = new frmThongKe();
            ShowChildForm(frm);
        }

        private void profileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProfile frm = new frmProfile();
            ShowChildForm(frm);
        }
    }
}
