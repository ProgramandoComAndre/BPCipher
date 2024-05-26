using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cifras
{
    public partial class Teclado_Angular : UserControl
    {
        public TextBox txtInput { set; get; }
        
        public Teclado_Angular()
        {
            InitializeComponent();
            
        }

        private void Teclado_Angular_Load(object sender, EventArgs e)
        {
            string alfabeto = "abcdefghijklmnopqrstuvwxyz";
            foreach(Control pb in this.Controls)
            {
                

                    foreach (char c in alfabeto)
                    {
                        if (pb.Name == c.ToString())
                        {
                            ((PictureBox)pb).Load(@"Imagens\Codigo angular\" + c + ".PNG");
                            break;
                        }
                    }
                
                

            }
        }

        private void z_Click(object sender, EventArgs e)
        {
            txtInput.Text += ((PictureBox)sender).Name;
        }

        private void btn_space_Click(object sender, EventArgs e)
        {
            txtInput.Text += " ";
        }

        private void button1_Click(object sender, EventArgs e)
        {
           if(txtInput.Text.Length != 0)
                txtInput.Text = txtInput.Text.Remove(txtInput.Text.Length - 1);
        }
    }
}
