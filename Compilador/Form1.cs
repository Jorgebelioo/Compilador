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

            dataGridView2.Columns.Clear();
            dataGridView2.Columns.Add("Nombre", "Nombre");
            dataGridView2.Columns.Add("Operacion", "Operacion");
            dataGridView2.Columns.Add("Operandos", "Operandos");

            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
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
            label4.Text = "       . . .\r\n";
            label4.ForeColor = Color.Black;

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

                label3.Text = "Sintactico OK";
                label3.ForeColor = Color.Green;
                label3.Font = new Font(label3.Font, FontStyle.Bold);
            }
            catch (Exception ex) when (ex.Message.StartsWith("Error sintáctico"))
            {
                label3.Text = "Error sintáctico";
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

                // 1 Análisis sintáctico (si falla, no lo atrapamos)
                Parser parser = new Parser(lexer);
                parser.Programa();

                // 2 Reiniciar lexer para el semántico
                lexer.Reset();

                // 3 Análisis semántico
                Semantico sem = new Semantico();
                sem.Analizar(lexer);

                label4.Text = "Semantico OK";
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

        // Boton Codigo Intermedio
        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();

            string codigo = textBox1.Text;

            if (string.IsNullOrWhiteSpace(codigo))
            {
                MessageBox.Show("Ingrese código");
                return;
            }

            try
            {
                Lexer lexer = new Lexer(codigo);

                // Sintáctico
                Parser parser = new Parser(lexer);
                parser.Programa();

                // Reiniciar lexer
                lexer.Reset();

                // Generar código intermedio
                CodigoIntermedio gen = new CodigoIntermedio();

                var instrucciones = gen.Generar(lexer);

                foreach (var ins in instrucciones)
                {
                    dataGridView2.Rows.Add(
                        ins.Nombre,
                        ins.Operacion,
                        ins.Operandos
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
