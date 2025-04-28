using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Text;

namespace IDE_COMPILADOR.AnalizadorLexico
{
    public class LexicalAnalyzer
    {
        private List<Token> tokens = new List<Token>();
        private List<string> errores = new List<string>();

        private int linea = 1;
        private int columna = 1;
        private int i = 0;
        private string texto;

        public (List<Token> tokens, List<string> errores) Analizar(string entrada)
        {
            tokens.Clear();
            errores.Clear();
            linea = 1;
            columna = 1;
            i = 0;
            texto = entrada;

            while (i < texto.Length)
            {
                char actual = texto[i];

                if (char.IsWhiteSpace(actual))
                {
                    if (actual == '\n') { linea++; columna = 1; }
                    else { columna++; }
                    i++;
                }
                else if (char.IsLetter(actual))
                {
                    ReconocerIdentificador();
                }
                else if (char.IsDigit(actual) || actual == '+' || actual == '-')
                {
                    ReconocerNumero();
                }
                else if (actual == '/')
                {
                    ReconocerComentario();
                }
                else if ("=><".Contains(actual))
                {
                    ReconocerOperadorRelacional();
                }
                else if (actual == '&')
                {
                    ReconocerOperadorLogico();
                }
                else if ("|*()KZ1;{}".Contains(actual))
                {
                    tokens.Add(new Token("OperadorEspecial", actual.ToString(), linea, columna));
                    columna++; i++;
                }
                else
                {
                    errores.Add($"Error léxico: símbolo inesperado '{actual}' en línea {linea}, columna {columna}");
                    columna++; i++;
                }
            }

            return (tokens, errores);
        }

        private void ReconocerIdentificador()
        {
            int inicioColumna = columna;
            StringBuilder sb = new StringBuilder();
            while (i < texto.Length && (char.IsLetterOrDigit(texto[i])))
            {
                sb.Append(texto[i]);
                i++;
                columna++;
            }
            tokens.Add(new Token("Identificador", sb.ToString(), linea, inicioColumna));
        }

        private void ReconocerNumero()
        {
            int inicioColumna = columna;
            StringBuilder sb = new StringBuilder();

            if (texto[i] == '+' || texto[i] == '-')
            {
                sb.Append(texto[i]);
                i++;
                columna++;
            }

            bool puntoEncontrado = false;
            while (i < texto.Length && (char.IsDigit(texto[i]) || texto[i] == '.'))
            {
                if (texto[i] == '.')
                {
                    if (puntoEncontrado)
                    {
                        errores.Add($"Error léxico: número con múltiples puntos en línea {linea}, columna {columna}");
                        break;
                    }
                    puntoEncontrado = true;
                }

                sb.Append(texto[i]);
                i++;
                columna++;
            }

            if (puntoEncontrado)
                tokens.Add(new Token("PuntoFlotante", sb.ToString(), linea, inicioColumna));
            else
                tokens.Add(new Token("Número", sb.ToString(), linea, inicioColumna));
        }

        private void ReconocerComentario()
        {
            int inicioColumna = columna;

            if (i + 1 < texto.Length)
            {
                if (texto[i + 1] == '*')
                {
                    i += 2;
                    columna += 2;
                    while (i + 1 < texto.Length && !(texto[i] == '*' && texto[i + 1] == '/'))
                    {
                        if (texto[i] == '\n') { linea++; columna = 1; }
                        else { columna++; }
                        i++;
                    }
                    if (i + 1 < texto.Length)
                    {
                        i += 2;
                        columna += 2;
                    }
                    tokens.Add(new Token("ComentarioExtenso", "Comentario Extenso", linea, inicioColumna));
                }
                else if (texto[i + 1] == '/')
                {
                    i += 2;
                    columna += 2;
                    while (i < texto.Length && texto[i] != '\n')
                    {
                        i++;
                        columna++;
                    }
                    tokens.Add(new Token("ComentarioInline", "Comentario In-line", linea, inicioColumna));
                }
                else
                {
                    tokens.Add(new Token("OperadorDivision", "/", linea, columna));
                    i++;
                    columna++;
                }
            }
            else
            {
                tokens.Add(new Token("OperadorDivision", "/", linea, columna));
                i++;
                columna++;
            }
        }

        private void ReconocerOperadorRelacional()
        {
            int inicioColumna = columna;
            char actual = texto[i];
            i++;
            columna++;

            if (i < texto.Length && texto[i] == '=')
            {
                tokens.Add(new Token("OperadorRelacional", actual + "=", linea, inicioColumna));
                i++;
                columna++;
            }
            else
            {
                tokens.Add(new Token("OperadorRelacional", actual.ToString(), linea, inicioColumna));
            }
        }

        private void ReconocerOperadorLogico()
        {
            int inicioColumna = columna;
            i++;
            columna++;
            if (i < texto.Length && texto[i] == '&')
            {
                tokens.Add(new Token("OperadorLogico", "&&", linea, inicioColumna));
                i++;
                columna++;
            }
            else
            {
                errores.Add($"Error léxico: operador lógico incompleto en línea {linea}, columna {inicioColumna}");
            }
        }
    }
}
