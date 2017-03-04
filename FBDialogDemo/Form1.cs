using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FBDialogDemo
{
    public partial class Form1 : Form
    {
        public string AppID = string.Empty;
        public string Scope = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Visible = false;
        }

        private void bFBLogin_Click(object sender, EventArgs e)
        {

            AppID = "1383609294993448";
            Scope = "public_profile,user_friends,user_photos,user_videos,email";
            FBDialog fbd = new FBDialog(AppID, Scope);
            
            switch (fbd.ShowDialog(this))
            {
                case DialogResult.Abort:    // There was an error
                    //Get the error information
                    //lbResult.Items.Add(string.Format("Error: {0}", fbd.error));
                    //lbResult.Items.Add(string.Format("Error reason: {0}", fbd.error_reason));
                    //lbResult.Items.Add(string.Format("Error description: {0}", fbd.error_description));
                    MessageBox.Show("There was an error or the user denied access! See log window for more information!", "Error: An error occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case DialogResult.Cancel:   // User clicked cancel or closed the dialog
                    MessageBox.Show("The user clicked cancel or closed the dialog!", "Error: Interupted by user", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case DialogResult.OK:   // Logon successfull
                    //lbResult.Items.Add(string.Format("Access token: {0}", fbd.access_token));
                    //lbResult.Items.Add(string.Format("Token expires: {0}", fbd.token_expires));
                    //lbResult.Items.Add(string.Format("Granted scopes: {0}", fbd.granted_scopes));
                    //lbResult.Items.Add(string.Format("Denied scopes: {0}", fbd.denied_scopes));
                    MessageBox.Show("User login was successfull! See log window for more information!", "Successfull!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    FetchDP(fbd.access_token);
                    break;
                default:
                    break;
            }
        }


        private void FetchDP(string access_token)
        {
            Facebook.FacebookClient client = new Facebook.FacebookClient();
            client.AccessToken = access_token;
            dynamic me = client.Get("me?fields=picture,email,name,gender");
            pictureBox1.Load(me.picture.data.url);
            label1.Visible = true;
            string lable = "Hello "+ me[2]+" !! Your email id is " + me[1];
            label1.Text = lable; 

            
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.google.com");
        }
    }
}
