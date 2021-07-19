using ipcam;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AlertSystem
{
    public partial class setCamera : Form
    {
        public setCamera()
        {
            InitializeComponent();
            setModulator();
            SetcameraModel = new setCameraModel();
        }
        private setCameraModel SetcameraModel;
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            new adminPanel().Show();
            this.Hide();
        }
        private void setModulator()
        {
           

        }
        private void setModel()
        {

        }

        private void setCamera_Load(object sender, EventArgs e)
        {

        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            // if all 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.ToString() == null || textBox1.Text.ToString() == "")
                MessageBox.Show("http is Required!");
            else if (!CheckValidURL(textBox1.Text.ToString()) )
                MessageBox.Show("Camera http is not valid.");

            else
            {
                String source = textBox1.Text;
                SetcameraModel.setSource(source);
                MessageBox.Show(SetcameraModel.getSource().ToString());
            }
        }
        public bool CheckValidURL(String uriName)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(uriName, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return result;
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {



          /*  if (checkedListBox1.GetItemChecked(0))
                MessageBox.Show("item chekd");
            //for (int i = 0; i < checkedListBox1.Items.Count; i++)
            //{
            //    checkedListBox1.SetItemChecked(i, true);
            //}*/
        }
    }
}
