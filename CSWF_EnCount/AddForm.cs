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
    public delegate void AddContact(object sender, List<string> data);//, Image pic);
    public partial class AddForm : Form
    {
        public event AddContact OnContactAdded;

        List<string> data;

        string name;
        string number;
        string address;
        string note;
        string category;
        string path;

        
        public AddForm()
        {
            InitializeComponent();
            data = new List<string>();
            
            name = "_";
            number = "_";
            address = "_";
            note = "_";
            category = "_";
            path = "default.jpg";
            
            Bitmap picNew = ImageResize.ResizeImage(Image.FromFile("default.jpg"));
            pictureBox1.Image = picNew;
        }

        private void AddForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            name = textBox1.Text;
            data.Add(name);

            number = textBox2.Text;
            data.Add(number);

            address = textBox3.Text;
            data.Add(address);

            note = textBox4.Text;
            data.Add(note);

            data.Add(category);

            if (path != "default.jpg")
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
                    fullName=fullName.Remove(pos, 1);
                    pos = fullName.IndexOfAny(omit);
                }

                path = path.Substring(path.LastIndexOf('.'));
                path = path.Insert(0, fullName);
                
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fs, Encoding.Default);
                bw.Write(b);
                fs.Close();
            }

            data.Add(path);
            OnContactAdded?.Invoke(this, data);
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

        
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            category = comboBox1.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap picNew = ImageResize.ResizeImage(Image.FromFile("default.jpg"));
            pictureBox1.Image = picNew;
            path = "default.jpg";

        }
    }
}
