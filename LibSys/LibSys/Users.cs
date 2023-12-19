using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace LibSys
{
    public partial class Users : Form
    {
        string connectionString = "Data Source=BAISRANIE\\SQLEXPRESS;Initial Catalog=LibSys;Integrated Security=True;";
        private SqlConnection con;
        private SqlCommand command;
        public Users()
        {
            InitializeComponent();
            con = new SqlConnection(connectionString);
        }

        

        private void Users_Load(object sender, EventArgs e)
        {
            LoadDataGrid();
        }
        private void LoadDataGrid()
        {
            con.Open();

            SqlCommand com = new SqlCommand("Select * from accounts order by AccountID asc", con);
            com.ExecuteNonQuery();

            SqlDataAdapter adap = new SqlDataAdapter(com);
            DataTable tab = new DataTable();

            adap.Fill(tab);
            grid1.DataSource = tab;

            con.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            con.Open();

            SqlCommand com = new SqlCommand("Select * from accounts where FirstName like '%" + txtSearch.Text + "%'", con);
            com.Parameters.AddWithValue("@title", txtLastname.Text);
            com.ExecuteNonQuery();

            SqlDataAdapter adap = new SqlDataAdapter(com);
            DataTable tab = new DataTable();

            adap.Fill(tab);
            grid1.DataSource = tab;

            con.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtFirstname.Text) && !string.IsNullOrWhiteSpace(txtLastname.Text) && !string.IsNullOrWhiteSpace(txtAge.Text))
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Get data from textboxes
                    string firstName = txtFirstname.Text;
                    string lastName = txtLastname.Text;
                    string age = txtAge.Text;

                    // Perform the delete operation
                    DeleteUser(firstName, lastName, age);

                    // Refresh the DataGridView after deletion
                    LoadDataGrid();

                    // Clear the textboxes after deletion
                    ClearTextBoxes();
                }
            }
            else
            {
                MessageBox.Show("Please enter data in all textboxes.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void ClearTextBoxes()
        {
            txtFirstname.Text = string.Empty;
            txtLastname.Text = string.Empty;
            txtAge.Text = string.Empty;
        }

        private void DeleteUser(string firstName, string lastName, string age)
        {
            con.Open();

            string deleteQuery = "DELETE FROM accounts WHERE FirstName = @firstName AND LastName = @lastName AND Age = @age";

            using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
            {
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastName", lastName);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.ExecuteNonQuery();
            }

            con.Close();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }



        private void grid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = grid1.Rows[e.RowIndex];

                // Assuming your columns are named "FirstName", "LastName", and "Email"
                string firstName = row.Cells["FirstName"].Value.ToString();
                string lastName = row.Cells["LastName"].Value.ToString();
                string Age = row.Cells["Age"].Value.ToString();

                // Set the values in your textboxes
               txtFirstname.Text = firstName;
                txtLastname.Text = lastName;
                txtAge.Text = Age;
            }
        }










    }
}
