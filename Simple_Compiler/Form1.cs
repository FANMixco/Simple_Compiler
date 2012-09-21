using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Simple_Compiler
{
    public partial class Form1 : Form
    {
        TextReader input;
        string name;
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new
                StreamReader(openFileDialog1.FileName);
                MessageBox.Show(sr.ReadToEnd());
                sr.Close();
                txtLocation.Text = openFileDialog1.FileName;
                try
                {
                    input = File.OpenText(txtLocation.Text);
                    name = Path.GetFileNameWithoutExtension(txtLocation.Text);
                    if (!txtLocation.Text.Equals(string.Empty))
                    {
                        button2.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en lectura del archivo","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Scanner scanner = null;
                using (input)
                {
                    scanner = new Scanner(input);
                }
                Parser parser = new Parser(scanner.Tokens);
                CodeGen codeGen = new CodeGen(parser.Resultado, Path.GetFileNameWithoutExtension(txtLocation.Text) + ".exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            button3.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Path.Combine(Application.StartupPath, name + ".exe")); 
        }
    }
}
