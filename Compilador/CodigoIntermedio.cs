using System;
using System.Collections.Generic;

namespace Compilador
{
    public class CodigoIntermedio
    {
        public class Instruccion
        {
            public string Nombre { get; set; }
            public string Operacion { get; set; }
            public string Operandos { get; set; }
        }

        public List<Instruccion> Generar(Lexer lexer)
        {
            List<Instruccion> codigo = new List<Instruccion>();

            lexer.GetToken(true);

            while (lexer.GetCodigoToken() != 24) // FIN
            {
                int token = lexer.GetCodigoToken();

                // ASIGNACION
                if (token == 25) // identificador
                {
                    string variable = lexer.GetLexema();

                    lexer.GetToken(true);

                    if (lexer.GetCodigoToken() == 15) // =
                    {
                        lexer.GetToken(true);

                        string op1 = lexer.GetLexema();

                        lexer.GetToken(true);

                        // OPERACION ARITMETICA
                        if (lexer.GetCodigoToken() >= 17 && lexer.GetCodigoToken() <= 20)
                        {
                            string operador = lexer.GetLexema();

                            lexer.GetToken(true);

                            string op2 = lexer.GetLexema();

                            string operacion = "";

                            switch (operador)
                            {
                                case "+": operacion = "ADD"; break;
                                case "-": operacion = "SUB"; break;
                                case "*": operacion = "MUL"; break;
                                case "/": operacion = "DIV"; break;
                            }

                            codigo.Add(new Instruccion
                            {
                                Nombre = variable,
                                Operacion = operacion,
                                Operandos = op1 + "," + op2
                            });
                        }
                        else
                        {
                            codigo.Add(new Instruccion
                            {
                                Nombre = variable,
                                Operacion = "MOV",
                                Operandos = op1
                            });
                        }
                    }
                }

                // MOSTRAR
                if (token == 6)
                {
                    lexer.GetToken(true); // (
                    lexer.GetToken(true);

                    string valor = lexer.GetLexema();

                    codigo.Add(new Instruccion
                    {
                        Nombre = "PRINT",
                        Operacion = "CALL",
                        Operandos = valor
                    });
                }

                lexer.GetToken(true);
            }

            return codigo;
        }
    }
}