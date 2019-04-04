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
using System.Security.Cryptography;

namespace CryptoPower
{
    public partial class Form1 : Form
    {
        public static string pathToFile = "";
        public static string encrypt = "";
        public static string decrypt = "";

        public string UTF8 (string text)
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(text);
            return Encoding.UTF8.GetString(utf8Bytes);
        }
    
        private static byte[] EncryptString(byte[] clearText, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(clearText, 0, clearText.Length);
            cs.Close();
            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        }

        public static string EncryptString1(string passw, string master)
        {
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(passw);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(master, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            byte[] encryptedData = EncryptString(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return Convert.ToBase64String(encryptedData);
        }

        private static byte[] DecryptString(byte[] cipherData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherData, 0, cipherData.Length);
            cs.Close();
            byte[] decryptedData = ms.ToArray();
            return decryptedData;
        }

        public static string DecryptString1(string passw, string master)
        {
            byte[] cipherBytes = Convert.FromBase64String(passw);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(master, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            byte[] decryptedData = DecryptString(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            encrypt = EncryptString1(rtbtext.Text, tbkey.Text);
            rtbencrypt.Text = encrypt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();

            save.FileName = "DefaultOutputName.txt";

            save.Filter = "Text File (*.txt)| *.txt";

            if (save.ShowDialog() == DialogResult.OK)

            {

                StreamWriter writer = new StreamWriter(save.OpenFile(), Encoding.UTF8);

                writer.Write(rtbencrypt.Text);

                writer.Dispose();

                writer.Close();

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }


        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Wählen sie das Dokument zum öffnen";
            theDialog.Filter = "Txt files (*.txt)|*.txt";
            //theDialog.InitialDirectory = @"C:\";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Datei geöffnet", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pathToFile = theDialog.FileName;//doesn't need .tostring because .filename returns a string// saves the location of the selected object

            }

            if (File.Exists(pathToFile))// only executes if the file at pathtofile exists//you need to add the using System.IO reference at the top of te code to use this
            {
                string texta = "";
                string texts = "";
                using (StreamReader sr = new StreamReader(pathToFile, System.Text.Encoding.UTF8,true))
                {
                    texts = sr.ReadToEnd();//all text wil be saved in text enters are also saved
                    texta = UTF8(texts);
                }
                rtbtext.Text = texta;
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            decrypt = DecryptString1(rtbtext.Text, tbkey.Text);
            rtbencrypt.Text = decrypt;
        }
    }
}
