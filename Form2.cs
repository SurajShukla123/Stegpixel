using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MailMessage mail = new MailMessage(from.Text, to.Text, subject.Text, body.Text);
                mail.Attachments.Add(new Attachment(attach.Text.ToString()));
                if (smtp.SelectedItem.ToString() == "smtp.gmail.com")
                {
                    SmtpClient client = new SmtpClient(smtp.SelectedItem.ToString());
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(user.Text, pass.Text);
                    client.EnableSsl = true;
                    client.Send(mail);
                    MessageBox.Show("Email Sent !!", "Success", MessageBoxButtons.OK);
                }
                else if (smtp.SelectedItem.ToString() == "smtp.outlook.com")
                {
                    SmtpClient client = new SmtpClient(smtp.SelectedItem.ToString());
                    client.Port = 465;
                    client.Credentials = new NetworkCredential(user.Text, pass.Text);
                    client.EnableSsl = true;
                    client.Send(mail);
                    MessageBox.Show("Email Sent !!", "Success", MessageBoxButtons.OK);
                }
                else if (smtp.SelectedItem.ToString() == "smtp.yahoo.com")
                {
                    SmtpClient client = new SmtpClient(smtp.SelectedItem.ToString());
                    client.Port = 25;
                    client.Credentials = new NetworkCredential(user.Text, pass.Text);
                    client.EnableSsl = true;
                    client.Send(mail);
                    MessageBox.Show("Email Sent !!", "Success", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Select Proper SMTP PROTOCOL");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog g = new OpenFileDialog();
            g.Title = "Select File";
            if (g.ShowDialog() == DialogResult.OK)
            {
                string path = g.FileName.ToString();
                attach.Text = path;
            }
        }
    }
}
