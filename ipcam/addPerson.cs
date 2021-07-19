using ipcam;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Helpers;
using System.Windows.Forms;

namespace AlertSystem
{
    public partial class addPerson : Form
    {
        static String access_key = "938921aa5556407a807c15fcd5fbb69c";
        WaitFormFunc waitForm = new WaitFormFunc();
        public addPerson()
        {
            InitializeComponent();
            /*   this.panel1.Hide();
               this.panel2.Show();
               this.panel3.Hide();*/
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // back
            new adminPanel().Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e) // panel 1 btn
        {
            //   this.panel1.Hide();
            this.panel2.Show();
            //  panel3.Hide();
        }


        private void train()
        {
            String APIurl = "https://palestine.cognitiveservices.azure.com/face/v1.0/persongroups/student/train";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(APIurl);
            MessageBox.Show(APIurl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Ocp-Apim-Subscription-Key", access_key);
            /*no body */
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
           
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { 
                if (httpResponse.StatusCode == HttpStatusCode.Accepted)
                {
                    MessageBox.Show("Person added successfully !.");
                }
                else
                {
                    MessageBox.Show("Other Status Code returned.");
                }
            }


        }
        private void addface(String personId)
        {


            {
                String Imageurl = textBox1.Text.ToString();
                String ImageData = textBox2.Text.ToString();
                MessageBox.Show(Imageurl + " , " + ImageData);
                if (Imageurl == "" || ImageData == "")
                {
                    MessageBox.Show("You should provide a name and userData");

                }
                else
                {
                    waitForm.Show(this);
                    String APIurl = "https://palestine.cognitiveservices.azure.com/face/v1.0/persongroups/student/persons/"+personId+"/persistedFaces?userData="+ImageData;

                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(APIurl);
                    MessageBox.Show(APIurl);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    // httpWebRequest.Headers.Add("Content-Type", "application/json");
                    httpWebRequest.Headers.Add("Ocp-Apim-Subscription-Key", access_key);
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = "{\"url\"" + ": \" " + Imageurl +
                            "\"}";
                        MessageBox.Show(json);
                        streamWriter.Write(json);
                    }
                    
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        dynamic r = JsonConvert.DeserializeObject(result);
                        try
                        {
                            
                            string persistedFaceId = r.persistedFaceId.ToString();
                            Console.WriteLine(persistedFaceId); // after ...train face

                            MessageBox.Show("persistedFaceId:" + persistedFaceId);
                            

                            /*this.panel2.Hide();
                            this.panel3.Show();*/
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("error request : "+ex.Message);
                        }
                        waitForm.Close();
                    }


                }
            }
        }
        private void button1_Click(object sender, EventArgs e) // panel 2 btns submit create person
        {
            String name = textBox3.Text.ToString();
            String userData = richTextBox1.Text.ToString(); 
            MessageBox.Show(name + " , " + userData);
            if (name == "" || userData == "")
            {
                MessageBox.Show("You should provide a name and userData");

            }
            else
            {
                waitForm.Show(this);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://palestine.cognitiveservices.azure.com/face/v1.0/persongroups/student/persons");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                // httpWebRequest.Headers.Add("Content-Type", "application/json");
                httpWebRequest.Headers.Add("Ocp-Apim-Subscription-Key", access_key);
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"name\"" + ": \" " + name + "\"," +
                      "\"userData\":\"" + userData + "\"}";
                    MessageBox.Show(json);
                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    dynamic r = JsonConvert.DeserializeObject(result);
                    try
                    {
                        string personId = r.personId.ToString();
                        Console.WriteLine(personId); // after ...add face
                        addface(personId);
                        //MessageBox.Show("personId :"+ personId);

                        /*this.panel2.Hide();
                        this.panel3.Show();*/
                    }
                    catch
                    {
                        MessageBox.Show("error request");
                    }
                }


            }
        }
    }
       
}
