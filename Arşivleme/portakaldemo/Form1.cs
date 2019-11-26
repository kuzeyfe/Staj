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
using System.Security.Cryptography;

namespace portakaldemo
{
    public partial class Form1 : Form
    {
        NotifyIcon MyIcon = new NotifyIcon();
        StreamReader sr = null;
        StreamWriter sr2 = null;

        public Form1()
        {
            InitializeComponent();
        }
        static string loglar = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\loglar.txt";
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Önce Renk, Sonra Yazı Tipi Seçiniz");
            ColorDialog renk = new ColorDialog();
            renk.ShowDialog();
            BackColor = renk.Color;
            FontDialog fontum = new FontDialog();
            fontum.ShowDialog();
            Font = fontum.Font;

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

        static string secilen = "";
        private string gercek;

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Klasor = new FolderBrowserDialog();
            Klasor.ShowDialog();
            secilen = Klasor.SelectedPath;
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
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(hedef))
                {
                   
                }
            }
            timer1.Interval = 5000;
            timer1.Enabled = false;
            timer1.Start();
            FileSystemWatcher degisim = new FileSystemWatcher();
            degisim.Path = secilen;
            degisim.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            degisim.Filter = "*.*";
            degisim.Changed += degisim_Changed;
            degisim.Created += degisim_Created;
            degisim.Deleted += degisim_Deleted;
            degisim.Renamed += degisim_Renamed;
            // Begin watching.
            degisim.IncludeSubdirectories = true;
            degisim.EnableRaisingEvents = true;
        }
        private void Copy(DirectoryInfo oOriginal, DirectoryInfo oFinal)
        {
            foreach (DirectoryInfo oFolder in oOriginal.GetDirectories())
                this.Copy(oFolder, oFinal.CreateSubdirectory(oFolder.Name));

            foreach (FileInfo oFile in oOriginal.GetFiles())
                oFile.CopyTo(oFinal.FullName + @"\" + DateTime.Now.ToFileTime() + oFile.Name, true);
        }

        private void degisim_Renamed(object sender, RenamedEventArgs e)
        {          
            try
            {
                string mFile = DateTime.Now.ToString("MM-dd-yy HH-mm-ss=") + e.Name;
                if (File.Exists(mFile) == false)
                {
                    File.Copy(Path.Combine(secilen, e.Name), Path.Combine(hedef, mFile));
                    //this.Copy(new DirectoryInfo(secilen), new DirectoryInfo(hedef));
                }
                File.AppendAllText(loglar, $"{e.Name} - dosyası:" + DateTime.Now.ToString() + ": " + "tarihinde adı değişti.\n");
                MyIcon.ShowBalloonTip(1, "Uyarı", $"{e.Name} dosyasının adı değişti!", ToolTipIcon.Info);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr2.Close();
                    this.Close();
                }
            }  
        }

        private void degisim_Deleted(object sender, FileSystemEventArgs e)
        {
            try
            {
                File.AppendAllText(loglar, $"{e.Name} - dosyası:" + DateTime.Now.ToString() + ": " + "tarihinde silindi.\n");
                MyIcon.ShowBalloonTip(1, "Uyarı", $"{e.Name} dosysı silindi!", ToolTipIcon.Info);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr2.Close();
                    this.Close();
                }
            }
        }
        private void degisim_Changed(object sender, FileSystemEventArgs e)
        {
             try
                {
                string mFile = DateTime.Now.ToFileTime() + e.Name;
                if (File.Exists(mFile) == false)
                {
                    File.Copy(Path.Combine(secilen, e.Name), Path.Combine(hedef, DateTime.Now.ToFileTime() + e.Name));
                }
                File.AppendAllText(loglar, $"{e.Name} - dosyası:" + DateTime.Now.ToString() + ": " + "tarihinde içeriği değişti.\n");
                MyIcon.ShowBalloonTip(1, "Uyarı", $"{e.Name} dosyasında yeni bir değişiklik var!", ToolTipIcon.Info);
                
                }
             finally
             {
                if (sr != null)
                {
                    sr.Close();
                    sr2.Close();
                    this.Close();
                }
             }
        }
        private void degisim_Created(object sender, FileSystemEventArgs e)
        {
             try
                {
                if (File.Exists(DateTime.Now.ToFileTime() + e.Name) != true)
                {
                    File.Copy(Path.Combine(secilen, e.Name), Path.Combine(hedef, DateTime.Now.ToString("yyyyMMddHHmmssffff") + e.Name));
                    File.AppendAllText(loglar, $"{e.Name} - dosyası:" + DateTime.Now.ToString() + ": " + "tarihinde oluşturuldu.\n");
                }
                MyIcon.ShowBalloonTip(1, "Uyarı", $"{e.Name} adında bir dosya oluştu!", ToolTipIcon.Info);
                }
             finally
             {
                if (sr != null)
                {
                    sr.Close();
                    sr2.Close();
                    this.Close();
                }
             }
        }
    }
}