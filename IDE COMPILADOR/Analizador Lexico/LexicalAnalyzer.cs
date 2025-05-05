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

        private readonly HashSet<string> palabrasReservadas = new()
    {
        "if", "else", "end", "do", "while", "switch", "case",
        "int", "float", "main", "cin", "cout"
    };

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
                else if ("=><!".Contains(actual))
                {
                    ReconocerOperadorRelacionalYLogico();
                }
                else if ("&|".Contains(actual))
                {
                    ReconocerOperadorLogicoComplejo();
                }
                else if ("+-*%^".Contains(actual))
                {
                    ReconocerOperadorAritmetico();
                }
                else if ("(){};,.".Contains(actual))
                {
                    tokens.Add(new Token("Simbolo", actual.ToString(), linea, columna));
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

            string valor = sb.ToString();
            string tipo = palabrasReservadas.Contains(valor) ? "PalabraReservada" : "Identificador";
            tokens.Add(new Token(tipo, valor, linea, inicioColumna));
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
                tokens.Add(new Token("Numero", sb.ToString(), linea, inicioColumna));
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
                    tokens.Add(new Token("OperadorAritmetico", "/", linea, columna));
                    i++;
                    columna++;
                }
            }
            else
            {
                tokens.Add(new Token("OperadorAritmetico", "/", linea, columna));
                i++;
                columna++;
            }
        }

        private void ReconocerOperadorRelacionalYLogico()
        {
            int inicioColumna = columna;
            char actual = texto[i];
            i++;
            columna++;

            if (i < texto.Length && texto[i] == '=')
            {
                string combinado = actual + "=";
                string tipo = (actual == '!') ? "OperadorLogico" : "OperadorRelacional";
                tokens.Add(new Token(tipo, combinado, linea, inicioColumna));
                i++;
                columna++;
            }
            else
            {
                string tipo = (actual == '!') ? "OperadorLogico" : (actual == '=' ? "Asignacion" : "OperadorRelacional");
                tokens.Add(new Token(tipo, actual.ToString(), linea, inicioColumna));
            }
        }

        private void ReconocerOperadorLogicoComplejo()
        {
            int inicioColumna = columna;
            char actual = texto[i];
            i++;
            columna++;
            if (i < texto.Length && texto[i] == actual)
            {
                tokens.Add(new Token("OperadorLogico", new string(actual, 2), linea, inicioColumna));
                i++;
                columna++;
            }
            else
            {
                errores.Add($"Error léxico: operador lógico incompleto en línea {linea}, columna {inicioColumna}");
            }
        }

        private void ReconocerOperadorAritmetico()
        {
            int inicioColumna = columna;
            char actual = texto[i];
            i++;
            columna++;
            if ((actual == '+' || actual == '-') && i < texto.Length && texto[i] == actual)
            {
                tokens.Add(new Token("OperadorAritmetico", new string(actual, 2), linea, inicioColumna));
                i++;
                columna++;
            }
            else
            {
                tokens.Add(new Token("OperadorAritmetico", actual.ToString(), linea, inicioColumna));
            }
        }
    }
}