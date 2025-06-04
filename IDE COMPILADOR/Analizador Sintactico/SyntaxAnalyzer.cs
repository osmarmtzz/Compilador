using System;
using System.Collections.Generic;
using IDE_COMPILADOR.AnalizadorLexico;
using IDE_COMPILADOR.AnalizadorSintactico.AST;

namespace IDE_COMPILADOR.AnalizadorSintactico
{
    public class SyntaxAnalyzer
    {
        private readonly List<Token> _tokens;
        private int _position;
        public List<string> Errors { get; } = new List<string>();

        public SyntaxAnalyzer(List<Token> tokens)
        {
            _tokens = tokens ?? new List<Token>();
            _position = 0;
        }

        /// <summary>
        /// Inicia el análisis sintáctico. Devuelve el nodo raíz ProgramNode si no hay errores
        /// fatales de estructura; de lo contrario, devuelve null y lista los errores en Errors.
        /// </summary>
        public ProgramNode Parse()
        {
            var program = ParseProgram();

            if (!IsAtEnd())
            {
                var t = CurrentToken();
                Error($"Contenido inesperado después de '}}': '{t.Valor}'", t.Linea, t.Columna);
            }

            return program;
        }

        #region Métodos Principales de Gramática

        // programa → main { lista_declaracion }
        private ProgramNode ParseProgram()
        {
            // 1) "main"
            if (!MatchKeyword("main"))
            {
                var t = CurrentToken();
                Error($"Se esperaba la palabra reservada 'main' pero se encontró '{t.Valor}'", t.Linea, t.Columna);
                if (!Synchronize("{")) return null;
            }

            // 2) "{"
            if (!MatchSymbol("{"))
            {
                var t = CurrentToken();
                Error($"Se esperaba '{{' después de 'main' pero se encontró '{t.Valor}'", t.Linea, t.Columna);
                if (!Synchronize("{")) return null;
            }

            // 3) lista_declaracion hasta '}'
            var declaraciones = new List<DeclarationNode>();
            while (!CheckSymbol("}") && !IsAtEnd())
            {
                var decl = ParseDeclaration();
                if (decl != null)
                    declaraciones.Add(decl);
                else
                    Advance();
            }

            // 4) "}"
            if (!MatchSymbol("}"))
            {
                var t = CurrentToken();
                Error($"Se esperaba '}}' al finalizar programa pero se encontró '{t.Valor}'", t.Linea, t.Columna);
            }

            return new ProgramNode(declaraciones);
        }

        // lista_declaracion → (declaracion)*
        private DeclarationNode ParseDeclaration()
        {
            if (CheckKeyword("int") || CheckKeyword("float") || CheckKeyword("bool"))
            {
                return ParseVariableDeclaration();
            }
            else if (IsStartOfStatement(CurrentToken()))
            {
                var stmts = ParseStatementList();
                return stmts.Count > 0 ? new StatementListNode(stmts) : null;
            }
            else
            {
                var t = CurrentToken();
                Error($"Declaración inesperada: '{t.Valor}'", t.Linea, t.Columna);
                return null;
            }
        }

        // declaracion_variable → tipo identificador ;
        private VariableDeclarationNode ParseVariableDeclaration()
        {
            string tipo = ParseType();
            if (tipo == null) return null;

            var ids = ParseIdentifierList();
            if (ids.Count == 0)
            {
                var t = CurrentToken();
                Error($"Se esperaba al menos un identificador para la declaración de tipo '{tipo}'", t.Linea, t.Columna);
            }

            if (!MatchSymbol(";"))
            {
                var t = CurrentToken();
                Error($"Se esperaba ';' al finalizar declaración de variable pero se encontró '{t.Valor}'", t.Linea, t.Columna);
                Synchronize(";");
            }

            return new VariableDeclarationNode(tipo, ids);
        }

        // identificador → id ( , id )*
        private List<string> ParseIdentifierList()
        {
            var identifiers = new List<string>();
            if (CheckType("Identificador"))
            {
                identifiers.Add(CurrentToken().Valor);
                Advance();
            }
            else
            {
                var t = CurrentToken();
                Error($"Se esperaba un identificador, pero se encontró '{t.Valor}'", t.Linea, t.Columna);
                return identifiers;
            }

            while (MatchSymbol(","))
            {
                if (CheckType("Identificador"))
                {
                    identifiers.Add(CurrentToken().Valor);
                    Advance();
                }
                else
                {
                    var t = CurrentToken();
                    Error($"Se esperaba un identificador después de ',' pero se encontró '{t.Valor}'", t.Linea, t.Columna);
                    break;
                }
            }

            return identifiers;
        }

