﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace LibSys
{
    public partial class LibSys : Form
    {
        string connectionString = "Data Source=BAISRANIE\\SQLEXPRESS;Initial Catalog=LibSys;Integrated Security=True;";
        private SqlConnection con;
        public LibSys()
        {
            InitializeComponent();
            con = new SqlConnection(connectionString);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDataGrid();
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand com = new SqlCommand("Insert into book (accession_number, title, author) VALUES (@accession_number, @title, @author)", con);
            com.Parameters.AddWithValue("@accession_number", txtno.Text);
            com.Parameters.AddWithValue("@title", txttitle.Text);
            com.Parameters.AddWithValue("@author", txtauthor.Text);
            com.ExecuteNonQuery();

            MessageBox.Show("Successfully SAVED!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            con.Close();
            LoadDataGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            con.Open();
            try
            {
                int num = int.Parse(txtno.Text);

                DialogResult dr;

                dr = MessageBox.Show("Are you sure you want to delete?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    SqlCommand com = new SqlCommand("Delete from book where accession_number = @accession_number", con);
                    com.Parameters.AddWithValue("@accession_number", txtno.Text);
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
                MessageBox.Show("Please enter accession number to delete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }


        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            con.Open();

            SqlCommand com = new SqlCommand("Select * from book where title like @title", con);
            com.Parameters.AddWithValue("@title", "%" + txtSearch.Text + "%");
            com.ExecuteNonQuery();

            SqlDataAdapter adap = new SqlDataAdapter(com);
            DataTable tab = new DataTable();

            adap.Fill(tab);
            grid1.DataSource = tab;

            con.Close();
        }



        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                int no = int.Parse(txtno.Text);
                string title = txttitle.Text;
                string author = txtauthor.Text;
                SqlCommand com = new SqlCommand("Update book SET title = @title, author = @author where accession_number = @accession_number", con);
                com.Parameters.AddWithValue("@accession_number", no);
                com.Parameters.AddWithValue("@title", title);
                com.Parameters.AddWithValue("@author", author);
                com.ExecuteNonQuery();

                MessageBox.Show("Successfully UPDATED!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
                LoadDataGrid();
            }
            catch (FormatException)
            {
                MessageBox.Show("Error! Enter author or title!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }

        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                int no = int.Parse(txtno.Text);
                string title = txttitle.Text;
                string author = txtauthor.Text;
                SqlCommand com = new SqlCommand("SELECT * FROM book WHERE title = @title OR author = @author OR accession_number = @accession_number", con);
                com.Parameters.AddWithValue("@title", title);
                com.Parameters.AddWithValue("@author", author);
                com.Parameters.AddWithValue("@accession_number", no);
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

                con.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Error! Enter number for accession number!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }

            //LoadDataGrid();
        }





        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtauthor.Clear();
            txtno.Clear();
            txtSearch.Clear();
            txttitle.Clear();
            this.Close();
        }

        private void grid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
