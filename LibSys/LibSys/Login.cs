using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace LibSys
{
    public partial class frmLogin : Form
    {
        string connectionString = "Data Source=BAISRANIE\\SQLEXPRESS;Initial Catalog=LibSys;Integrated Security=True;";
        private SqlConnection con;
        public frmLogin()
        {
            InitializeComponent();
            con = new SqlConnection(connectionString);
          
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            con.Open();
            string username = txtBoxUser.Text;
            string password = txtBoxPass.Text;

            SqlCommand cmd = new SqlCommand("SELECT AccountPassword FROM Accounts WHERE Username = '" + username + "'", con);
            string pass = cmd.ExecuteScalar()?.ToString();

            if (password == pass)
            {
                cmd = new SqlCommand("SELECT FirstName from Accounts WHERE username = '" + username + "'", con);
                string fname = cmd.ExecuteScalar()?.ToString();
                cmd = new SqlCommand("SELECT LastName from Accounts WHERE username = '" + username + "'", con);
                string lname = cmd.ExecuteScalar()?.ToString();
                frmMain main = new frmMain(fname, lname, username);
                MessageBox.Show("Success", "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                main.ShowDialog();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Error", "Login error, wrong password", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            con.Close();
        }

        private void labelRegister_Click(object sender, EventArgs e)
        {
            new RegisterForm().Show();
        }




        private void frmLogin_Load(object sender, EventArgs e)
        {
            this.AcceptButton = btnLogin;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtBoxPass.Clear();
            txtBoxUser.Clear();
        }
      
    }
}