        // tipo → int | float | bool
        private string ParseType()
        {
            if (MatchKeyword("int")) return "int";
            if (MatchKeyword("float")) return "float";
            if (MatchKeyword("bool")) return "bool";

            var t = CurrentToken();
            Error($"Se esperaba un tipo de dato ('int', 'float' o 'bool') pero se encontró '{t.Valor}'", t.Linea, t.Columna);
            return null;
        }

        // lista_sentencias → (sentencia)*
        private List<StatementNode> ParseStatementList()
        {
            var statements = new List<StatementNode>();
            while (IsStartOfStatement(CurrentToken()) && !CheckSymbol("}") && !IsAtEnd())
            {
                var stmt = ParseStatement();
                if (stmt != null)
                    statements.Add(stmt);
                else
                    Advance();
            }
            return statements;
        }

        // sentencia → seleccion | iteracion | repeticion | sent_in | sent_out | asignacion
        private StatementNode ParseStatement()
        {
            if (MatchKeyword("if")) return ParseSelection();
            if (MatchKeyword("while")) return ParseIteration();
            if (MatchKeyword("do")) return ParseRepetition();
            if (MatchKeyword("cin")) return ParseSentIn();
            if (MatchKeyword("cout")) return ParseSentOut();
            if (CheckType("Identificador") && PeekNextIs("=")) return ParseAssignment();

            var t = CurrentToken();
            Error($"Sentencia desconocida o mal formateada comenzando en '{t.Valor}'", t.Linea, t.Columna);
            return null;
        }

        // asignacion → id = expresion ;
        private AssignmentNode ParseAssignment()
        {
            string id = CurrentToken().Valor;
            Advance();

            if (MatchOperator("="))
            {
                if (MatchOperator("="))
                {
                    Retreat();
                    var t = CurrentToken();
                    Error($"Se esperaba el operador de asignación '=' pero se encontró '=='", t.Linea, t.Columna);
                    Advance();
                    return null;
                }
            }
            else
            {
                var t2 = CurrentToken();
                Error($"Se esperaba '=' en la asignación pero se encontró '{t2.Valor}'", t2.Linea, t2.Columna);
                return null;
            }

            var expr = ParseExpression() ?? new LiteralNode("0");

            if (!MatchSymbol(";"))
            {
                var t3 = CurrentToken();
                Error($"Se esperaba ';' después de la asignación pero se encontró '{t3.Valor}'", t3.Linea, t3.Columna);
                Synchronize(";");
            }

            return new AssignmentNode(id, expr);
        }

        // seleccion → if expresion then lista_sentencias [ else lista_sentencias ] end
        private IfNode ParseSelection()
        {
            var cond = ParseExpression();
            if (cond == null)
            {
                var t = CurrentToken();
                Error($"Se esperaba expresión en 'if' pero se encontró '{t.Valor}'", t.Linea, t.Columna);
            }

            if (!MatchKeyword("then"))
            {
                var t2 = CurrentToken();
                Error($"Se esperaba 'then' después de la condición en 'if' pero se encontró '{t2.Valor}'", t2.Linea, t2.Columna);
                Synchronize("then");
            }

            var thenStmts = ParseStatementList();
            List<StatementNode> elseStmts = null;
            if (MatchKeyword("else"))
            {
                elseStmts = ParseStatementList();
            }

            if (!MatchKeyword("end"))
            {
                var t3 = CurrentToken();
                Error($"Se esperaba 'end' al finalizar la estructura 'if' pero se encontró '{t3.Valor}'", t3.Linea, t3.Columna);
                Synchronize("end");
            }

            return new IfNode(cond, thenStmts, elseStmts);
        }

        // iteracion → while expresion lista_sentencias end
        private WhileNode ParseIteration()
        {
            var cond = ParseExpression();
            if (cond == null)
            {
                var t = CurrentToken();
                Error($"Se esperaba expresión en 'while' pero se encontró '{t.Valor}'", t.Linea, t.Columna);
            }

            var body = ParseStatementList();
            if (!MatchKeyword("end"))
            {
                var t2 = CurrentToken();
                Error($"Se esperaba 'end' al finalizar la estructura 'while' pero se encontró '{t2.Valor}'", t2.Linea, t2.Columna);
                Synchronize("end");
            }

            return new WhileNode(cond, body);
        }

