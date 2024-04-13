namespace ImageProcessing
{
    public partial class Form1 : Form
    {
        private Bitmap? img1;
        private Bitmap? img2;
        private Bitmap? img3;
        private Bitmap? img4;
        private Bitmap? img5;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            var file = openFileDialog1.FileName;
            if (file != null)
            {
                img1 = new Bitmap(file);
                img2 = new Bitmap(file);
                img3 = new Bitmap(file);
                img4 = new Bitmap(file);
                img5 = new Bitmap(file);
                pictureBox1.Image = img1;
                pictureBox2.Image = img2;
                pictureBox3.Image = img3;
                pictureBox4.Image = img4;
                pictureBox5.Image = img5;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var ToMono = (Color pix) =>
            {
                return (pix.R + pix.G + pix.B)/3;
            };
            Parallel.For(0, 4, z =>
            {
                int temp;
                var color = Color.Black;
                Color pixel;
                
                switch (z)
                {
                    case 0:
                        if (img2 == null) { return; }
                        for (int i = 0; i < img2.Height; i++)
                        {
                            for (int j = 0; j < img2.Width; j++)
                            {
                                pixel = img2.GetPixel(j, i);
                                color = Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                                img2.SetPixel(j, i, color);
                            }
                        }
                        pictureBox2.Image = img2;
                        break;
                    case 1:
                        if (img3 == null) { return; }
                        for (int i = 0; i < img3.Height; i++)
                        {
                            for (int j = 0; j < img3.Width / 2; j++)
                            {
                                color = img3.GetPixel(j, i);
                                img3.SetPixel(j, i, img3.GetPixel(img3.Width - 1 - j, i));
                                img3.SetPixel(img3.Width - 1 - j, i, color);
                            }
                        }
                        pictureBox3.Image = img3;
                        break;
                    case 2:
                        if (img4 == null) { return; }
                        for (int i = 0; i < img4.Height; i++)
                        {
                            for (int j = 0; j < img4.Width; j++)
                            {
                                color = img4.GetPixel(j, i);
                                temp = (color.R + color.G + color.B) / 3;
                                img4.SetPixel(j, i, Color.FromArgb(temp, temp, temp));
                            }
                        }
                        pictureBox4.Image = img4;
                        break;

                    case 3:
                        if (img5 == null) { return; }
                        for (int i = 0; i < img5.Height; i++)
                        {
                            for (int j = 0; j < img5.Width; j++)
                            {
                                if (j == 0)
                                {
                                    img5.SetPixel(j, i, Color.FromArgb(0, 0, 0));
                                }
                                else if (j == img5.Width - 1)
                                {
                                    img5.SetPixel(j, i, Color.FromArgb(0, 0, 0));
                                }
                                else
                                {
                                    temp = Math.Abs(ToMono(img1.GetPixel(j - 1, i)) - ToMono(img1.GetPixel(j + 1, i)));
                                    if (temp > 50) temp = 255;
                                    else temp = 0;
                                    img5.SetPixel(j, i, Color.FromArgb(temp, temp, temp));

                                }
                            }
                        }
                        for (int i = 0; i < img5.Height; i++)
                        {
                            for (int j = 0; j < img5.Width; j++)
                            {
                                if (i == 0)
                                {
                                    img5.SetPixel(j, i, Color.FromArgb(0, 0, 0));
                                }
                                else if (i == img5.Height - 1)
                                {
                                    img5.SetPixel(j, i, Color.FromArgb(0, 0, 0));
                                }
                                else
                                {
                                    temp = Math.Abs(ToMono(img1.GetPixel(j, i - 1)) - ToMono(img1.GetPixel(j, i + 1)));
                                    if (temp > 50) temp = 255;
                                    else temp = 0;
                                    if (img5.GetPixel(j, i).R == 255)
                                        temp = 255;
                                    img5.SetPixel(j, i, Color.FromArgb(temp, temp, temp));

                                }
                            }
                        }
                        pictureBox5.Image = img5;
                        break;

                }
            });
            






           


            

        }
    }
}
