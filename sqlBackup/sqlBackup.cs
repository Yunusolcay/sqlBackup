using DatabaseBackup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sqlBackup
{
    class sqlBackup
    {

        SqlConnection con = new SqlConnection();
        SqlCommand sqlcmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        FileZipper filezip = new FileZipper();

        public mailKontrol yedekAl_v2(string dbName,string email)
        {
            mailKontrol mk = new mailKontrol();
            bool backupSuccess = false;
            bool zipSuccess = false;
            string databaseServer = AppSettings.config.Configuration.databaseServer;
            string databaseUserID = AppSettings.config.Configuration.databaseUserID;
            string databasePassword = AppSettings.config.Configuration.databasePassword;
            string folderName = AppSettings.config.Configuration.backupLocation;
            string backupFolder = AppSettings.config.Configuration.backupFolder;
            string fileName = string.Format("yedekx_{0}.bak", DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime startTime = DateTime.Now;
            string fullpath = Path.Combine(backupFolder, folderName);
            string fullFileName = fullpath + "\\" + fileName;


            foreach (AppSettings.DatabaseList dbL in AppSettings.config.Configuration.databaseList)
            {
                try
                {
                    if (dbL.databaseName == dbName)
                    {
                        using (SqlConnection con = new SqlConnection(
             string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}",
             databaseServer, dbL.databaseName, databaseUserID, databasePassword)))
                        {
                            con.Open();
                            fullFileName = fullpath + "\\" + fileName.Replace(".bak", "") + "\\" + fileName;
                            string zipfullFileName = fullpath + "\\" + fileName.Replace(".bak", "") + "\\zip\\" + fileName;
                            fullFileName = fullFileName.Replace("yedekx", dbL.databaseName);
                            zipfullFileName = zipfullFileName.Replace("yedekx", dbL.databaseName);
                            string createFolderName = fullFileName.Replace(fileName.Replace("yedekx", dbL.databaseName), "");
                            bool exists = System.IO.Directory.Exists(createFolderName);
                            if (!exists)
                                System.IO.Directory.CreateDirectory(createFolderName);
                            exists = System.IO.Directory.Exists(createFolderName + @"\zip");
                            if (!exists)
                                System.IO.Directory.CreateDirectory(createFolderName + @"\zip");
                            if (File.Exists(fullFileName) == false)
                            {
                                using (SqlCommand command = new SqlCommand(string.Format("BACKUP DATABASE [{0}] TO  DISK = N'{1}' WITH FORMAT, INIT,  NAME = N'DB-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10", dbL.databaseName, fullFileName), con))
                                {
                                    command.CommandTimeout = 0;
                                    command.ExecuteNonQuery();
                                    backupSuccess = true;
                                    filezip.Zip(zipfullFileName, createFolderName, dbL.partMB);
                                    zipSuccess = true;
                                    mk.snc = 1;
                                    mk.message = MailGonder(zipfullFileName, createFolderName,email);
                                }
                            }
                        }
                    }
                   
                }
                catch (Exception e)
                {
                    backupSuccess = false;
                    LogWrite(e.Message);
                    mk.snc = 1;
                    mk.message = e.Message;
                }
            }
            return mk;
        }

        public string MailGonder(string fileName,string folder,string gidecekEmail)
        {

            string bdy = "<html><font face='Arial'>";
            string sbj = "";
            bdy += "Yedek dosyası : " + fileName + " <br/>Yedek dosyası : " + folder;
            bdy += "</font></html>";
            return _MailGonder(bdy, sbj, gidecekEmail, folder,"");


        }
        string _MailGonder(string bdy, string sbj, string email, string folder, string baslik = "Günlük yedekleme")
        {
            try
            {
                if (email.Length > 7)
                {
                    NetworkCredential crd = new NetworkCredential("test@gmail.com", "123");
                    MailAddress adr = new MailAddress("test@gmail.com", baslik);
                    MailMessage msg = new MailMessage();
                    msg.To.Add(email);
                    msg.Subject = baslik;
                    msg.From = adr;
                    msg.Body = bdy;
                    msg.IsBodyHtml = true;
                    System.Net.Mail.Attachment attachment;
                    string[] fileArray = Directory.GetFiles(folder + "\\zip");
                    foreach(string fileName in fileArray)
                    {
                        attachment = new System.Net.Mail.Attachment(fileName);
                        msg.Attachments.Add(attachment);
                    }
                   
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = crd;
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Send(msg);
                }
                return folder + " başarıyla yedek alındı";
            }
            catch (SmtpException ex)
            {
                return ex.Message;
            }
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

    public class mailKontrol
    {
        public int snc { get; set; }
        public string message { get; set; }
    }


}
