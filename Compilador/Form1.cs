using System;
using System.Text;
using System.Windows.Forms;

namespace Compilador
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        // BOTÓN SCANNER
        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();

            string codigo = textBox1.Text;

            if (string.IsNullOrWhiteSpace(codigo))
            {
                MessageBox.Show("Ingrese un programa para analizar");
                return;
            }

            Lexer lexer = new Lexer(codigo);
            StringBuilder resultado = new StringBuilder();

            do
            {
                lexer.GetToken(true);

                resultado.AppendLine(
                    lexer.GetLexema() +
                    "  ->  " +
                    lexer.GetTipoToken() +
                    " (" +
                    lexer.GetCodigoToken() +
                    ")"
                );

            } while (lexer.GetCodigoToken() != 24); // 24 = <FIN>

            textBox2.Text = resultado.ToString();
        }
    }
}
