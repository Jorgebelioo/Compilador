using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Compilador
{
    public class Lexer
    {
        private string[] tokens;
        private int indice;
        private string tokenActual = "";
        private string tipoToken = "";
        private int codigoToken = 0;

        public Lexer(string codigo)
        {
            codigo = Regex.Replace(codigo, "([{}();.=<>+\\-*])", " $1 ");

            tokens = codigo.Trim().Split(
                new[] { ' ', '\t', '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries
            );

            indice = 0;
        }

        public string GetLexema()
        {
            return tokenActual;
        }

        public string GetTipoToken()
        {
            return tipoToken;
        }

        public int GetCodigoToken()
        {
            return codigoToken;
        }

        public void GetToken(bool avanza)
        {
            if (indice >= tokens.Length)
            {
                tokenActual = "<FIN>";
                tipoToken = "<FIN>";
                codigoToken = 24;
                return;
            }

            string token = tokens[indice];
            if (avanza) indice++;

            tokenActual = token;

            DetectarToken(token);
        }

        private void DetectarToken(string token)
        {
            switch (token)
            {
                // PALABRAS RESERVADAS
                case "modulo": tipoToken = "Palabra reservada"; codigoToken = 1; return;
                case "importar": tipoToken = "Palabra reservada"; codigoToken = 2; return;
                case "entero": tipoToken = "Palabra reservada"; codigoToken = 3; return;
                case "logico": tipoToken = "Palabra reservada"; codigoToken = 4; return;
                case "repetir": tipoToken = "Palabra reservada"; codigoToken = 5; return;
                case "mostrar": tipoToken = "Palabra reservada"; codigoToken = 6; return;
                case "verdadero": tipoToken = "Palabra reservada"; codigoToken = 7; return;
                case "falso": tipoToken = "Palabra reservada"; codigoToken = 8; return;
                case "social": tipoToken = "Palabra reservada"; codigoToken = 21; return;
                case "antisocial": tipoToken = "Palabra reservada"; codigoToken = 22; return;
                case "inmovil": tipoToken = "Palabra reservada"; codigoToken = 23; return;
                case "<FIN>": tipoToken = "<FIN>"; codigoToken = 24; return;

                // SÍMBOLOS
                case "{": tipoToken = "Llave apertura"; codigoToken = 9; return;
                case "}": tipoToken = "Llave cierre"; codigoToken = 10; return;
                case "(": tipoToken = "Paréntesis apertura"; codigoToken = 11; return;
                case ")": tipoToken = "Paréntesis cierre"; codigoToken = 12; return;
                case ";": tipoToken = "Fin de instrucción"; codigoToken = 13; return;
                case ".": tipoToken = "Separador / acceso"; codigoToken = 14; return;
                case "=": tipoToken = "Operador asignación"; codigoToken = 15; return;
                case "<": tipoToken = "Operador menor que"; codigoToken = 16; return;
                case "+": tipoToken = "Operador suma"; codigoToken = 17; return;
                case "-": tipoToken = "Operador resta"; codigoToken = 18; return;
                case "*": tipoToken = "Operador multiplicación"; codigoToken = 19; return;
                case "/": tipoToken = "Operador división"; codigoToken = 20; return;
            }

            // Número
            if (Regex.IsMatch(token, @"^\d+$"))
            {
                tipoToken = "Número";
                codigoToken = 26;
                return;
            }

            // Identificador
            if (Regex.IsMatch(token, @"^[a-zA-Z][a-zA-Z0-9]*$"))
            {
                tipoToken = "Identificador";
                codigoToken = 25;
                return;
            }

            tipoToken = "Token inválido";
            codigoToken = -1;
            Error(token);
        }

        private void Error(string token)
        {
            MessageBox.Show("Error léxico: token inválido -> " + token);
        }
    }
}