        // repeticion → do lista_sentencias_simple while expresion
        // Cambiamos para que el parser reconozca inmediatamente el 'while' de clausura.
        private DoWhileNode ParseRepetition()
        {
            // 'do' ya consumido

            // 1) Parsear exactamente UNA sentencia como "cuerpo" del do.
            var bodyList = new List<StatementNode>();
            var firstStmt = ParseStatement();
            if (firstStmt != null)
                bodyList.Add(firstStmt);
            else
            {
                var tBody = CurrentToken();
                Error($"Dentro de 'do' se esperaba una sentencia pero se encontró '{tBody.Valor}'", tBody.Linea, tBody.Columna);
            }

            // 2) Ahora consumir el 'while' que cierra el 'do'
            if (!MatchKeyword("while"))
            {
                var t = CurrentToken();
                Error($"Se esperaba 'while' después de 'do' pero se encontró '{t.Valor}'", t.Linea, t.Columna);
                Synchronize("while");
            }

            // 3) Condición del do...while
            var cond = ParseExpression();
            if (cond == null)
            {
                var t2 = CurrentToken();
                Error($"Se esperaba expresión en 'while' de 'do...while' pero se encontró '{t2.Valor}'", t2.Linea, t2.Columna);
            }

            return new DoWhileNode(bodyList, cond);
        }

        // sent_in → cin >> id ;
        private InputNode ParseSentIn()
        {
            // 'cin' ya consumido

            // 1) '>>' → ahora chequeamos como dos operadores “>”
            if (MatchOperator(">") && MatchOperator(">"))
            {
                // OK: consumimos dos tokens “>”
            }
            else
            {
                var t = CurrentToken();
                Error($"Se esperaba '>>' en 'cin >> id' pero se encontró '{t.Valor}'", t.Linea, t.Columna);
            }

            // 2) id
            string id = null;
            if (CheckType("Identificador"))
            {
                id = CurrentToken().Valor;
                Advance();
            }
            else
            {
                var t2 = CurrentToken();
                Error($"Se esperaba un identificador después de 'cin >>' pero se encontró '{t2.Valor}'", t2.Linea, t2.Columna);
            }

            // 3) ';'
            if (!MatchSymbol(";"))
            {
                var t3 = CurrentToken();
                Error($"Se esperaba ';' al final de 'cin >> id' pero se encontró '{t3.Valor}'", t3.Linea, t3.Columna);
                Synchronize(";");
            }

            return new InputNode(id);
        }

        // sent_out → cout << salida [;]
        private OutputNode ParseSentOut()
        {
            // 'cout' ya consumido

            // 1) '<<' → como dos operadores “<”
            if (MatchOperator("<") && MatchOperator("<"))
            {
                // OK: consumimos dos tokens “<”
            }
            else
            {
                var t = CurrentToken();
                Error($"Se esperaba '<<' en 'cout <<' pero se encontró '{t.Valor}'", t.Linea, t.Columna);
            }

            // 2) salida
            var salidaNode = ParseSalida() ?? new LiteralNode("");

            // 3) opcional ';'
            if (MatchSymbol(";"))
            {
                // OK
            }

            return new OutputNode(salidaNode);
        }

        // salida → “cadena” | expresion | “cadena” << expresion | expresion << “cadena”
        private ASTNode ParseSalida()
        {
            // Detectar cadena literal
            if (CheckValorStartsWith("\""))
            {
                var raw = CurrentToken().Valor;
                Advance();
                var lit = new LiteralNode(raw.Trim('"'));

                // caso “cadena” << expresion
                if (MatchSymbol("<") && MatchSymbol("<"))
                {
                    var rightExpr = ParseExpression() ?? new LiteralNode("");
                    return new BinaryOpNode(lit, "<<", rightExpr);
                }

                return lit;
            }
            else
            {
                // expresion
                var expr = ParseExpression();
                if (expr == null)
                {
                    var t = CurrentToken();
                    Error($"Se esperaba expresión o cadena pero se encontró '{t.Valor}'", t.Linea, t.Columna);
                    return null;
                }

                // caso expresion << “cadena”
                if (MatchSymbol("<") && MatchSymbol("<"))
                {
                    if (CheckValorStartsWith("\""))
                    {
                        var raw2 = CurrentToken().Valor;
                        Advance();
                        var lit2 = new LiteralNode(raw2.Trim('"'));
                        return new BinaryOpNode(expr, "<<", lit2);
                    }
                    else
                    {
                        var t2 = CurrentToken();
                        Error($"Se esperaba cadena después de '<<' pero se encontró '{t2.Valor}'", t2.Linea, t2.Columna);
                        return expr;
                    }
                }

                return expr;
            }
        }

