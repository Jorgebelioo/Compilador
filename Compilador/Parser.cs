using System;

namespace Compilador
{
    public class Parser
    {
        private Lexer lexer;
        private int tokenActual;

        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
            Avanzar();
        }

        private void Avanzar()
        {
            lexer.GetToken(true);
            tokenActual = lexer.GetCodigoToken();
        }

        private void Match(int esperado)
        {
            if (tokenActual == esperado)
                Avanzar();
            else
                Error("Se esperaba token: " + esperado);
        }

        private void Error(string mensaje)
        {
            throw new Exception("Error sintáctico: " + mensaje +
                                " | Token actual: " + lexer.GetLexema());
        }

        public void Programa()
        {
            while (tokenActual == 2)
                Importacion();

            if (tokenActual == 21 || tokenActual == 22 || tokenActual == 23)
            {
                while (tokenActual == 21 || tokenActual == 22 || tokenActual == 23)
                    Avanzar();
            }
            else
                Error("Se esperaba al menos un modificador");

            Match(1);
            Match(25);
            Match(9);

            while (tokenActual == 3 || tokenActual == 4)
                Declaracion();

            while (EsInicioInstruccion())
                Instruccion();

            Match(10);
            Match(24);
        }

        private void Importacion()
        {
            Match(2);
            Match(25);

            while (tokenActual == 14)
            {
                Match(14);
                Match(25);
            }

            Match(13);
        }

        private void Declaracion()
        {
            Avanzar(); // tipo
            Match(25);
            Match(13);
        }

        private bool EsInicioInstruccion()
        {
            return tokenActual == 9 ||
                   tokenActual == 5 ||
                   tokenActual == 6 ||
                   tokenActual == 25;
        }

        private void Instruccion()
        {
            if (tokenActual == 9)
                Bloque();
            else if (tokenActual == 5)
                Ciclo();
            else if (tokenActual == 6)
                Imprimir();
            else if (tokenActual == 25)
                Asignacion();
            else
                Error("Instrucción inválida");
        }

        private void Bloque()
        {
            Match(9);
            while (EsInicioInstruccion())
                Instruccion();
            Match(10);
        }

        private void Ciclo()
        {
            Match(5);
            Match(11);
            Expresion();
            Match(12);
            Bloque();
        }

        private void Imprimir()
        {
            Match(6);
            Match(11);
            Expresion();
            Match(12);
            Match(13);
        }

        private void Asignacion()
        {
            Match(25);
            Match(15);
            Expresion();
            Match(13);
        }

        private void Expresion()
        {
            if (tokenActual == 7 || tokenActual == 8 ||
                tokenActual == 25 || tokenActual == 26)
            {
                Avanzar();

                if (tokenActual >= 16 && tokenActual <= 20)
                {
                    Avanzar();

                    if (tokenActual == 25 || tokenActual == 26)
                        Avanzar();
                    else
                        Error("Se esperaba identificador o número");
                }
            }
            else
            {
                Error("Expresión inválida");
            }
        }
    }
}