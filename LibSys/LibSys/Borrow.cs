using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace LibSys
{
    public partial class Borrow : Form
    {
        string connectionString = "Data Source=BAISRANIE\\SQLEXPRESS;Initial Catalog=LibSys;Integrated Security=True;";
        private SqlConnection con;
        private SqlCommand command;
        private int borrowedAccession = 0;
        private string username = "";

        public Borrow(string username)
        {
            InitializeComponent();
            con = new SqlConnection(connectionString);
            this.username = username;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            con.Open();

            SqlCommand com = new SqlCommand("Select * from book where title like '%" + txtSearch.Text + "%'", con);
            com.ExecuteNonQuery();

            SqlDataAdapter adap = new SqlDataAdapter(com);
            DataTable tab = new DataTable();

            adap.Fill(tab);
            grid1.DataSource = tab;

            con.Close();
        }

        private void grid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (grid1.SelectedCells.Count > 0)
            {
                int selectedrowindex = grid1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = grid1.Rows[selectedrowindex];
                string cellValue = Convert.ToString(selectedRow.Cells["accession_number"].Value);
                borrowedAccession = int.Parse(cellValue);
                string title = Convert.ToString(selectedRow.Cells["title"].Value);
                labelBorrowedBook.Text = "Book to borrow: " + title;
            }
        }

        private void btnBorrow_Click(object sender, EventArgs e)
        {
            if (borrowedAccession == 0)
            {
                MessageBox.Show("Click book to borrow in the grid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            con.Open();

            try
            {
                command = new SqlCommand("SELECT bookStatus FROM book WHERE accession_number = " + borrowedAccession, con);
                string status = command.ExecuteScalar().ToString();

                if (status == "Not Available")
                {
                    MessageBox.Show("Book not available", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                    return;
                }
                command = new SqlCommand("SELECT FirstName from Accounts WHERE username = '" + username + "'", con);
                string fname = command.ExecuteScalar().ToString();
                command = new SqlCommand("SELECT LastName from Accounts WHERE username = '" + username + "'", con);
                string lname = command.ExecuteScalar().ToString();
                command = new SqlCommand("SELECT Age from Accounts WHERE username = '" + username + "'", con);
                int age = int.Parse(command.ExecuteScalar().ToString());
                int daysBorrow = int.Parse(txtDaysBorrow.Text);

                command = new SqlCommand("UPDATE book SET bookStatus = 'Not Available' WHERE accession_number = " + borrowedAccession, con);
                command.ExecuteNonQuery();

                command = new SqlCommand("SELECT ISNULL(MAX(BorrowerID), 0) + 1 FROM Borrower", con);
                int newBorId = Convert.ToInt32(command.ExecuteScalar());

                command = new SqlCommand("INSERT INTO borrower VALUES(@borId, @FirstName, @LastName, @Age, @DaysBorrow, @accNo)", con);
                command.Parameters.AddWithValue("@borId", newBorId);
                command.Parameters.AddWithValue("@FirstName", fname);
                command.Parameters.AddWithValue("@LastName", lname);
                command.Parameters.AddWithValue("@Age", age);
                command.Parameters.AddWithValue("@DaysBorrow", daysBorrow);
                command.Parameters.AddWithValue("@accNo", borrowedAccession);
                command.ExecuteNonQuery();

                command = new SqlCommand("SELECT title from book where accession_number = " + borrowedAccession, con);
                string title = command.ExecuteScalar().ToString();
                MessageBox.Show("Successfully Borrowed," + title + "!", "Borrowed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
                LoadDataGrid();

            }
            catch (FormatException)
            {
                MessageBox.Show("Input the right format or input all in the fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }


        }

        private void LoadDataGrid()
        {
            con.Open();

            SqlCommand com = new SqlCommand("Select * from book order by accession_number asc", con);
            com.ExecuteNonQuery();

            SqlDataAdapter adap = new SqlDataAdapter(com);
            DataTable tab = new DataTable();

            adap.Fill(tab);
            grid1.DataSource = tab;

            con.Close();
        }

        private void Borrow_Load(object sender, EventArgs e)
        {
            LoadDataGrid();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }
    }
}
