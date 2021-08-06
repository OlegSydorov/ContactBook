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

namespace CSWF_EnCount
{
    public delegate void EditContact(object sender, List<string> data);
   public partial class EditForm : Form
   {
        public event AddContact OnContactEdited;
        List<string> dataTemp;

        string name;
        string number;
        string address;
        string note;
        string category;
        string path;
        string pathTemp;
        public EditForm(List<string> data)
        {
            InitializeComponent();
            dataTemp = new List<string>();
            textBox1.Text = data[0];
            textBox2.Text = data[1];
            textBox3.Text = data[2];
            textBox4.Text = data[3];

            pictureBox1.Image = (data[5] != "default.jpg") ? ImageResize.ResizeImage(Image.FromFile(data[5])) : Image.FromFile("default.jpg");

            comboBox1.Text = data[4];
            path = data[5];
            pathTemp = data[5];
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap picNew = ImageResize.ResizeImage(Image.FromFile("default.jpg"));
            pictureBox1.Image = picNew;
            path = "default.jpg";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Opening picture";
            openFileDialog1.Filter = "Image Files(*.BMP; *.JPG; *.GIF)| *.BMP; *.JPG; *.GIF | All files(*.*) | *.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog1.FileName;
                pictureBox1.Image = Image.FromFile(path);
                Bitmap picNew = ImageResize.ResizeImage(Image.FromFile(path));
                pictureBox1.Image = picNew;
            }


            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            name = textBox1.Text;
            number = textBox2.Text;
            address = textBox3.Text;
            note = textBox4.Text;
            category = comboBox1.Text;

            dataTemp.Add(name);
            dataTemp.Add(number);
            dataTemp.Add(address);
            dataTemp.Add(note);
            dataTemp.Add(category);

            if (path != pathTemp)
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs, Encoding.Default);
                byte[] b = new byte[fs.Length];
                b = br.ReadBytes((int)fs.Length);
                fs.Close();

                string fullName = textBox1.Text;
                char[] omit = { ' ', ',', '.' };
                int pos = fullName.IndexOfAny(omit);
                while (pos != -1)
                {
                    fullName = fullName.Remove(pos, 1);
                    pos = fullName.IndexOfAny(omit);
                }

                path = path.Substring(path.LastIndexOf('.'));
                path = path.Insert(0, fullName);

                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fs, Encoding.Default);
                bw.Write(b);
                fs.Close();
            }

           
            dataTemp.Add(path);
            OnContactEdited?.Invoke(this, dataTemp);
            this.Close();
        }
    }
}
