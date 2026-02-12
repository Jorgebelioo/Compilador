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
            label3.Text = "       . . .\r\n";
            label3.ForeColor = Color.Black;

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
                    lexer.GetTipoToken()
                );

            } while (lexer.GetCodigoToken() != 24); // 24 = <FIN>

            textBox2.Text = resultado.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label3.Text = "";

            string codigo = textBox1.Text;

            if (string.IsNullOrWhiteSpace(codigo))
            {
                MessageBox.Show("Ingrese un programa para analizar");
                return;
            }

            try
            {
                Lexer lexer = new Lexer(codigo);
                Parser parser = new Parser(lexer);

                parser.Programa();

                label3.Text = "Programa OK";
                label3.ForeColor = Color.Green;
                label3.Font = new Font(label3.Font, FontStyle.Bold);
            }
            catch
            {
                label3.Text = "Syntax error";
                label3.ForeColor = Color.Red;
                label3.Font = new Font(label3.Font, FontStyle.Bold);
            }
        }

    }
}
