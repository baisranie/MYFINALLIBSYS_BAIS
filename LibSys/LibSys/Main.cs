using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using LibSys;
namespace LibSys
{
    
    public partial class frmMain : Form
    {
        private string Fname = "", Lname = "", username = "";
        public frmMain(string Fname, string Lname, string username)
        {
            InitializeComponent();
            this.Fname = Fname;
            this.Lname = Lname; 
            this.username = username;
        }

        private void librarySystremToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pass = Interaction.InputBox("Password:", "Admin Access Only", "Enter here");
            if (pass == "superAdmin")
            {
                LibSys lib = new LibSys();
                lib.ShowDialog();
            }
            else
            {
                MessageBox.Show("Wrong Password!", "Admin access only!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void borrowerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pass = Interaction.InputBox("Password:", "Admin Access Only", "Enter here");
            if(pass == "staffAdmin"){
                Borrower b = new Borrower();
                b.ShowDialog();
            }
            else
            {
                MessageBox.Show("Wrong Password!", "Admin access only!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pass = Interaction.InputBox("Password:", "Admin Access Only", "Enter here");
            if (pass == "superAdmin")
            {
                
                Users user = new Users();
                user.ShowDialog();
            }
            else
            {
                MessageBox.Show("Wrong Password!", "Admin access only!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void returnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReturnBook ret = new ReturnBook(this.Fname, this.Lname);
            ret.ShowDialog();
        }

        private void masterfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
               
                    frmLogin l = new frmLogin();

                    // Display the login form
                    l.ShowDialog();

                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void borrowABookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Borrow borrow = new Borrow(username);
            borrow.ShowDialog();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            labelWelcome.Text = "Welcome, " + this.Fname + " " + this.Lname + "!";
        }
    }
}
