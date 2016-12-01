using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Point empty = new Point(256, 256);
        System.Collections.ArrayList alist = new System.Collections.ArrayList();
        Point[] locP = new Point[] {
            new Point(10,10),new  Point(92,10), new Point(174,10), new Point(256,10),
            new Point(10,92),new  Point(92,92), new Point(174,92), new Point(256,92),
            new Point(10,174),new Point(92,174),new Point(174,174),new Point(256,174),
            new Point(10,256),new Point(92,256),new Point(174,256),new Point(256,256)};
        Bitmap mPic;
        System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
        bool paused = false, started = true;
        float width;
        float height;

        private bool isTouchingEmpty(Point picL)
        {
            int SPACE = 82;
            bool b = false;
            if ((picL.X == empty.X + SPACE || picL.X == empty.X - SPACE) ||
               (picL.Y == empty.Y + SPACE || picL.Y == empty.Y - SPACE))
            {
                if ((picL.X == empty.X) && (picL.Y != empty.Y)
                    || (picL.Y == empty.Y) && (picL.X != empty.X))
                    b = true;
            }
            return b;
        }

        public Form1()
        {
            InitializeComponent();
            alist.Add(pictureBox1);
            alist.Add(pictureBox2);
            alist.Add(pictureBox3);
            alist.Add(pictureBox4);
            alist.Add(pictureBox5);
            alist.Add(pictureBox6);
            alist.Add(pictureBox7);
            alist.Add(pictureBox8);
            alist.Add(pictureBox9);
            alist.Add(pictureBox10);
            alist.Add(pictureBox11);
            alist.Add(pictureBox12);
            alist.Add(pictureBox13);
            alist.Add(pictureBox14);
            alist.Add(pictureBox15);
            mPic = (Bitmap)new ComponentResourceManager(typeof(Form1)).GetObject("Untitled-2");
            mPic = new Bitmap(mPic, new Size(376, 376));
            width = (float)(mPic.Width / 4.0);
            height = (float)(mPic.Height / 4.0);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g;
            RectangleF rec = new RectangleF(0f, 0f, width, height);
            int index = 0;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    try
                    {
                        g = ((PictureBox)(alist[index])).CreateGraphics();
                        ((PictureBox)(alist[index])).Refresh();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        g = pictureBox16.CreateGraphics();
                        pictureBox16.Refresh();
                        rec.Location = new PointF(x * width, y * height);
                        g.DrawImage(mPic, 0, 0, rec, GraphicsUnit.Pixel);
                        return;
                    }
                    rec.Location = new PointF(x * width, y * height);
                    g.DrawImage(mPic, 0, 0, rec, GraphicsUnit.Pixel);
                    index++;
                }
            }
        }

        private void openButton(object sender, EventArgs e)
        {
            st.Stop();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".jpg";
            ofd.Filter = "Picture files (*.jpg)(*.jpeg)(*.gif)(*.png)|*.jpg|All files (*.*)|*.*";
            if (ofd.ShowDialog() != DialogResult.Cancel)
                mPic = new Bitmap(ofd.OpenFile());
            else
                mPic = (Bitmap)new ComponentResourceManager(typeof(Form1)).GetObject("Untitled-2");
            mPic = new Bitmap(mPic, new Size(376, 376));
            width = (float)(mPic.Width / 4.0);
            height = (float)(mPic.Height / 4.0);
        }

        private void pic_MouseEnter(object sender, EventArgs e)
        {
            PictureBox temp = (PictureBox)sender;
            if (isTouchingEmpty(temp.Location))
            {
                Point p = temp.Location;
                Point t = temp.Location;
                for (int x = 0; x < 20; x++)
                {
                    System.Threading.Thread.Sleep(10);
                    TimeSpan ts = st.Elapsed;
                    label2.Text = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                    label2.Refresh();
                    if (temp.Location.X < empty.X)
                    {
                        t.X += 3;
                        temp.Location = t;
                    }
                    else
                    {
                        t.X -= 3;
                        temp.Location = t;
                    }
                    if (temp.Location.Y < empty.Y)
                    {
                        t.Y += 3;
                        temp.Location = t;
                    }
                    else
                    {
                        t.Y -= 3;
                        temp.Location = t;
                    }
                }
                temp.Location = empty;
                empty = p;
            }
            for (int x = 0; x < alist.Count; x++)
            {
                if (!((PictureBox)(alist[x])).Location.Equals(locP[x]))
                {
                    st.Start();
                    pictureBox16.Visible = false;
                    label1.Visible = false;
                    return;
                }
            }
            pictureBox16.Visible = true;
            label1.Visible = true;
            st.Stop();
        }

        private void Shuffle(object sender, EventArgs e)
        {
            st.Stop();
            st.Reset();
            Random r = new Random();
            Point[] usedP = new Point[locP.Length];
            for (int x = 0; x < usedP.Length; x++)
                usedP[x] = new Point(0, 0);
            int index = 0;
            foreach (PictureBox pic in alist)
            {
                if (started)
                    pic.MouseEnter += new EventHandler(pic_MouseEnter);
                int x;
            here:
                x = r.Next(0, 16);
                for (int sub = 0; sub < usedP.Length; sub++)
                {
                    if (locP[x] == usedP[sub])
                        goto here;
                }
                System.Threading.Thread.Sleep(40);
                usedP[index++] = locP[x];
                pic.Location = locP[x];
            }
            started = false;
            foreach (PictureBox pic in alist)
            {
                for (int x = 0; x < locP.Length; x++)
                {
                    if (pic.Location.Equals(locP[x]))
                        usedP[x] = new Point(0, 0);
                }
            }
            for (int x = 0; x < locP.Length; x++)
            {
                if (!(usedP[x].Equals(new Point(0, 0))))
                {
                    empty = locP[x];
                    break;
                }
            }
        }

        private void solButton_Click(object sender, EventArgs e)
        {
            st.Stop();
            st.Reset();
            for (int x = 0; x < alist.Count; x++)
            {
                if (((PictureBox)(alist[x])).Location != locP[x])
                {
                    ((PictureBox)(alist[x])).Location = locP[x];
                    System.Threading.Thread.Sleep(150);
                }
            }
            empty = new Point(256, 256);
            pictureBox16.Visible = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = st.Elapsed;
            label2.Text = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        }

        private void pButton_Click(object sender, EventArgs e)
        {
            if (paused)
            {
                paused = false;
                st.Start();
                solButton.Enabled = true;
                ranButton.Enabled = true;
            }
            else
            {
                paused = true;
                solButton.Enabled = false;
                ranButton.Enabled = false;
                st.Stop();
            }
            foreach (PictureBox pic in alist)
                pic.Visible = !paused;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            for (int x = 0; x < 4; x++)
            {
                pictureBox16.Visible = true;
                pictureBox16.Refresh();
                Application.DoEvents();
                System.Threading.Thread.Sleep(500);
                pictureBox16.Visible = false;
                System.Threading.Thread.Sleep(500);
            }
            Shuffle(new object(), EventArgs.Empty);
            timer1.Enabled = true;
            Application.DoEvents();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("this is a test");
        }
    }
}
