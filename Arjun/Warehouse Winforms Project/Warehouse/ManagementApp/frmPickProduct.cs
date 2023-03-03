using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarehouseManagement;

namespace ManagementApp
{
    public partial class frmPickProduct : Form
    {
        public frmPickProduct()
        {
            InitializeComponent();
        }

        Employee empNew = null;
        Product product = new Product();
        DBAccess dba = new DBAccess();

        internal void AccessPickProduct(Employee emp)
        {
            empNew = emp;
            ShowDialog();
        }

        private void frmPickProduct_Load(object sender, EventArgs e)
        {
            GetProductsFromDatabase();
        }

        private void GetProductsFromDatabase()
        {
            DBAccess dba = new DBAccess();
            DataTable dt = new DataTable();
            dt = dba.GetProductList();
            LoadProductList(dt);
        }

        int dataTableRowCount = 0;
        int leastDistanceIndex;
        int productCount = 0;
        double totalCost = 0;

        List<string[]> productListUpdated = new List<string[]>();

        private void LoadProductList(DataTable dt)
        {
            List<string[]> productList = new List<string[]>();
            dataTableRowCount = dt.Rows.Count;
            string originCoordinates = "0, 0";
            SortedDictionary<double, double> distanceToIndex = new SortedDictionary<double, double>();
            List<double> sortedList = new List<double>();
            int randomNum = 0;


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string[] tempArray = new string[10];

                for (int k = 0; k < tempArray.Length; k++)
                {
                    tempArray[k] = null;
                }
                for (int j = 0; j < 6; j++)
                {
                    tempArray[j] = dt.Rows[i][j].ToString();
                }

                tempArray[6] = tempArray[4].Substring(0, tempArray[4].IndexOf(","));  // Index 6 :  X Co-ordinate
                tempArray[7] = tempArray[4].Substring(tempArray[4].IndexOf(",") + 1);

                //The above X and Y values are not being used anywhere else in the code, hence redundant.

                double tempDistance = FindDistance(tempArray[4], originCoordinates);
                tempArray[8] = (tempDistance).ToString();
                distanceToIndex.Add(tempDistance, i);

                Random r = new Random();      //generating a random number for Quantity
                randomNum = r.Next(1, 10);

                tempArray[9] = randomNum.ToString();

                productList.Add(tempArray);
            }

            foreach (KeyValuePair<double, double> blabla in distanceToIndex)
            {
                sortedList.Add(blabla.Value);
            }

            leastDistanceIndex = Convert.ToInt32(sortedList[0]);

            SetProductPropertiesAndDisplay(productList);

            totalCost = product.Cost;

            dba.UpdateProductData(product);

            productListUpdated.Add(productList[leastDistanceIndex]);

            for (int i = 0; i < productList.Count; i++)
            {
                if (i != leastDistanceIndex)
                {
                    productListUpdated.Add(productList[i]);
                }
            }

        }

        private void SetProductPropertiesAndDisplay(List<string[]> productList)
        {
            lblItemName.Text = productList[leastDistanceIndex][0];
            lblLocation.Text = productList[leastDistanceIndex][3];
            lblQuantity.Text = productList[leastDistanceIndex][9];


            product.Name = productList[leastDistanceIndex][0];
            product.Dpci = productList[leastDistanceIndex][1];
            product.UnitPrice = Convert.ToDouble(productList[leastDistanceIndex][2].ToString());
            product.Quantity = Convert.ToDouble(productList[leastDistanceIndex][9]);
            product.Cost = product.UnitPrice * product.Quantity;
            


            product.FullDetails = string.Empty;
            product.FullDetails = $"Item: {product.Name}\t" +
                                      $"Unit Price: {product.UnitPrice}\t" +
                                      $"Units: {product.Quantity} \t" +
                                      $"Cost: {product.Cost}";

            product.Summary.Add(product.FullDetails);
            productCount++;

            product.Availability = Convert.ToDouble(productList [leastDistanceIndex][5].ToString());

            if (product.Availability >= product.Quantity)
            {
                product.Availability = product.Availability - product.Quantity;
            }

            else
            {
                product.Quantity = product.Availability;
                product.Availability = 0;
                //need to figure out what to do when products are unavailable
                //Probably there must be some message.
            }
        }


