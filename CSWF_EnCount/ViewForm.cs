using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSWF_EnCount
{
    //public delegate void ViewContact(object sender, List<string> data);//, Image pic);
    public partial class ViewForm : Form
    {
        public ViewForm(List<string> data)
        {
            InitializeComponent();
            textBox1.Text = data[0];
            textBox2.Text = data[1];
            textBox3.Text = data[2];
            textBox4.Text = data[3];

            pictureBox1.Image = (data[5] != "_")? ImageResize.ResizeImage(Image.FromFile(data[5])):Image.FromFile("default.jpg");

            label5.Text = data[4];

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Edit form opening");
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
