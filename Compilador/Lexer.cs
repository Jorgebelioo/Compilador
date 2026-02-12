using System.Text.RegularExpressions;

namespace Compilador
{
    public class Lexer
    {
        private string[] tokens;
        private int indice;
        private string tipoToken = "";
        private string tokenActual = "";

        // PALABRAS RESERVADAS MODUS
        private readonly string[] palabrasReservadas =
        {
            "modulo", "importar",
            "entero", "logico",
            "repetir", "mostrar",
            "verdadero", "falso",
            "social", "antisocial", "inmovil",
            "<FIN>"
        };

        // SÍMBOLOS TERMINALES
        private readonly string[] simbolos =
        {
            "{", "}", "(", ")", ";", ".", "=", "<", "+", "-", "*", "/"
        };

        public Lexer(string codigo)
        {
            // Separar símbolos
            codigo = Regex.Replace(codigo, "([{}();.=<>+\\-*])", " $1 ");

            tokens = codigo.Trim().Split(
                new[] { ' ', '\t', '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries
            );

            indice = 0;
            tokenActual = tokens.Length > 0 ? tokens[0] : "<FIN>";
        }

        public string GetTokenActual()
        {
            return tokenActual;
        }

        public string GetTipoToken()
        {
            return tipoToken;
        }

        public string GetToken(bool avanza)
        {
            if (indice >= tokens.Length)
            {
                tokenActual = "<FIN>";
                tipoToken = "<FIN>";
                return tokenActual;
            }

            string token = tokens[indice];
            if (avanza) indice++;

            tokenActual = token;
            tipoToken = DetectarTipo(token);

            return tokenActual;
        }

        private string DetectarTipo(string token)
        {
            // Palabras reservadas
            foreach (string palabra in palabrasReservadas)
            {
                if (token == palabra)
                {
                    tipoToken = "Palabra reservada";
                    return tipoToken;
                }
            }

            // Símbolos 
            switch (token)
            {
                case "+":
                    return "Operador suma";

                case "-":
                    return "Operador resta";

                case "*":
                    return "Operador multiplicación";

                case "/":
                    return "Operador división";

                case "<":
                    return "Operador menor que (<)";

                case "=":
                    return "Operador asignación";

                case "{":
                    return "Llave apertura";

                case "}":
                    return "Llave cierre";

                case "(":
                    return "Paréntesis apertura";

                case ")":
                    return "Paréntesis cierre";

                case ";":
                    return "Fin de instrucción";

                case ".":
                    return "Separador / acceso";

                case "<FIN>":
                    return "Fin de programa";
            }

            // Número
            if (Regex.IsMatch(token, @"^\d+$"))
            {
                tipoToken = "Número";
                return tipoToken;
            }

            // Identificador
            if (Regex.IsMatch(token, @"^[a-zA-Z][a-zA-Z0-9]*$"))
            {
                tipoToken = "Identificador";
                return tipoToken;
            }

            tipoToken = "Token inválido";
            Error(token);
            return tipoToken;
        }

        private void Error(string token)
        {
           MessageBox.Show("Error léxico: token inválido -> " + token);
        }
    }
}