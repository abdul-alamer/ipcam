using System;
using System.Windows.Forms;
using Ozeki.Media.MediaHandlers;
//using Ozeki.Media.MediaHandlers.IPCamera;
using Ozeki.Media.MediaHandlers.Video;
using Ozeki.Media.Video.Controls;
using Ozeki.Media.IPCamera;
using Ozeki.Media.MJPEGStreaming;
using System.Net;
using System.IO;
using System.Drawing;
using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using System.Text;
using System.Net.Http;
using System.Collections.Generic;
using ipcam;
/*change in line 213,491,206,422,156*/
namespace Onvif.IP.Camera.Viewer
{
    public partial class Form1 : Form
    {
        public ChromiumWebBrowser browser;
        reportModel ReportModel; // to add detected persons & weapons
        
        Panel pan = new Panel();
        Panel pan1 = new Panel();
        Panel pan2 = new Panel();
        Panel pan3 = new Panel();
        private static bool exec_report = true;
        private setCameraModel setCamera;
        public string GetTimestamp()
        {
            DateTime now = DateTime.Now;
            return now.ToString("F");
        }
        public Form1()
        {
            InitializeComponent();
            ReportModel = new reportModel();
         //   MessageBox.Show("start");
            setCamera = new setCameraModel();
            Cef.Initialize(new CefSettings());
            browser = new ChromiumWebBrowser("http://admin:As99348291@172.17.28.115/ISAPI/Streaming/channels/101/httpPreview");
            //browser.RequestContext = new RequestContext();

            panel1.Controls.Add(browser);
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                startdetection();

            }).Start();
            browser.Dock = DockStyle.Fill;

