using System;
using System.Collections.Generic;

namespace Compilador
{
    public class Semantico
    {
        private Dictionary<string, string> tablaSimbolos;

        public Semantico()
        {
            tablaSimbolos = new Dictionary<string, string>();
        }

        public void Analizar(Lexer lexer)
        {
            tablaSimbolos.Clear();

            lexer.GetToken(true);

            while (lexer.GetCodigoToken() != 24) // <FIN>
            {
                int token = lexer.GetCodigoToken();

                // DECLARACIÓN
                if (token == 3 || token == 4) // entero | logico
                {
                    string tipo = token == 3 ? "entero" : "logico";

                    lexer.GetToken(true);

                    if (lexer.GetCodigoToken() != 25)
                        throw new Exception("Error semántico: Se esperaba identificador");

                    string nombre = lexer.GetLexema();
                    Declarar(nombre, tipo);
                }

                // ASIGNACIÓN
                else if (token == 25) // identificador
                {
                    string nombre = lexer.GetLexema();

                    lexer.GetToken(true);

                    if (lexer.GetCodigoToken() == 15) // =
                    {
                        VerificarDeclarada(nombre);

                        lexer.GetToken(true);

                        string tipoExpresion = ObtenerTipoExpresion(lexer);

                        ValidarAsignacion(nombre, tipoExpresion);
                    }
                }

                lexer.GetToken(true);
            }
        }

        // OBTENER TIPO DE EXPRESIÓN COMPLETA
        private string ObtenerTipoExpresion(Lexer lexer)
        {
            string tipo1 = ObtenerTipoOperando(lexer);

            lexer.GetToken(true);

            // Operador aritmético (+ - * /)
            if (lexer.GetCodigoToken() >= 17 && lexer.GetCodigoToken() <= 20)
            {
                int operador = lexer.GetCodigoToken();

                lexer.GetToken(true);

                string tipo2 = ObtenerTipoOperando(lexer);

                // Solo enteros pueden operar aritméticamente
                if (tipo1 != "entero" || tipo2 != "entero")
                    throw new Exception("Error semántico: Operación aritmética requiere enteros");

                return "entero";
            }

            return tipo1;
        }

        // OBTENER TIPO DE OPERANDO
        private string ObtenerTipoOperando(Lexer lexer)
        {
            int token = lexer.GetCodigoToken();

            if (token == 7 || token == 8) // verdadero | falso
                return "logico";

            if (token == 26) // número
                return "entero";

            if (token == 25) // identificador
            {
                string nombre = lexer.GetLexema();
                return ObtenerTipo(nombre);
            }

            throw new Exception("Error semántico: Operando inválido");
        }

        // DECLARAR VARIABLE
        private void Declarar(string nombre, string tipo)
        {
            if (tablaSimbolos.ContainsKey(nombre))
                throw new Exception("Error semántico: Variable ya declarada -> " + nombre);

            tablaSimbolos.Add(nombre, tipo);
        }

        // VERIFICAR DECLARACIÓN
        private void VerificarDeclarada(string nombre)
        {
            if (!tablaSimbolos.ContainsKey(nombre))
                throw new Exception("Error semántico: Variable no declarada -> " + nombre);
        }

        // OBTENER TIPO DE VARIABLE
        private string ObtenerTipo(string nombre)
        {
            VerificarDeclarada(nombre);
            return tablaSimbolos[nombre];
        }

        // VALIDAR ASIGNACIÓN
        private void ValidarAsignacion(string nombre, string tipoExpresion)
        {
            string tipoVariable = ObtenerTipo(nombre);

            if (tipoVariable != tipoExpresion)
                throw new Exception("Error semántico: Tipos incompatibles en asignación -> " + nombre);
        }
    }
}