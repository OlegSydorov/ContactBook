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

namespace CSWF_EnCount
{
    public partial class Form1 : Form
    {
        Dictionary<string, List<string>> dataSet;
        int save = 1;
        string pswrd;
        string pswrdBuffer;
        public Form1()
        {
            InitializeComponent();
            dataSet = new Dictionary<string, List<string>>();
            pswrd = "";
            pswrdBuffer = "";


            listView1.View = View.Details;

            listView1.SmallImageList = new ImageList();
            listView1.LargeImageList = new ImageList();
            
            listView1.LargeImageList.Images.Add(Image.FromFile("default.jpg"));
            listView1.SmallImageList.Images.Add(Image.FromFile("default.jpg"));
            listView1.SmallImageList.ImageSize=new Size(50, 75);
           
            listView1.Columns.Add("FULL NAME", 250, HorizontalAlignment.Center);
            listView1.Columns.Add("TELEPHONE NUMBER", 150, HorizontalAlignment.Center);
            listView1.Columns.Add("ADDRESS", 180, HorizontalAlignment.Center);
            listView1.Columns.Add("NOTE", 200, HorizontalAlignment.Center);

            listView1.Groups.Add("FAMILY", "FAMILY");
            listView1.Groups.Add("FRIENDS", "FRIENDS");
            listView1.Groups.Add("COLLEAGUES", "COLLEAGUES");
            listView1.Groups.Add("CLIENTS", "CLIENTS");
            listView1.Groups.Add("SERVICES", "SERVICES");
            listView1.Groups.Add("OTHER", "OTHER");
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem l in listView1.SelectedItems)
            {
                
                EditForm eForm = new EditForm(dataSet[l.Text]);

                dataSet.Remove(l.Text);
                eForm.OnContactEdited += Form_OnContactEdited;

                DialogResult res = eForm.ShowDialog();
                if (res == DialogResult.OK)
                {
                    MessageBox.Show(eForm.Name);
                }
            }
        }

        private void Form_OnContactEdited(object sender, List<string> data)
        {
            
            listView1.SelectedItems[0].SubItems.Clear();
            listView1.SelectedItems[0].Text=data[0];
            listView1.SelectedItems[0].SubItems.Add(data[1]);
            listView1.SelectedItems[0].SubItems.Add(data[2]);
            listView1.SelectedItems[0].SubItems.Add(data[3]);

            MessageBox.Show("Encounter edited!");
            int index = 0;
            for (int i = 0; i < listView1.Groups.Count; i++)
            {
                if (listView1.Groups[i].Name == data[4]) index = i;
            }

            listView1.SelectedItems[0].Group = listView1.Groups[index];

            if (data[5] != "default.jpg")
            {
                listView1.LargeImageList.Images.Add(ImageResize.ResizeImage(Image.FromFile(data[5])));
                listView1.SmallImageList.Images.Add(ImageResize.ResizeSmallImage(Image.FromFile(data[5])));

                int indexPic = listView1.LargeImageList.Images.Count;
                listView1.SelectedItems[0].ImageIndex = indexPic - 1;
            }
            else listView1.SelectedItems[0].ImageIndex = 0;

            dataSet.Add(data[0], data);
            save++;
        }
    

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddForm aForm = new AddForm();
           aForm.OnContactAdded += Form_OnContactAdded;

