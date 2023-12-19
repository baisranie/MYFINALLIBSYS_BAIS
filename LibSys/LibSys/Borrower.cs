using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibSys
{ 
    public partial class Borrower : Form
    {
        string connectionString = "Data Source=BAISRANIE\\SQLEXPRESS;Initial Catalog=LibSys;Integrated Security=True;";8
        private SqlConnection con;

        public Borrower()
        {
            InitializeComponent();
            con = new SqlConnection(connectionString);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("Insert into Borrower (BorrowerID, FirstName, LastName, Age, DaysBorrow) VALUES (@BorrowerID, @FirstName, @LastName, @Age, @DaysBorrow);", con);

                com.Parameters.AddWithValue("@BorrowerID", int.Parse(txtID.Text));
                com.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                com.Parameters.AddWithValue("@LastName", txtLastName.Text);
                com.Parameters.AddWithValue("@Age", int.Parse(txtAge.Text));
                com.Parameters.AddWithValue("@DaysBorrow", int.Parse(txtDaysBorrow.Text));
                com.ExecuteNonQuery();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
                MessageBox.Show("Error!", "Input all information, and appropriately.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }

            MessageBox.Show("Successfully SAVED!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadDataGrid();
        }



        private void LoadDataGrid()
        {
            con.Open();
            SqlDataAdapter adap = new SqlDataAdapter("Select * from Borrower order by BorrowerID asc", con);
            DataTable tab = new DataTable();

            adap.Fill(tab);
            grid1.DataSource = tab;

            con.Close();
        }



        private void Borrower_Load(object sender, EventArgs e)
        {
            LoadDataGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            con.Open();
            try
            {
                int num = int.Parse(txtID.Text);

                DialogResult dr;

                dr = MessageBox.Show("Are you sure you want to delete?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    SqlCommand com = new SqlCommand("Delete from Borrower where BorrowerID = @BorrowerID", con);
                    com.Parameters.AddWithValue("@BorrowerID", num);
                    com.ExecuteNonQuery();

                    MessageBox.Show("Successfully DELETED!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Successfully CANCELLED!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                con.Close();
                LoadDataGrid();
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter borrower ID number to delete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                int no = int.Parse(txtID.Text);
                string firstName = txtFirstName.Text;
                string lastName = txtLastName.Text;
                int age = int.Parse(txtAge.Text);
                int daysBorrow = int.Parse(txtDaysBorrow.Text);

                SqlCommand com = new SqlCommand("Update Borrower SET FirstName = @FirstName, LastName = @LastName, Age = @Age, DaysBorrow = @DaysBorrow where BorrowerID = @BorrowerID", con);
                com.Parameters.AddWithValue("@BorrowerID", no);
                com.Parameters.AddWithValue("@FirstName", firstName);
                com.Parameters.AddWithValue("@LastName", lastName);
                com.Parameters.AddWithValue("@Age", age);
                com.Parameters.AddWithValue("@DaysBorrow", daysBorrow);

                if (com.ExecuteNonQuery() == 0)
                {
                    MessageBox.Show("Error! Information not found in database!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Successfully UPDATED!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataGrid();
            }
            catch (FormatException)
            {
                MessageBox.Show("Error! Enter all information!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }



        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                int no = int.Parse(txtID.Text);
                SqlCommand com = new SqlCommand("SELECT * FROM Borrower WHERE BorrowerID = @BorrowerID", con);
                com.Parameters.AddWithValue("@BorrowerID", no);
                com.ExecuteNonQuery();

                SqlDataReader reader = com.ExecuteReader();

                if (reader.HasRows)
                {
                    MessageBox.Show("Successfully shown!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Not found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                reader.Close();
                SqlDataAdapter adap = new SqlDataAdapter(com);
                DataTable tab = new DataTable();

                adap.Fill(tab);
                grid1.DataSource = tab;

            }
            catch (FormatException)
            {
                MessageBox.Show("Error! Enter number for Borrower ID Number only!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //butangann pa og func ari
        }
    }
}
