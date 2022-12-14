using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sqlBackup
{
    public partial class Form1 : Form
    {
        private sqlBackup mSql = new sqlBackup();

        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            DateTime dateTime = DateTime.Now;
            string format = "HH:mm";
            InitializeComponent();
            AppSettings.InitializeAppSettings();
            listBox1.Items.Add("------------------ Yedekleme Hizmeti Başladı (" + dateTime.ToString(format) + ") ------------------ \r\n");
            listBox1.Items.Add("------------------ Saat (" + dateTime.Hour.ToString() + ") ------------------ \r\n");
            TimerGuncelle.Interval = 1 * (1000 * 1);
            TimerGuncelle.Start();
            Application.DoEvents();
        }

        private void TimerGuncelle_TickAsync(object sender, EventArgs e)
        {
            Application.DoEvents();
            TimerGuncelle.Stop();
            mailKontrol mk = new mailKontrol();
            try
            {
  
                listBox1.Items.Add("Güncelle 1 \r\n");
                foreach (AppSettings.DatabaseList dbL in AppSettings.config.Configuration.databaseList)
                {
                    mk = mSql.yedekAl_v2(dbL.databaseName, dbL.email1);
                    if (mk.snc > 0)
                    {
                        listBox1.Items.Add(mk.message + "\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                listBox1.Items.Add("Bir hata oluştu lütfen tekrar deneyiniz.");
                LogWrite(ex.Message);
            }
            TimerGuncelle.Interval = 30 * 50000 * 60;
            TimerGuncelle.Start();
            listBox1.Items.Add("Güncelle2 \r\n");
            Application.DoEvents();
        }

        public void LogWrite(string logMessage)
        {
            string m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log.txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }

    }
}
