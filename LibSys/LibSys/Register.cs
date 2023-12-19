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
using System.Runtime.CompilerServices;


namespace LibSys
{
    public partial class RegisterForm : Form
    { 
        string connectionString = "Data Source=BAISRANIE\\SQLEXPRESS;Initial Catalog=LibSys;Integrated Security=True;";
        private SqlConnection con;

        public RegisterForm()
        {
            InitializeComponent();
            con = new SqlConnection(connectionString);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            con.Open();
            try
            {
                string username = txtBoxUser.Text;
                string password = txtBoxPass.Text;
                string fName = txtFName.Text;
                string lName = txtLName.Text;
                int age = Convert.ToInt32(numericUpDown1.Value);

                SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(AccountID), 0) + 1 FROM Accounts", con);
                int accID = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("INSERT INTO Accounts VALUES(@username, @pass, @fname,  @lname, @age, @accID);", con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@pass", password);
                cmd.Parameters.AddWithValue("@fname", fName);
                cmd.Parameters.AddWithValue("@lname", lName);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.Parameters.AddWithValue("@accID", accID);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Registered!", "You have successfully registered, " + username, MessageBoxButtons.OK, MessageBoxIcon.Information);

                con.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Error", "Input username and password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }

        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            this.AcceptButton = btnRegister;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtBoxPass.Clear();
            txtBoxUser.Clear();
            txtFName.Clear();
            txtLName.Clear();
        }

        private void groupBoxLogin_Enter(object sender, EventArgs e)
        {

        }

        private void labelLogin_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
