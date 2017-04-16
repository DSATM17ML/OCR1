using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.Imaging;
using Accord.Imaging.Filters;
using Accord.Imaging.Converters;

namespace ocr1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        Bitmap original;
        Bitmap thresholdimage;
        Bitmap grayimage;
        Bitmap newImage;
        Bitmap newImage1;
        Bitmap newImage3;
        Bitmap newImage4;
        Bitmap invertimage;
        Bitmap invertimage1;
        Bitmap blobimage;
        int count=0;
        Bitmap invertimage2;
        Bitmap invertimage3;
        Bitmap blobimage1;

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd1 = new OpenFileDialog();
            if (fd1.ShowDialog() == DialogResult.OK)
            {
                Bitmap img = new Bitmap(fd1.FileName);
                img.GetPixel(10, 5);
                original = img;
                pictureBox1.Image = original;
            }
        }

        private void segmentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Grayscale grayscale = new GrayscaleBT709();
            grayimage = grayscale.Apply(original);
            pictureBox2.Image = grayimage;
            Threshold bw = new Threshold();
            thresholdimage = bw.Apply(grayimage);
            pictureBox2.Image = thresholdimage;
          //  ResizeBicubic resize = new ResizeBicubic(200, thresholdimage.Height);
          //  thresholdimage = resize.Apply(thresholdimage);
            List<int> xC = new List<int> ();
            int xPrev = 0;
            for (int i = 0; i < thresholdimage.Width; i++)
            {
                count = 0;
                for (int j = 0; j < thresholdimage.Height; j++)
                {
                    Color p = thresholdimage.GetPixel(i, j);
                    if (p.R == 0 && p.G == 0 && p.B == 0)
                    {
                        count++;
                    }
                  
                }
                if(count != 0) 
                {
                    if (xC.Count == 0)
                    {
                        xC.Add(i);
                    }
                    else
                    {
                        if (i - xPrev > 1)
                        {
                            xC.Add(i);
                        }
                    }
                    xPrev = i;
                }
            }
            List<Bitmap> ni = new List<Bitmap>();
            for (int i = 0; i <xC.Count; i++)
            {
                int endbound;
                if (i + 1 >= xC.Count)
                {
                    endbound = thresholdimage.Width;
                }
                else
                {
                    endbound = xC[i + 1];
                }
               Crop cr = new Crop(new Rectangle(xC[i], 0, endbound - xC[i], thresholdimage.Height));
                 newImage = cr.Apply(thresholdimage);
                    ni.Add(newImage);
                    pictureBox4.Image = newImage;
                  //  newImage.Save(@"C:\users\rahul\desktop\sampleImage.png");
                }

            Invert inv = new Invert();
            invertimage = inv.Apply(newImage);
            pictureBox5.Image = invertimage;

            ExtractBiggestBlob ebb = new ExtractBiggestBlob();
            blobimage = ebb.Apply(invertimage);
            pictureBox5.Image = blobimage;

            Invert inv1 = new Invert();
            invertimage1 = inv1.Apply(blobimage);
            pictureBox5.Image = invertimage1;

            List<int> yC = new List<int>();
            int xPrev1 = 0;
            for (int j = 0; j < thresholdimage.Height; j++)
            {
                count = 0;
                for (int i = 0; i < thresholdimage.Width; i++)
                {
                    Color p = thresholdimage.GetPixel(i, j);
                    if (p.R == 0 && p.G == 0 && p.B == 0)
                    {
                        count++;
                    }

                    if (count != 0)
                    {
                        if (yC.Count == 0)
                        {
                            yC.Add(j);
                        }
                        else
                        {
                            if (j - xPrev1 > 1)
                            {
                                yC.Add(j);
                            }
                        }
                        xPrev1 = j;
                    }
                }
            }
            List<Bitmap> ni1 = new List<Bitmap>();
            for (int j = 0; j < yC.Count; j++)
            {
                int endbound;
                if (j + 1 >= yC.Count)
                {
                    endbound = thresholdimage.Height;
                }
                else
                {
                    endbound = yC[j + 1];
                }
                Crop cr = new Crop(new Rectangle(0, yC[j], thresholdimage.Height, endbound - yC[j]));
                newImage3 = cr.Apply(thresholdimage);
                ni1.Add(newImage3);
                pictureBox3.Image = newImage3;
                // newImage.Save(@"C:\users\rahul\desktop\sampleImage1.png");
            }
            Invert inv2 = new Invert();
            invertimage3 = inv2.Apply(newImage3);
            pictureBox6.Image = invertimage3;

            ExtractBiggestBlob ebb1 = new ExtractBiggestBlob();
            blobimage1 = ebb1.Apply(invertimage3);
            pictureBox6.Image = blobimage1;

            Invert inv3 = new Invert();
            invertimage2 = inv3.Apply(blobimage1);
            pictureBox6.Image = invertimage2; 
        }                
    }
 }