            DialogResult res = aForm.ShowDialog();
            if (res == DialogResult.OK)
            {
                MessageBox.Show(aForm.Name);
            }
        }

        private void Form_OnContactAdded(object sender, List<string> data)
        {
            ListViewItem item = new ListViewItem(data[0]);

            item.SubItems.Add(data[1]);
            item.SubItems.Add(data[2]);
            item.SubItems.Add(data[3]);
            MessageBox.Show("New encounter added!");
            int index=0;
            for (int i = 0; i < listView1.Groups.Count; i++)
            {
                if (listView1.Groups[i].Name == data[4]) index = i;
            }

            item.Group = listView1.Groups[index];

            if (data[5] != "default.jpg")
            {
                listView1.LargeImageList.Images.Add(Image.FromFile(data[5]));
                listView1.SmallImageList.Images.Add(Image.FromFile(data[5]));
                int indexPic = listView1.LargeImageList.Images.Count;
                item.ImageIndex = indexPic - 1;
            }
            else item.ImageIndex = 0;


            listView1.Items.Add(item);
            dataSet.Add(data[0], data);
            save++;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (var s in listView1.SelectedItems)
            {
                ListViewItem item = new ListViewItem();
                item = s as ListViewItem;
                string name = item.Text;

                ViewForm vf = new ViewForm(dataSet[name]);
                vf.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            while (listView1.SelectedItems.Count > 0)
            {
                dataSet.Remove(listView1.SelectedItems[0].Text);
                listView1.SelectedItems[0].Remove();
            }
            save++;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveEncrypt(dataSet);
            save = 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataSet.Clear();
            listView1.Items.Clear();

            LoadDecrypt();

            //StreamReader stream = new StreamReader("data.txt", Encoding.Default);
            //string line;
            //line = stream.ReadLine();

            //while (line != null)
            //{
            //    List<string> list = new List<string>();
            //    string name=null;
            //    char[] delim = {'|'};
            //    string[] subS = line.Split(delim, StringSplitOptions.RemoveEmptyEntries);
            //    name = subS[0];
            //    list.Add((subS[0] != "_") ? subS[0] : "_");
            //    list.Add((subS[1] != "_")?subS[1]:"_");
            //    list.Add((subS[2] != "_") ? subS[2] : "_");
            //    list.Add((subS[3] != "_") ? subS[3] : "_");
            //    list.Add((subS[4] != "_") ? subS[4] : "_");
            //    list.Add((subS[5] != "default.jpg") ? subS[5] : "default.jpg");
               
            //    dataSet.Add(name, list);

            //    line = stream.ReadLine();
            //}
            //stream.Close();

            //foreach (var d in dataSet)
            //{
            //    ListViewItem item = new ListViewItem(d.Value[0]);

            //    item.SubItems.Add(d.Value[1]);
            //    item.SubItems.Add(d.Value[2]);
            //    item.SubItems.Add(d.Value[3]);
                
            //    int index = 0;
            //    for (int i = 0; i < listView1.Groups.Count; i++)
            //    {
            //        if (listView1.Groups[i].Name == d.Value[4]) index = i;
            //    }

            //    item.Group = listView1.Groups[index];

            //    if (d.Value[5] != "default.jpg")
            //    {
            //        listView1.LargeImageList.Images.Add(Image.FromFile(d.Value[5]));
            //        listView1.SmallImageList.Images.Add(Image.FromFile(d.Value[5]));
            //        int indexPic = listView1.LargeImageList.Images.Count;
            //        item.ImageIndex = indexPic - 1;
            //    }
            //    else item.ImageIndex = 0;

            //    MessageBox.Show(@"New encounter "+d.Value[0]+" added!");
            //    listView1.Items.Add(item);
            //}
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (save >1)
            {
                string message = "Do you want to save the list of encountered?";
                string caption = "Saving to file";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    SaveEncrypt(dataSet);
                }
            }
                this.Close();

        }

        private void SaveEncrypt(Dictionary <string, List<string>> data)
        {

            StreamWriter writer = new StreamWriter("data.txt", false, Encoding.Default);
            foreach (var d in dataSet)
            {
                writer.WriteLine(d.Value[0] + "|" + d.Value[1] + "|" + d.Value[2] + "|" + d.Value[3] + "|" + d.Value[4] + "|" + d.Value[5]);
            }
            writer.Close();

            

            FileStream destFile = File.Create("dataLocked.txt");

            FileStream infile = new FileStream("data.txt", FileMode.Open, FileAccess.Read, FileShare.Read);

            byte[] buffer = new byte[infile.Length];

            infile.Read(buffer, 0, buffer.Length);
            pswrdBuffer = "";
            while (pswrdBuffer == "")
            {
               ShowBox();
                
            }
            pswrd = pswrdBuffer;

            PasswordSave(pswrd);

            RijndaelManaged alg = new RijndaelManaged();
            
            byte[] salt = Encoding.ASCII.GetBytes("1cG11Mq19w");

            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(pswrd, salt);
            alg.Key = key.GetBytes(alg.KeySize / 8);
            alg.IV = key.GetBytes(alg.BlockSize / 8);
            ICryptoTransform encriptor = alg.CreateEncryptor();

            CryptoStream encstream = new CryptoStream(destFile, encriptor, CryptoStreamMode.Write);
            encstream.Write(buffer, 0, buffer.Length);
            encstream.Close();
            infile.Close();
            destFile.Close();
            File.Delete("data.txt");

        }

        private void LoadDecrypt()
        {
            FileStream decryptedFile = File.Create("dataUnLocked.txt");
            FileStream cryptedFile = new FileStream("dataLocked.txt", FileMode.Open, FileAccess.Read, FileShare.Read);

            byte[] salt = Encoding.ASCII.GetBytes("1cG11Mq19w");

            if (pswrdBuffer != "")
            {
                pswrdBuffer = "";
                while (pswrdBuffer != pswrd)
                {
                    ShowBox();
                }
            }
            else
            {
                pswrd = PasswordLoad();
                while (pswrdBuffer != pswrd)
                {
                    ShowBox();
                }
            }


                Rfc2898DeriveBytes key2 = new Rfc2898DeriveBytes(pswrd, salt);
            RijndaelManaged alg = new RijndaelManaged(); 
            alg.Key = key2.GetBytes(alg.KeySize / 8);
            alg.IV = key2.GetBytes(alg.BlockSize / 8);

            ICryptoTransform decriptor = alg.CreateDecryptor();
            CryptoStream decstream = new CryptoStream(cryptedFile, decriptor, CryptoStreamMode.Read);
            int b = decstream.ReadByte();
            while (b != -1)
            {
                decryptedFile.WriteByte((byte)b);
                b = decstream.ReadByte();
            }

            decstream.Close();
            cryptedFile.Close();
            decryptedFile.Close();

            StreamReader stream = new StreamReader("dataUnLocked.txt", Encoding.Default);
            string line;
            line = stream.ReadLine();

            while (line != null)
            {
                List<string> list = new List<string>();
                string name = null;
                char[] delim = { '|' };
                string[] subS = line.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                name = subS[0];
                list.Add((subS[0] != "_") ? subS[0] : "_");
                list.Add((subS[1] != "_") ? subS[1] : "_");
                list.Add((subS[2] != "_") ? subS[2] : "_");
                list.Add((subS[3] != "_") ? subS[3] : "_");
                list.Add((subS[4] != "_") ? subS[4] : "_");
                list.Add((subS[5] != "default.jpg") ? subS[5] : "default.jpg");

                dataSet.Add(name, list);

                line = stream.ReadLine();
            }
            stream.Close();

            foreach (var d in dataSet)
            {
                ListViewItem item = new ListViewItem(d.Value[0]);

                item.SubItems.Add(d.Value[1]);
                item.SubItems.Add(d.Value[2]);
                item.SubItems.Add(d.Value[3]);

                int index = 0;
                for (int i = 0; i < listView1.Groups.Count; i++)
                {
                    if (listView1.Groups[i].Name == d.Value[4]) index = i;
                }

                item.Group = listView1.Groups[index];

                if (d.Value[5] != "default.jpg")
                {
                    listView1.LargeImageList.Images.Add(Image.FromFile(d.Value[5]));
                    listView1.SmallImageList.Images.Add(Image.FromFile(d.Value[5]));
                    int indexPic = listView1.LargeImageList.Images.Count;
                    item.ImageIndex = indexPic - 1;
                }
                else item.ImageIndex = 0;

                MessageBox.Show(@"New encounter " + d.Value[0] + " added!");
                listView1.Items.Add(item);
            }
        }

        private void ShowBox()
        {
            PassWordForm pForm = new PassWordForm();
            pForm.Owner = this;
            pForm.OnPasswordEntered += Form_OnPasswordEntered;
            DialogResult res = pForm.ShowDialog();
        }

        private void Form_OnPasswordEntered(object sender, string word)
        {
            pswrdBuffer=word;
        }

        private void PasswordSave(string word)
        {
            FileStream destFile = File.Create("pswrdLocked.txt");
            byte[] buffer = Encoding.ASCII.GetBytes(word);
            RijndaelManaged alg = new RijndaelManaged();
            byte[] salt = Encoding.ASCII.GetBytes("1cG11Mq19w");
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes("EnCount", salt);
            alg.Key = key.GetBytes(alg.KeySize / 8);
            alg.IV = key.GetBytes(alg.BlockSize / 8);
            ICryptoTransform encriptor = alg.CreateEncryptor();
            CryptoStream encstream = new CryptoStream(destFile, encriptor, CryptoStreamMode.Write);
            encstream.Write(buffer, 0, buffer.Length);
            encstream.Close();
            destFile.Close();
        }

        private string PasswordLoad()
        {
            string result = null;
            FileStream decryptedFile = File.Create("pswrdUnLocked.txt"); 
            FileStream cryptedFile = new FileStream("pswrdLocked.txt", FileMode.Open, FileAccess.Read, FileShare.Read);

            byte[] salt = Encoding.ASCII.GetBytes("1cG11Mq19w");

            Rfc2898DeriveBytes key2 = new Rfc2898DeriveBytes("EnCount", salt);
            RijndaelManaged alg = new RijndaelManaged();
            alg.Key = key2.GetBytes(alg.KeySize / 8);
            alg.IV = key2.GetBytes(alg.BlockSize / 8);

            ICryptoTransform decriptor = alg.CreateDecryptor();
            CryptoStream decstream = new CryptoStream(cryptedFile, decriptor, CryptoStreamMode.Read);
            
            int b = decstream.ReadByte();

            while (b != -1)
            {
                decryptedFile.WriteByte((byte)b);
                b = decstream.ReadByte();
            }

            decstream.Close();
            cryptedFile.Close();
            decryptedFile.Close();

            StreamReader stream = new StreamReader("pswrdUnLocked.txt", Encoding.Default);
            result=stream.ReadLine();
            stream.Close();
            File.Delete("pswrdUnLocked.txt");

            return result;

        }
    }
}
