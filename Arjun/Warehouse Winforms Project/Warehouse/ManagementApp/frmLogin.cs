using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementApp
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        Employee emp = new Employee();

        private void btnEnter_Click(object sender, EventArgs e)
        {
            ValidateEmployeeCredentials();
        }

        private void ValidateEmployeeCredentials()
        {
            if (!PasswordCheck())
            {
                return;
            }

            frmPickProduct pickProduct = new frmPickProduct();
            pickProduct.AccessPickProduct(emp);
            Close();

        }

        private bool PasswordCheck()
        {

            emp.EmployeeId = txtEmpId.Text;
            emp.Password = txtPassword.Text;
            if (emp.EmployeeId == "abcd" && emp.Password == "1234")
            {
                return true;
            }
            return false;
        }
    }
}
