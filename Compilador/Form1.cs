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
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Lexema", "Lexema");
            dataGridView1.Columns.Add("Tipo", "Tipo Token");

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
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

        //Boton Lexer
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            label3.Text = "       . . .\r\n";
            label3.ForeColor = Color.Black;

            label4.Text = "       . . .\r\n";
            label4.ForeColor = Color.Black;

            string codigo = textBox1.Text;

            if (string.IsNullOrWhiteSpace(codigo))
            {
                MessageBox.Show("Ingrese un programa para analizar");
                return;
            }

            Lexer lexer = new Lexer(codigo);

            do
            {
                lexer.GetToken(true);

                dataGridView1.Rows.Add(
                    lexer.GetLexema(),
                    lexer.GetTipoToken()
                );

            } while (lexer.GetCodigoToken() != 24); // 24 = <FIN>
        }

        //Boton Parser
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
            catch (Exception ex) when (ex.Message.StartsWith("Error sintáctico"))
            {
                label3.Text = "Syntax error";
                label3.ForeColor = Color.Red;
                label3.Font = new Font(label3.Font, FontStyle.Bold);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //Boton Semantico
        private void button3_Click(object sender, EventArgs e)
        {
            label4.Text = "";

            string codigo = textBox1.Text;

            if (string.IsNullOrWhiteSpace(codigo))
            {
                MessageBox.Show("Ingrese un programa para analizar");
                return;
            }

            try
            {
                // Crear lexer
                Lexer lexer = new Lexer(codigo);

                // 1?? Análisis sintáctico (si falla, no lo atrapamos)
                Parser parser = new Parser(lexer);
                parser.Programa();

                // 2?? Reiniciar lexer para el semántico
                lexer.Reset();

                // 3?? Análisis semántico
                Semantico sem = new Semantico();
                sem.Analizar(lexer);

                label4.Text = "Código OK";
                label4.ForeColor = Color.Green;
                label4.Font = new Font(label4.Font, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Error semántico"))
                {
                    label4.Text = "Error semántico";
                }
                else if (ex.Message.StartsWith("Error sintáctico"))
                {
                    
                }
                label4.Text = "Error semántico";
                label4.ForeColor = Color.Red;
                label4.Font = new Font(label4.Font, FontStyle.Bold);
            }
        }
    }
}
