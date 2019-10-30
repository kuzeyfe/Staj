﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Timers;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;

namespace portakaldemo
{
    public partial class Form1 : Form
    {
        static string pathA = secili;
        static string pathB = hedef;
        FileCompare myFileCompare = new FileCompare();
        IEnumerable<System.IO.FileInfo> list1;
        IEnumerable<System.IO.FileInfo> list2;
        bool kıyasla;

        public Form1()
        {
            InitializeComponent();
        }
        NotifyIcon MyIcon = new NotifyIcon();

        string loglar = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\loglar.txt";
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Önce Renk, Sonra Yazı Tipi Seçiniz");
            ColorDialog Renk = new ColorDialog();
            Renk.ShowDialog();
            FontDialog Font = new FontDialog();
            Font.ShowDialog();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Portakal Yazılım A.Ş.");

        }
        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                Hide();
                MyIcon.Visible = true;
                MyIcon.Text = "Portakal";
                MyIcon.BalloonTipTitle = "Program Çalışıyor";
                MyIcon.BalloonTipText = "Program sağ alt köşede konumlandı.";
                MyIcon.ShowBalloonTip(1000);
                MyIcon.MouseDoubleClick += new MouseEventHandler(MyIcon_MouseDoubleClick);
                this.WindowState = FormWindowState.Minimized;

            }
        }

        void MyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
            MyIcon.Visible = true;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //icon
            MyIcon.Icon = new Icon(@"C:\Users\Turkoglu\Desktop\Portakal\123.ico");
            MessageBox.Show("Programı Çalıştırmadan Önce Yedeklemek İstediğiniz Dosyayı Ve Yedekleme Yerinizi Seçiniz!");

        }

        static string secili = "";
        private string gercek;

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Klasor = new FolderBrowserDialog();
            Klasor.ShowDialog();
            secili = Klasor.SelectedPath;
            gercek = Klasor.SelectedPath;

        }

        static string hedef = "";
        private string yedek;

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Klasor = new FolderBrowserDialog();
            Klasor.ShowDialog();
            hedef = Klasor.SelectedPath;
            yedek = Klasor.SelectedPath;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Interval = 5000;
            timer1.Enabled = true;
            timer1.Start();
            MessageBox.Show("Timer Çalışıyor");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            MessageBox.Show("Timer Durduruldu.");
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 5000;
            timer1.Enabled = false;
            timer1.Start();
            System.IO.DirectoryInfo dir1 = new System.IO.DirectoryInfo(secili);
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(hedef);
            list1 = dir1.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            kıyasla = list1.SequenceEqual(list2, myFileCompare);
            if (kıyasla != true)
            {
                Array.ForEach(Directory.GetFiles(hedef, "*.*", SearchOption.AllDirectories), File.Delete);
                MyIcon.ShowBalloonTip(1, "Uyarı", "Yeni bir değişiklik var!", ToolTipIcon.Info);
                this.CopyAll(new DirectoryInfo(secili), new DirectoryInfo(hedef));
                if (!File.Exists(loglar))

                {
                    //string createText = "Hello and Welcome" + Environment.NewLine;
                    File.WriteAllText(loglar, "Log Kayıtları Oluşturuldu. Eski Kayıt Yok!! \n");
                }
                else
                { // This text is always added, making the file longer over time
                  //string appendText = "This is extra text" + Environment.NewLine;
                    File.AppendAllText(loglar, "portakaldemo.zip dosyası:" + DateTime.Now.ToString() + ": " + "tarihinde oluşturulmuştur.\n");
                }
                
            }

        }

        private void CopyAll(DirectoryInfo oOriginal, DirectoryInfo oFinal)
        {
            foreach (DirectoryInfo oFolder in oOriginal.GetDirectories())
                this.CopyAll(oFolder, oFinal.CreateSubdirectory(oFolder.Name));

            foreach (FileInfo oFile in oOriginal.GetFiles())
                oFile.CopyTo(oFinal.FullName + @"\" + oFile.Name, true);
        }
        class FileCompare : System.Collections.Generic.IEqualityComparer<System.IO.FileInfo>
        {
            public FileCompare() { }

            public bool Equals(System.IO.FileInfo f1, System.IO.FileInfo f2)
            {
                return (f1.Name == f2.Name &&
                        f1.Length == f2.Length);
            }
            public int GetHashCode(System.IO.FileInfo fi)
            {
                string s = $"{fi.Name}{fi.Length}";
                return s.GetHashCode();
            }
        }

    }
}