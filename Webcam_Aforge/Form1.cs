using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Webcam_Aforge
{
   
    public partial class Form1 : Form
    {
        private FilterInfoCollection infoCollection;
        private int webcam1 = -1;
        
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webcamList();
            
            
        }

        private void webcamList()
        {
            infoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            int counter = 0;

            foreach(FilterInfo videoCapture in infoCollection)
            {
                counter = counter + 1;
                cWebcamList.Items.Add(videoCapture.Name + " " + counter.ToString());
            }
        }

        private void connectWebcam()
        {
            webcam1 = cWebcamList.SelectedIndex;
            try
            {
                if (webcam1 != -1)
                {
                    int index = webcam1;
                    videoSourcePlayer1.VideoSource = (IVideoSource)new VideoCaptureDevice(infoCollection[index].MonikerString);
                    videoSourcePlayer1.Start();
                    bGetPict.Enabled = true;

                }
                else
                {
                    MessageBox.Show("Please select Webcam !");
                }
            }
            catch (Exception e)
            {

            }
        }

        private void bConnect_Click(object sender, EventArgs e)
        {
            connectWebcam();
        }

        private void closeForm()
        {
            try
            {
                if (videoSourcePlayer1.IsRunning)
                {
                    videoSourcePlayer1.SignalToStop();
                    videoSourcePlayer1.WaitForStop();
                    videoSourcePlayer1.Stop();
                }
            }
            catch (Exception e)
            {

            }
            this.Dispose();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            closeForm();
            System.Windows.Forms.Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeForm();
        }

        private void bGetPict_Click(object sender, EventArgs e)
        {
            Bitmap b = null;
            try
            {
                b = videoSourcePlayer1.GetCurrentVideoFrame();
                if (b != null)
                {
                    pictureBox1.Image = b;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    bSavePict.Enabled = true;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void bSavePict_Click(object sender, EventArgs e)
        {
            string picName = "Picture1";
            try
            {
                if (pictureBox1.Image != null)
                {
                    using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                    {
                        fbd.ShowDialog();
                        string recPath = fbd.SelectedPath;
                        pictureBox1.Image.Save(recPath + @"\" + picName + ".jpg", ImageFormat.Jpeg);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