        private static double FindDistance(string v, string w)
        {
            string x1 = v.Substring(0, v.IndexOf(","));
            string y1 = v.Substring(v.IndexOf(",") + 1);

            string x2 = w.Substring(0, w.IndexOf(","));
            string y2 = w.Substring(w.IndexOf(",") + 1);

            double tempX1 = Convert.ToInt32(x1);
            double tempY1 = Convert.ToInt32(y1);

            double tempX2 = Convert.ToInt32(x2);
            double tempY2 = Convert.ToInt32(y2);

            double tempDistance = Math.Sqrt(((tempX1 - tempX2) * (tempX1 - tempX2)) +
                                            ((tempY1 - tempY2) * (tempY1 - tempY2)));
            return tempDistance;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (productListUpdated.Count == 1)
            {
                frmProductSummary prodSummary = new frmProductSummary();
                product = prodSummary.ShowSummary(product);
                Close();
                return;
            }

            IdentifyTheNextProductAndContinue();
        }

        
        List<string[]> tempList = new List<string[]>();
        List<string[]> tempList2 = new List<string[]>();

        private void IdentifyTheNextProductAndContinue()
        {
            leastDistanceIndex = Convert.ToInt32(CompareDistances(productListUpdated));

            SetProductPropertiesAndDisplay(productListUpdated);

            totalCost += product.Cost;

            dba.UpdateProductData(product);

            product.TotalCost = totalCost;
            product.Count = productCount;


            tempList.Clear();
            tempList2.Clear();


            tempList.Add(productListUpdated[leastDistanceIndex]);

            productListUpdated.RemoveAt(leastDistanceIndex);
            productListUpdated.RemoveAt(0);


            foreach (var blabla in productListUpdated)
            {
                tempList2.Add(blabla);
            }

            productListUpdated.Clear();

            productListUpdated.Add(tempList[0]);

            foreach (var blabla in tempList2)
            {
                productListUpdated.Add(blabla);
            }
        }

        private string CompareDistances(List<string[]> productListUpdated)
        {
            
            List<string[]> comparisonList = CreateComparisonList(productListUpdated);

            SortedDictionary<double, string> distanceComparison = new SortedDictionary<double, string>();

            double tinyIncrement = 0.0001;

            //This tiny increment is needed to keep the keys of the dictionary unique. 
            //For certain pair of products distances could be equal.
            //Adding an incremental tiny numeric ensures all the keys are unique
            
            for (int i = 1; i < comparisonList.Count; i++)
            {
                distanceComparison.Add(FindDistance(comparisonList[i][0], comparisonList[0][0]) + tinyIncrement, $"0{i}");
                tinyIncrement += 0.00001;
            }

            List<string> tempSortedDistanceList = new List<string>();

            foreach (KeyValuePair<double, string> blabla in distanceComparison)
            {
                tempSortedDistanceList.Add(blabla.Value);
            }

            if (tempSortedDistanceList.Count > 0)
            {
                string leastDistanceCombo = tempSortedDistanceList[0];
                string t = leastDistanceCombo.Substring(1);
                return t;
            }
            return "0";
        }


        

        private List<string[]> CreateComparisonList(List<string[]> anyList)
        {

            string[][] tempArrayNew = new string[anyList.Count][];

            for (int i = 0; i < anyList.Count; i++)
            {

                tempArrayNew[i] = new string[2];
                tempArrayNew[i][0] = anyList[i][4];
                tempArrayNew[i][1] = i.ToString();

            }

            List<string[]> cmpList = new List<string[]>();
            for (int i = 0; i < tempArrayNew.GetLength(0); i++)
            {

                cmpList.Add(tempArrayNew[i]);

            }
            return cmpList;
        }

    }
}
