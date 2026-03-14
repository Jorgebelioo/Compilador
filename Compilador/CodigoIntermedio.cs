using System;
using System.Collections.Generic;

namespace Compilador
{
    public class CodigoIntermedio
    {
        private List<Linea> data = new List<Linea>();
        private List<Linea> code = new List<Linea>();

        private string nombrePrograma = "programa";
        private int contadorEtiquetas = 1;

        public List<Linea> Generar(Lexer lexer)
        {
            data.Clear();
            code.Clear();

            lexer.GetToken(true);

            // ignorar modificadores
            while (lexer.GetCodigoToken() == 21 ||
                   lexer.GetCodigoToken() == 22 ||
                   lexer.GetCodigoToken() == 23)
                lexer.GetToken(true);

            // nombre del modulo
            if (lexer.GetCodigoToken() == 1)
            {
                lexer.GetToken(true);
                nombrePrograma = lexer.GetLexema();
            }

            lexer.GetToken(true); // {

            lexer.GetToken(true);

            // =====================
            // DECLARACIONES
            // =====================

            while (lexer.GetCodigoToken() == 3 || lexer.GetCodigoToken() == 4)
            {
                string tipo = lexer.GetCodigoToken() == 3 ? "DW" : "DB";

                lexer.GetToken(true);

                string nombre = lexer.GetLexema();

                data.Add(new Linea
                {
                    Nombre = nombre,
                    Operacion = tipo,
                    Operandos = "?"
                });

                lexer.GetToken(true); // ;
                lexer.GetToken(true);
            }

            // =====================
            // INSTRUCCIONES
            // =====================

            while (lexer.GetCodigoToken() != 10 && lexer.GetCodigoToken() != 24)
            {
                TraducirInstruccion(lexer);
                lexer.GetToken(true);
            }

            // =====================
            // GENERAR PROGRAMA
            // =====================

            List<Linea> programa = new List<Linea>();

            programa.Add(new Linea { Operacion = "TITLE", Operandos = nombrePrograma });
            programa.Add(new Linea { Operacion = ".MODEL", Operandos = "SMALL" });
            programa.Add(new Linea { Operacion = ".STACK", Operandos = "100h" });

            programa.Add(new Linea { Operacion = ".DATA" });

            programa.AddRange(data);

            programa.Add(new Linea { Operacion = ".CODE" });

            programa.Add(new Linea { Nombre = "MAIN", Operacion = "PROC" });

            programa.Add(new Linea { Operacion = "MOV", Operandos = "AX,@DATA" });
            programa.Add(new Linea { Operacion = "MOV", Operandos = "DS,AX" });

            programa.AddRange(code);

            programa.Add(new Linea { Operacion = "MOV", Operandos = "AH,4Ch" });
            programa.Add(new Linea { Operacion = "INT", Operandos = "21h" });

            programa.Add(new Linea { Nombre = "MAIN", Operacion = "ENDP" });
            programa.Add(new Linea { Operacion = "END", Operandos = "MAIN" });

            return programa;
        }

        // =====================
        // TRADUCCION
        // =====================

        private void TraducirInstruccion(Lexer lexer)
        {
            int token = lexer.GetCodigoToken();

            // ASIGNACION
            if (token == 25)
            {
                string var = lexer.GetLexema();

                lexer.GetToken(true); // =

                if (lexer.GetCodigoToken() == 15)
                {
                    lexer.GetToken(true);

                    string op1 = ConvertirValor(lexer.GetLexema());

                    lexer.GetToken(true);

                    if (lexer.GetCodigoToken() >= 17 && lexer.GetCodigoToken() <= 20)
                    {
                        string operador = lexer.GetLexema();

                        lexer.GetToken(true);

                        string op2 = ConvertirValor(lexer.GetLexema());

                        code.Add(new Linea { Operacion = "MOV", Operandos = "AX," + op1 });

                        string op = "";

                        if (operador == "+") op = "ADD";
                        if (operador == "-") op = "SUB";
                        if (operador == "*") op = "IMUL";
                        if (operador == "/") op = "DIV";

                        code.Add(new Linea { Operacion = op, Operandos = "AX," + op2 });

                        code.Add(new Linea { Operacion = "MOV", Operandos = var + ",AX" });
                    }
                    else
                    {
                        code.Add(new Linea { Operacion = "MOV", Operandos = var + "," + op1 });
                    }
                }
            }

            // Repetir
            if (token == 5)
            {
                string L1 = NuevaEtiqueta();
                string L2 = NuevaEtiqueta();

                code.Add(new Linea { Nombre = L1 });

                lexer.GetToken(true); // (
                lexer.GetToken(true);

                string op1 = lexer.GetLexema();

                lexer.GetToken(true);

                string operador = lexer.GetLexema();

                lexer.GetToken(true);

                string op2 = lexer.GetLexema();

                code.Add(new Linea { Operacion = "MOV", Operandos = "AX," + op1 });
                code.Add(new Linea { Operacion = "CMP", Operandos = "AX," + op2 });

                string salto = "JGE";

                if (operador == "<") salto = "JGE";
                if (operador == ">") salto = "JLE";
                if (operador == "==") salto = "JNE";

                code.Add(new Linea { Operacion = salto, Operandos = L2 });

                lexer.GetToken(true); // )

                lexer.GetToken(true); // {

                while (lexer.GetCodigoToken() != 10)
                {
                    TraducirInstruccion(lexer);
                    lexer.GetToken(true);
                }

                code.Add(new Linea { Operacion = "JMP", Operandos = L1 });

                code.Add(new Linea { Nombre = L2 });
            }
        }

        // =====================
        // UTILIDADES
        // =====================

        private string ConvertirValor(string valor)
        {
            return valor;
        }

        private string NuevaEtiqueta()
        {
            return "L" + contadorEtiquetas++;
        }
    }
}