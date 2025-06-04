using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE_COMPILADOR.AnalizadorSintactico.AST
{
    public abstract class ASTNode { }

    public class ProgramNode : ASTNode
    {
        public List<DeclarationNode> Declarations { get; }
        public ProgramNode(List<DeclarationNode> declarations)
            => Declarations = declarations;
    }

    public abstract class DeclarationNode : ASTNode { }

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

    public class StatementListNode : DeclarationNode
    {
        public List<StatementNode> Statements { get; }
        public StatementListNode(List<StatementNode> stmts)
            => Statements = stmts;
    }

    public abstract class StatementNode : ASTNode { }

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

    public class InputNode : StatementNode
    {
        public string Identifier { get; }
        public InputNode(string id) => Identifier = id;
    }

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

    public abstract class ExpressionNode : ASTNode { }

    public class LiteralNode : ExpressionNode
    {
        public string Value { get; }
        public LiteralNode(string v) => Value = v;
    }

    public class BinaryOpNode : ExpressionNode
    {
        public ExpressionNode Left { get; }
        public string Operator { get; }
        public ExpressionNode Right { get; }
        public BinaryOpNode(ExpressionNode l, string op, ExpressionNode r)
        {
            Left = l; Operator = op; Right = r;
        }
    }

    public class IdentifierNode : ExpressionNode
    {
        public string Name { get; }
        public IdentifierNode(string n) => Name = n;
    }
}