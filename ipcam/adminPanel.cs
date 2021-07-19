using Onvif.IP.Camera.Viewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AlertSystem
{
    public partial class adminPanel : Form
    {
        public adminPanel()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // show camera
            /* new showCamera().Show();
             this.Hide();*/
            new Form1().Show();

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // set camara
            new setCamera().Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            // reports
            new reports().Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            // notification
            new notification().Show();
            this.Hide();

        }

        private void adminPanel_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            // add person
            new addPerson().Show();
            this.Hide();
            
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            // logout 
            new login().Show();
            this.Close();
        }
    }
}
