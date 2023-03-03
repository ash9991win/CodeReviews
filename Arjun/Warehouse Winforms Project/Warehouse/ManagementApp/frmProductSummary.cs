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
    public partial class frmProductSummary : Form
    {
        public frmProductSummary()
        {
            InitializeComponent();
        }

        Product productNew = null;
        internal Product ShowSummary(Product product)
        {
            productNew = product;
            DisplayAllProducts();
            ShowDialog();
            return productNew;
        }

        private void DisplayAllProducts()
        {
            lstPickedProducts.Items.Clear();
            foreach (var element in productNew.Summary)
            {
                lstPickedProducts.Items.Add(element);
            }
            lblProductCount.Text = productNew.Count.ToString();
            lblTotalCost.Text = productNew.TotalCost.ToString("c");
        }
    }
}