        // expresion → relacional ( (&&||) relacional )*
        private ExpressionNode ParseExpression()
        {
            var left = ParseRelational();
            if (left == null) return null;

            while (MatchOperator("&&") || MatchOperator("||"))
            {
                var op = PreviousToken().Valor;
                var right = ParseRelational();
                if (right == null)
                {
                    var t = CurrentToken();
                    Error($"Se esperaba expresión a la derecha de operador '{op}' pero se encontró '{t.Valor}'", t.Linea, t.Columna);
                    right = new LiteralNode("false");
                }
                left = new BinaryOpNode(left, op, right);
            }

            return left;
        }

        // relacional → expresion_simple [ rel_op expresion_simple ]
        private ExpressionNode ParseRelational()
        {
            var left = ParseSimpleExpression();
            if (left == null) return null;

            if (IsRelationalOperator(CurrentToken()))
            {
                var op = CurrentToken().Valor;
                Advance();
                var right = ParseSimpleExpression();
                if (right == null)
                {
                    var t = CurrentToken();
                    Error($"Se esperaba expresión simple después de '{op}' pero se encontró '{t.Valor}'", t.Linea, t.Columna);
                    right = new LiteralNode("0");
                }
                return new BinaryOpNode(left, op, right);
            }

            return left;
        }

        // expresion_simple → termino ( ( + | - | ++ | -- ) termino )*
        private ExpressionNode ParseSimpleExpression()
        {
            var left = ParseTerm();
            if (left == null) return null;

            while (
                MatchOperator("+") ||
                MatchOperator("-") ||
                MatchOperator("++") ||
                MatchOperator("--")
            )
            {
                var op = PreviousToken().Valor;
                var right = ParseTerm();
                if (right == null)
                {
                    var t = CurrentToken();
                    Error($"Se esperaba término después de '{op}' pero se encontró '{t.Valor}'", t.Linea, t.Columna);
                    right = new LiteralNode("0");
                }
                left = new BinaryOpNode(left, op, right);
            }

            return left;
        }

        // termino → factor ( ( * | / | % ) factor )*
        private ExpressionNode ParseTerm()
        {
            var left = ParseFactor();
            if (left == null) return null;

            while (
                MatchOperator("*") ||
                MatchOperator("/") ||
                MatchOperator("%")
            )
            {
                var op = PreviousToken().Valor;
                var right = ParseFactor();
                if (right == null)
                {
                    var t = CurrentToken();
                    Error($"Se esperaba factor después de '{op}' pero se encontró '{t.Valor}'", t.Linea, t.Columna);
                    right = new LiteralNode("0");
                }
                left = new BinaryOpNode(left, op, right);
            }

            return left;
        }

        // factor → componente ( ^ componente )*
        private ExpressionNode ParseFactor()
        {
            var left = ParseComponent();
            if (left == null) return null;

            while (MatchOperator("^"))
            {
                var op = PreviousToken().Valor;
                var right = ParseComponent();
                if (right == null)
                {
                    var t = CurrentToken();
                    Error($"Se esperaba componente después de '{op}' pero se encontró '{t.Valor}'", t.Linea, t.Columna);
                    right = new LiteralNode("0");
                }
                left = new BinaryOpNode(left, op, right);
            }

            return left;
        }

        // componente → ( expresion ) | número | id | bool | ! componente
        private ExpressionNode ParseComponent()
        {
            var t = CurrentToken();

            if (MatchSymbol("("))
            {
                var expr = ParseExpression();
                if (!MatchSymbol(")"))
                {
                    var t2 = CurrentToken();
                    Error($"Se esperaba ')' pero se encontró '{t2.Valor}'", t2.Linea, t2.Columna);
                    Synchronize(")");
                }
                return expr;
            }

            if (CheckKeyword("true") || CheckKeyword("false"))
            {
                var val = CurrentToken().Valor;
                Advance();
                return new LiteralNode(val);
            }

            if (CheckType("Numero") || CheckType("PuntoFlotante"))
            {
                var num = CurrentToken().Valor;
                Advance();
                return new LiteralNode(num);
            }

            if (CheckType("Identificador"))
            {
                var id = CurrentToken().Valor;
                Advance();
                return new IdentifierNode(id);
            }

            if (MatchOperator("!"))
            {
                var operand = ParseComponent();
                if (operand == null)
                {
                    var t3 = CurrentToken();
                    Error($"Se esperaba componente después de '!' pero se encontró '{t3.Valor}'", t3.Linea, t3.Columna);
                    operand = new LiteralNode("false");
                }
                return new BinaryOpNode(new LiteralNode(""), "!", operand);
            }

            Error($"Se esperaba un componente (número, identificador, 'true', 'false', '(', '!') pero se encontró '{t.Valor}'", t.Linea, t.Columna);
            return null;
        }

