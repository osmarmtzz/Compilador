using System;
using System.Collections.Generic;

namespace IDE_COMPILADOR.AnalizadorSintactico.AST
{
    // Nodo base de todo el AST
    public abstract class ASTNode { }

    // ───────────────────────────────────────────────────────────────────────
    // Programa → lista de declaraciones
    // ───────────────────────────────────────────────────────────────────────
    public class ProgramNode : ASTNode
    {
        public List<DeclarationNode> Declarations { get; }

        public ProgramNode(List<DeclarationNode> declarations)
        {
            Declarations = declarations;
        }
    }

    // ───────────────────────────────────────────────────────────────────────
    // DECLARACIÓN: puede ser VariableDeclarationNode o StatementListNode
    // ───────────────────────────────────────────────────────────────────────
    public abstract class DeclarationNode : ASTNode { }

    // VariableDeclarationNode → tipo Identificador ( , Identificador )* ;
    public class VariableDeclarationNode : DeclarationNode
    {
        public string TypeName { get; }
        public List<string> Identifiers { get; }

        public VariableDeclarationNode(string typeName, List<string> identifiers)
        {
            TypeName = typeName;
            Identifiers = identifiers;
        }
    }

    // StatementListNode → lista de sentencias (para anidar dentro de ProgramNode)
    public class StatementListNode : DeclarationNode
    {
        public List<StatementNode> Statements { get; }

        public StatementListNode(List<StatementNode> stmts)
        {
            Statements = stmts;
        }
    }

    // ====================================================================
    // ──────── Nodos de Sentencia ────────────────────────────────────────
    // ====================================================================
    public abstract class StatementNode : ASTNode { }

    // Asignación → id = expresión ;
    public class AssignmentNode : StatementNode
    {
        public string Identifier { get; }
        public ExpressionNode Expression { get; }

        public AssignmentNode(string id, ExpressionNode expr)
        {
            Identifier = id;
            Expression = expr;
        }
    }

    // IfNode → if expresión then lista_sentencias [ else lista_sentencias ] end
    public class IfNode : StatementNode
    {
        public ExpressionNode Condition { get; }
        public List<StatementNode> ThenBranch { get; }
        public List<StatementNode> ElseBranch { get; }

        public IfNode(ExpressionNode cond, List<StatementNode> thenB, List<StatementNode> elseB)
        {
            Condition = cond;
            ThenBranch = thenB;
            ElseBranch = elseB;
        }
    }

    // WhileNode → while expresión lista_sentencias end
    public class WhileNode : StatementNode
    {
        public ExpressionNode Condition { get; }
        public List<StatementNode> Body { get; }

        public WhileNode(ExpressionNode cond, List<StatementNode> body)
        {
            Condition = cond;
            Body = body;
        }
    }

    // DoWhileNode → do lista_sentencias while expresión end
    public class DoWhileNode : StatementNode
    {
        public List<StatementNode> Body { get; }
        public ExpressionNode Condition { get; }

        public DoWhileNode(List<StatementNode> body, ExpressionNode cond)
        {
            Body = body;
            Condition = cond;
        }
    }

    // InputNode → cin >> id ;
    public class InputNode : StatementNode
    {
        public string Identifier { get; }

        public InputNode(string id)
        {
            Identifier = id;
        }
    }

    // OutputNode → cout << salida [;]
    public class OutputNode : StatementNode
    {
        public ASTNode Value { get; }
        public bool ExprFirst { get; }

        public OutputNode(ASTNode value, bool exprFirst = false)
        {
            Value = value;
            ExprFirst = exprFirst;
        }
    }

    // ───────────────────────────────────────────────────────────────────────
    // Nodos para post-incremento/post-decremento y do…until
    // ───────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Nodo para sentencia de post-incremento (id++) o post-decremento (id--).
    /// </summary>
    public class UnaryPostfixNode : StatementNode
    {
        public string Identifier { get; }
        public string Operator { get; }   // "++" o "--"

        public UnaryPostfixNode(string identifier, string op)
        {
            Identifier = identifier;
            Operator = op;
        }
    }

    /// <summary>
    /// Nodo para la estructura:
    ///   do
    ///       (cero o más sentencias)
    ///       while <expresión>          ← bucle interno
    ///           (cero o más sentencias dentro del while interno)
    ///       end                        ← cierra el while interno
    ///   until <expresión> ;             ← cierra el do
    /// </summary>
    public class DoUntilNode : StatementNode
    {
        // Sentencias justo después de 'do' y antes del 'while' interno
        public List<StatementNode> BodyDo { get; }

        // Condición del while interno
        public ExpressionNode ConditionWhile { get; }

        // Sentencias dentro del while interno (hasta su 'end')
        public List<StatementNode> BodyWhile { get; }

        // Condición del 'until' que cierra el bloque do
        public ExpressionNode ConditionUntil { get; }

        public DoUntilNode(
            List<StatementNode> bodyDo,
            ExpressionNode conditionWhile,
            List<StatementNode> bodyWhile,
            ExpressionNode conditionUntil)
        {
            BodyDo = bodyDo;
            ConditionWhile = conditionWhile;
            BodyWhile = bodyWhile;
            ConditionUntil = conditionUntil;
        }
    }

    // ====================================================================
    // ──────── Nodos de Expresión ────────────────────────────────────────
    // ====================================================================
    public abstract class ExpressionNode : ASTNode { }

    // LiteralNode → número, punto flotante, true/false
    public class LiteralNode : ExpressionNode
    {
        public string Value { get; }

        public LiteralNode(string v)
        {
            Value = v;
        }

        public override string ToString() => Value;
    }

    // BinaryOpNode → operación binaria (left op right)
    public class BinaryOpNode : ExpressionNode
    {
        public ExpressionNode Left { get; }
        public string Operator { get; }
        public ExpressionNode Right { get; }

        public BinaryOpNode(ExpressionNode l, string op, ExpressionNode r)
        {
            Left = l;
            Operator = op;
            Right = r;
        }

        public override string ToString() => $"({Left} {Operator} {Right})";
    }

    // IdentifierNode → variable
    public class IdentifierNode : ExpressionNode
    {
        public string Name { get; }

        public IdentifierNode(string n)
        {
            Name = n;
        }

        public override string ToString() => Name;
    }
}
