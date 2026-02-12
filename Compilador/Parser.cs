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

        private void Match(int tokenEsperado)
        {
            if (tokenActual == tokenEsperado)
            {
                Avanzar();
            }
            else
            {
                Error("Se esperaba token: " + tokenEsperado);
            }
        }

        private void Error(string mensaje)
        {
            throw new Exception("Error sintáctico: " + mensaje +
                                " | Token actual: " + lexer.GetLexema());
        }

        // ==============================
        // PROGRAMA
        // ==============================

        public void Programa()
        {
            // (Importacion)*
            while (tokenActual == 2) // importar
            {
                Importacion();
            }

            // Modificador+
            if (tokenActual == 21 || tokenActual == 22 || tokenActual == 23)
            {
                while (tokenActual == 21 || tokenActual == 22 || tokenActual == 23)
                {
                    Modificador();
                }
            }
            else
            {
                Error("Se esperaba al menos un modificador");
            }

            Match(1);   // modulo
            Match(25);  // Identificador
            Match(9);   // {

            // (Declaracion)*
            while (tokenActual == 3 || tokenActual == 4)
            {
                Declaracion();
            }

            // Instruccion*
            while (EsInicioInstruccion())
            {
                Instruccion();
            }

            Match(10);  // }
            Match(24);  // <FIN>
        }

        // ==============================
        // REGLAS
        // ==============================

        private void Modificador()
        {
            if (tokenActual == 21 || tokenActual == 22 || tokenActual == 23)
                Avanzar();
            else
                Error("Modificador inválido");
        }

        private void Importacion()
        {
            Match(2);   // importar
            Match(25);  // Identificador

            while (tokenActual == 14) // .
            {
                Match(14);
                Match(25);
            }

            Match(13);  // ;
        }

        private void Declaracion()
        {
            if (tokenActual == 3 || tokenActual == 4) // entero | logico
                Avanzar();
            else
                Error("Tipo inválido");

            Match(25);  // Identificador
            Match(13);  // ;
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

        private bool EsInicioInstruccion()
        {
            return tokenActual == 9 ||   // {
                   tokenActual == 5 ||   // repetir
                   tokenActual == 6 ||   // mostrar
                   tokenActual == 25;    // identificador
        }

        private void Bloque()
        {
            Match(9); // {

            while (EsInicioInstruccion())
            {
                Instruccion();
            }

            Match(10); // }
        }

        // ACTUALIZADO: ahora exige Bloque
        private void Ciclo()
        {
            Match(5);   // repetir
            Match(11);  // (

            Expresion();

            Match(12);  // )

            Bloque();   // ← CAMBIO IMPORTANTE
        }

        private void Imprimir()
        {
            Match(6);   // mostrar
            Match(11);  // (

            Expresion();

            Match(12);  // )
            Match(13);  // ;
        }

        private void Asignacion()
        {
            Match(25);  // Identificador
            Match(15);  // =

            Expresion();

            Match(13);  // ;
        }

        private void Expresion()
        {
            // verdadero | falso
            if (tokenActual == 7 || tokenActual == 8)
            {
                Avanzar();
                return;
            }

            // Identificador | Numero
            if (tokenActual == 25 || tokenActual == 26)
            {
                Avanzar();

                // Operador
                if (tokenActual == 16 || tokenActual == 17 ||
                    tokenActual == 18 || tokenActual == 19 ||
                    tokenActual == 20)  // ← ahora incluye división
                {
                    Avanzar();

                    if (tokenActual == 25 || tokenActual == 26)
                        Avanzar();
                    else
                        Error("Se esperaba identificador o número");
                }

                return;
            }

            Error("Expresión inválida");
        }
    }
}
