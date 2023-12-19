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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace LibSys
{
    public partial class ReturnBook : Form
    {
        string connectionString = "Data Source=BAISRANIE\\SQLEXPRESS;Initial Catalog=LibSys;Integrated Security=True;";
        private SqlConnection con;
        private SqlCommand command;
        private int borrowedAccession = 0;
        private string fName = "", lName = "";

        public ReturnBook(string fName, string lName)
        {
            InitializeComponent();
            con = new SqlConnection(connectionString);
            this.fName = fName;
            this.lName = lName;
        }

        private void Return_Load(object sender, EventArgs e)
        {
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

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (borrowedAccession == 0)
            {
                MessageBox.Show("Click book to return in the grid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            con.Open();

            try
            {
                command = new SqlCommand("SELECT bookStatus FROM book WHERE accession_number = " + borrowedAccession, con);
                string status = command.ExecuteScalar().ToString();

                command = new SqlCommand("UPDATE book SET bookStatus = 'Available' WHERE accession_number = " + borrowedAccession, con);
                command.ExecuteNonQuery();

                command.ExecuteNonQuery();

                command = new SqlCommand("SELECT title from book where accession_number = " + borrowedAccession, con);
                string title = command.ExecuteScalar().ToString();
                MessageBox.Show("Successfully Returned, " + title + "!", "Returned", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
                LoadDataGrid();

            }
            catch (FormatException)
            {
                MessageBox.Show("Input the right format or input all in the fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
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
                labelBorrowedBook.Text = "Book to return: " + title;
            }
        }


        //private void txtSearch_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        con.Open();

        //        SqlCommand com = new SqlCommand("SELECT book.accession_number, book.title, Borrower.BorrowerId, Borrower.LastName, Borrower.FirstName FROM book INNER JOIN Borrower ON book.accession_number = Borrower.book_accession_number WHERE Borrower.FirstName LIKE '%' + @fname + '%' AND Borrower.LastName LIKE '%' + @lname + '%' AND book.accession_number LIKE '%' + @searchText + '%'", con);
        //        com.Parameters.AddWithValue("@fname", this.fName);
        //        com.Parameters.AddWithValue("@lname", this.lName);
        //        com.Parameters.AddWithValue("@searchText", txtSearch.Text);

        //        SqlDataAdapter adap = new SqlDataAdapter(com);
        //        DataTable tab = new DataTable();

        //        adap.Fill(tab);
        //        grid1.DataSource = tab;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error: " + ex.Message);
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}












        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

            con.Open();

            SqlCommand com = new SqlCommand("SELECT br.BorrowerId, b.accession_number, b.title FROM Borrower AS br INNER JOIN book AS b ON br.book_accession_number = b.accession_number WHERE br.FirstName = @fname AND br.LastName = @lname AND b.accession_number LIKE '%" + txtSearch.Text + "%'", con);
            com.Parameters.AddWithValue("@fname", this.fName);
            com.Parameters.AddWithValue("@lname", this.lName);

            SqlDataAdapter adap = new SqlDataAdapter(com);
            DataTable tab = new DataTable();

            adap.Fill(tab);
            grid1.DataSource = tab;

            con.Close();
        }
    }
}