        #endregion

        #region Utilitarios de Tokens

        private Token CurrentToken()
        {
            if (_position >= 0 && _position < _tokens.Count)
                return _tokens[_position];
            return new Token("EOF", "", -1, -1);
        }

        private Token PreviousToken()
        {
            int idx = _position - 1;
            if (idx >= 0 && idx < _tokens.Count)
                return _tokens[idx];
            return new Token("EOF", "", -1, -1);
        }

        private Token PeekNextToken()
        {
            int idx = _position + 1;
            if (idx >= 0 && idx < _tokens.Count)
                return _tokens[idx];
            return new Token("EOF", "", -1, -1);
        }

        private bool IsAtEnd()
            => _position >= _tokens.Count;

        private void Advance()
        {
            if (!IsAtEnd())
                _position++;
        }

        private void Retreat()
        {
            if (_position > 0)
                _position--;
        }

        private bool CheckType(string tipo)
        {
            var t = CurrentToken();
            return t.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase);
        }

        private bool CheckKeyword(string palabra)
        {
            var t = CurrentToken();
            return t.Tipo.Equals("PalabraReservada", StringComparison.OrdinalIgnoreCase)
                   && t.Valor.Equals(palabra, StringComparison.Ordinal);
        }

        private bool CheckSymbol(string simbolo)
        {
            var t = CurrentToken();
            return t.Tipo.Equals("Simbolo", StringComparison.OrdinalIgnoreCase)
                   && t.Valor.Equals(simbolo, StringComparison.Ordinal);
        }

        private bool CheckOperator(string op)
        {
            var t = CurrentToken();
            return (t.Tipo.Equals("OperadorAritmetico", StringComparison.OrdinalIgnoreCase)
                    || t.Tipo.Equals("OperadorLogico", StringComparison.OrdinalIgnoreCase))
                   && t.Valor.Equals(op, StringComparison.Ordinal);
        }

        private bool CheckValorStartsWith(string prefix)
        {
            var t = CurrentToken();
            return t.Valor.StartsWith(prefix, StringComparison.Ordinal);
        }

        private bool MatchType(string tipo)
        {
            if (CheckType(tipo))
            {
                Advance();
                return true;
            }
            return false;
        }

        private bool MatchKeyword(string palabra)
        {
            if (CheckKeyword(palabra))
            {
                Advance();
                return true;
            }
            return false;
        }

        private bool MatchSymbol(string simbolo)
        {
            if (CheckSymbol(simbolo))
            {
                Advance();
                return true;
            }
            return false;
        }

        private bool MatchOperator(string op)
        {
            if (CheckOperator(op))
            {
                if (op.Length == 2)
                {
                    if (CurrentToken().Valor.Equals(op, StringComparison.Ordinal))
                    {
                        Advance();
                        return true;
                    }
                    return false;
                }
                else
                {
                    if (CurrentToken().Valor.Equals(op, StringComparison.Ordinal))
                    {
                        Advance();
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        private bool PeekNextIs(string valor)
        {
            var next = PeekNextToken();
            return next != null && next.Valor.Equals(valor, StringComparison.Ordinal);
        }

        #endregion

        #region Manejo de Errores y Sincronización

        private void Error(string message, int linea, int columna)
        {
            Errors.Add($"Error sintáctico en línea {linea}, columna {columna}: {message}");
        }

        private bool Synchronize(string valorEspera)
        {
            while (!IsAtEnd())
            {
                if (CurrentToken().Valor.Equals(valorEspera, StringComparison.Ordinal))
                    return true;
                Advance();
            }
            return false;
        }

        #endregion

        #region Verificación de Inicio de Sentencia y Operadores Relacionales

        private bool IsStartOfStatement(Token t)
        {
            if (t == null) return false;
            if (t.Tipo.Equals("PalabraReservada", StringComparison.OrdinalIgnoreCase))
            {
                return t.Valor switch
                {
                    "if" or "while" or "do" or "cin" or "cout" => true,
                    _ => false,
                };
            }
            if (t.Tipo.Equals("Identificador", StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

        private bool IsRelationalOperator(Token t)
        {
            if (t == null || !t.Tipo.Equals("OperadorLogico", StringComparison.OrdinalIgnoreCase))
                return false;
            return t.Valor is "<" or "<=" or ">" or ">=" or "==" or "!=";
        }

        #endregion
    }
}
