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
    public delegate void EnterPassword(object sender, string word);
    public partial class PassWordForm : Form
    {
        public event EnterPassword OnPasswordEntered;
        public PassWordForm()
        {
            InitializeComponent();
        }

        private void PassWordForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OnPasswordEntered?.Invoke(this, textBox1.Text);
            this.Close();
        }
    }
}