            Controls.Add(pan);
            Controls.Add(pan1);
            Controls.Add(pan2);
            Controls.Add(pan3);


        }

        // Connect camera video channel to image provider and start
        private void connectBtn_Click(object sender, EventArgs e)
        {
      
        }
      
        public static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;
        }
      
        public async void startdetection() 
        {
            if (this.IsHandleCreated)
            {
                if (button1.Enabled==false) { 
                panel1.Invoke(new MethodInvoker(async delegate {
                    Bitmap controlBitMap = new Bitmap(panel1.Width, panel1.Height);
                    Graphics g = Graphics.FromImage(controlBitMap);
                    g.CopyFromScreen(PointToScreen(panel1.Location), new System.Drawing.Point(0, 0), panel1.Size);


                    System.IO.MemoryStream ms = new MemoryStream();
                    controlBitMap.Save(ms, ImageFormat.Jpeg);
                    byte[] byteImage = ms.ToArray();
                    var SigBase64 = Convert.ToBase64String(byteImage); // Get Base64

                    //     controlBitMap.Save(Application.StartupPath + "\\img.jpg");




                    var client = new HttpClient();

                    // Create the HttpContent for the form to be posted.
                    var requestContent = new FormUrlEncodedContent(new[] {
    new KeyValuePair<string, string>("image",SigBase64),
});//error

                    // Get the response.
                    HttpResponseMessage response = await client.PostAsync(
                        "https://api.imgbb.com/1/upload?expiration=600&key=24af7b0ec341c6a091392971057aeb08",
                        requestContent);

                    // Get the response content.
                    HttpContent responseContent = response.Content;

                    // Get the stream of the content.
                    using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
                    {
                        // Write the output.
                        string result = await reader.ReadToEndAsync();
                        dynamic r = JsonConvert.DeserializeObject(result);
                      /*  new Thread(() =>
                                              {
                                                  Thread.CurrentThread.IsBackground = true;*/
                                                  string url = r.data.url;
                                                  detectface(url);     
                                                 detectweapon(url);
                                           /*   }).Start();*/
                    }




                    /*  */










                }));
            }
            else
            {
                    Task.Delay(new TimeSpan(0, 0, 1)).ContinueWith(o => {
                    startdetection();

                    });
            }
          
            }
            else
            {
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    /* run your code here */
                    startdetection();

                }).Start();
            }
          

        }
        public void detectweapon(string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://www.picpurify.com/analyse/1.1");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
           // httpWebRequest.Headers.Add("Content-Type", "application/json");
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                //5BeskvPBCAKDPh4fARLn8SCNW15pSzy6 --wesam
                //S7pDQY792u6gySduy4dWfbhr5FsO0Mf3 -- me 
                string json = "{\"API_KEY\":\"" + "S7pDQY792u6gySduy4dWfbhr5FsO0Mf3" + "\"," +
                    "\"url_image\":\"" + url + "\"," +
                      "\"task\":\"" + "weapon_moderation" + "\"}";

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                dynamic r = JsonConvert.DeserializeObject(result);
                try
                {
                    string weapon = r.weapon_moderation.weapon_content;
                    Console.WriteLine(weapon);
                    if (weapon == "False")
                    {
                        label3.Invoke(new MethodInvoker(async delegate
                        {

                            label3.Text = "No Weapon Detected...";
                            exec_report = true;

                        }));
                    }
                    else
                    {
                        label3.Invoke(new MethodInvoker(async delegate
                        {

                            label3.Text = "Weapon Detected!!!";

                         /*  if (label2.Text == "Face Found In DataBase..."  &&exec_report)
                            {
                                String name = label1.Text.ToString(); // get userData from label text
                                ReportModel.setAll(label3.Text.ToString(), name, GetTimestamp(),
                                    setCamera.getSource().ToString());
                                exec_report = false;
                            }
                            else if (label2.Text == "Face Was Not Found In DataBase!!!" && exec_report)
                            {
                                String name = "Unknown Person";
                                ReportModel.setAll(label3.Text.ToString(), name, GetTimestamp(),
                                   setCamera.getSource().ToString());
                                exec_report = false;
                            }
                           

                            */
                        }));
                    }
                }
                catch
                {

                    
                }


            }

        }
        public void detectface(string url) // should add recognition model into parameter 
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://palestine.cognitiveservices.azure.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=true&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise&recognitionModel=recognition_04");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            // httpWebRequest.Headers.Add("Content-Type", "application/json");
            httpWebRequest.Headers.Add("Ocp-Apim-Subscription-Key", "938921aa5556407a807c15fcd5fbb69c");
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string json = "{\"url\":\"" + url + "\"}";

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                dynamic r = JsonConvert.DeserializeObject(result);
                try
                {
                string faceId = r[0].faceId;
                    Console.WriteLine(r);
                    int top = r[0].faceRectangle.top;
                    int left = r[0].faceRectangle.left;
                    int width = r[0].faceRectangle.width;
                    int height = r[0].faceRectangle.height;
                createpanel(top,left,width,height);
                new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        /* run your code here */
                        identifyface(faceId);

                    }).Start();
                    
                }
                catch
                {
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                       
                        label1.Invoke(new MethodInvoker(delegate {

                            label1.Text = "No Faces Detected.";
                        }));
                        label2.Invoke(new MethodInvoker(delegate {

                            label2.Text = "";
                        }));


                    }).Start();
                    Task.Delay(new TimeSpan(0, 0, 4)).ContinueWith(o => {
                        new Thread(() =>
                        {
                            Thread.CurrentThread.IsBackground = true;
                          
                            startdetection();

                        }).Start();
                    });

                }

            }

        }
     

        public void createpanel(int top,int left,int width,int height)
        {
            
            if (IsHandleCreated)
            {

                pan.Enabled = false;
                pan.Width = width;
                pan.Height = 2;
                pan.Location = new System.Drawing.Point(left, top);
                pan.BackColor = Color.Red;



                pan.BringToFront();

                pan1.Enabled = false;
                pan1.Width = width;
                pan1.Height = 2;
                pan1.Location = new System.Drawing.Point(left, top+height);
                pan1.BackColor = Color.Red;



                pan1.BringToFront();


                pan2.Enabled = false;
                pan2.Width = 2;
                pan2.Height = height;
                pan2.Location = new System.Drawing.Point(left, top);
                pan2.BackColor = Color.Red;



                pan2.BringToFront();

                pan3.Enabled = false;
                pan3.Width = 2;
                pan3.Height = height;
                pan3.Location = new System.Drawing.Point(left+width, top);
                pan3.BackColor = Color.Red;



                pan3.BringToFront();
            }
            else
            {
                createpanel(top,left,width,height);

            }
        }
        public void identifyface(string faceId)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://palestine.cognitiveservices.azure.com/face/v1.0/identify");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            // httpWebRequest.Headers.Add("Content-Type", "application/json");
            httpWebRequest.Headers.Add("Ocp-Apim-Subscription-Key", "938921aa5556407a807c15fcd5fbb69c");
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string json = "{\"faceIds\":[\"" + faceId + "\"]," +
                      "\"personGroupId\":\"" + "student" + "\"}";

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                dynamic r = JsonConvert.DeserializeObject(result);
                try
                {
  string personId = r[0].candidates[0].personId;
                Console.WriteLine(personId);
                    label2.Invoke(new MethodInvoker(async delegate {

                        label2.Text = "Face Found In DataBase...";

                    }));

                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    /* run your code here */
                    getperson(personId);
                }).Start();
             
                }
                catch
                {
                 
                    label1.Invoke(new MethodInvoker(async delegate {

                        label1.Text = "Face Found";

                    }));
                    label2.Invoke(new MethodInvoker(async delegate {

                        label2.Text = "Face Was Not Found In DataBase!!!";
                        exec_report = true;

                    }));
                    Task.Delay(new TimeSpan(0, 0, 4)).ContinueWith(o =>
                    {
                        new Thread(() =>
                        {
                            Thread.CurrentThread.IsBackground = true;
                            /* run your code here */
                            startdetection();

                        }).Start();
                    });
                    }
              

            }
        }
    
        public void getperson(string personId)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://palestine.cognitiveservices.azure.com/face/v1.0/persongroups/student/persons/" + personId);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            // httpWebRequest.Headers.Add("Content-Type", "application/json");
            httpWebRequest.Headers.Add("Ocp-Apim-Subscription-Key", "938921aa5556407a807c15fcd5fbb69c");


            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                dynamic r = JsonConvert.DeserializeObject(result);

              //  Console.WriteLine(r);
                string persistedFaceId = r.persistedFaceIds[0];
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    /* run your code here */
                    getface(persistedFaceId, personId);

                }).Start();
               

            }
        }
        public void getface(string persistedFaceId,string personId)//514fb219-7b2a-43a1-b21a-e43a8e75179c/persistedFaces/197e00b7-aa15-4829-a729-1982d2d3f596
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://palestine.cognitiveservices.azure.com/face/v1.0/persongroups/student/persons/" + personId+"/persistedFaces/"+persistedFaceId);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            // httpWebRequest.Headers.Add("Content-Type", "application/json");
            httpWebRequest.Headers.Add("Ocp-Apim-Subscription-Key", "938921aa5556407a807c15fcd5fbb69c");


            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                dynamic r = JsonConvert.DeserializeObject(result);

                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    /* run your code here */
                    label1.Invoke(new MethodInvoker(delegate {

                        label1.Text = "Found : " + r.userData; // show person name & user  
                        exec_report = true;
                        /* setperson method for reports */
                        //label3.Text = "Weapon Detected!!!"ReportModel
                       /* if (label3.Text == "Weapon Detected!!!")
                        {
                            ReportModel.setAll(label3.Text.ToString(),
                                r.userData,GetTimestamp(), setCamera.getSource().ToString());
                        }*/
 
                    }));
                       

                }).Start();
                Task.Delay(new TimeSpan(0, 0,4)).ContinueWith(o => {
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        /* run your code here */
                        startdetection();

                    }).Start();
                });


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button1.Enabled = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
      
        }
        private void browser_Paint(object sender, PaintEventArgs e)
        {
        
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
          
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
          
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}