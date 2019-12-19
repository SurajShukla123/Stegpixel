using System; 
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Fileencrypt
{
    public partial class FileEncryptDEcrypt : Form
    {
        byte[] abc;
        byte[,] table;
        public FileEncryptDEcrypt()
        {
            InitializeComponent();
        }

        private void btBrowse_Click(object sender, EventArgs e) // called by API on clicking browse button
        {
            OpenFileDialog od = new OpenFileDialog();
            od.Multiselect = false;
            if (od.ShowDialog() == DialogResult.OK)
            {
                tbPath.Text = od.FileName;
            }
        }

        private void rbEncrypt_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEncrypt.Checked)
            {
                rbDecrypt.Checked = false;
            }
        }

        private void rbDecrypt_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDecrypt.Checked)
                rbEncrypt.Checked = false;
        }

        private void Form1_Load(object sender, EventArgs e) // called from API on clicking button event
        {
            rbEncrypt.Checked = true;
            // initialize abc and table- these data structures are used apply the LSB algorithm and it stores bytes for future use.
            abc = new byte[256];     //1D Byte array
            for (int i = 0; i < 256; i++)
                abc[i] = Convert.ToByte(i);
            table = new byte[256, 256];    //2D Byte array or matrix
            for (int i = 0; i < 256; i++)
                for (int j = 0; j < 256; j++)
                {
                    table[i, j] = abc[(i + j) % 256];
                }
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            //Check input values
            if (!File.Exists(tbPath.Text))
            {
                MessageBox.Show("File does not Exist.");
                return;
            }
            if (String.IsNullOrEmpty(tbPassword.Text))
            {
                MessageBox.Show("Password empty. Please enter your password");
                return;
            }
            // Get file content and key for encrypt/decrypt
            try
            {
                byte[] fileContent = File.ReadAllBytes(tbPath.Text);  //Data file to be encrypted is stored in fileContent
                byte[] passwordTmp = Encoding.ASCII.GetBytes(tbPassword.Text); //Password is stored in passwordTmp for temperory use.
                byte[] keys = new byte[fileContent.Length];  //Array of keys for encryption
                for (int i = 0; i < fileContent.Length; i++)  //generating keys using password dynamically
                    keys[i] = passwordTmp[i % passwordTmp.Length]; 
                // Encrypt
                byte[] result = new byte[fileContent.Length];
                if (rbEncrypt.Checked)  // if file is checked then encrypt the data
                {
                    for (int i = 0; i < fileContent.Length; i++)
                    {
                        byte value = fileContent[i]; //temporarily store i'th file content location in a new byte
                        byte key = keys[i];  //keys of i'th position in byte array
                        int valueIndex = -1, keyIndex = -1;  //store -1 to valueIndex and keyIndex for exception handling
                        for (int j = 0; j < 256; j++)
                            if (abc[j] == value) // if content of file at location 'i' equal to the 'j'th location of abc
                            {
                                valueIndex = j;  //store j into valueIndex for searching from Table to store intro encrypted data
                                break;
                            }
                        for (int j = 0; j < 256; j++)
                            if (abc[j] == key)  // if content of key at location 'i' equal to the 'j'th location of abc
                            {
                                keyIndex = j;  //store j into keyIndex for searching from Table to store intro encrypted data
                                break;
                            }
                        result[i] = table[keyIndex, valueIndex]; // fetch data from Table matrix and store it as encrypted data for i'th bit
                    }
                }
                //Decrypt - if file is uncheked then decrypt the data
                // the entire process is reverse of encryption methon
                else
                {
                    for (int i = 0; i < fileContent.Length; i++)
                    {
                        byte value = fileContent[i];
                        byte key = keys[i];
                        int valueIndex = -1, keyIndex = -1;
                        for (int j = 0; j < 256; j++)
                            if (abc[j] == key)
                            {
                                keyIndex = j;
                                break;
                            }
                        for (int j = 0; j < 256; j++)
                            if (table[keyIndex, j] == value)
                            {
                                valueIndex = j;
                                break;
                            }
                        result[i] = abc[valueIndex];
                    }
                }
                // Save result to new file with the same extension - endrypted file and new file should have same extension
                String fileExt = Path.GetExtension(tbPath.Text); // store extension of file in fileExt
                SaveFileDialog sd = new SaveFileDialog();
                sd.Filter = "Files (*" + fileExt + ") | *" + fileExt; // genrate regex for new file name
                if (sd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(sd.FileName, result); //writing data of result into new file 
                }
            }
            catch
            {
                MessageBox.Show("File is in use. Close other program is using this file and try again.");
                return;
            }

        }
    }
}
