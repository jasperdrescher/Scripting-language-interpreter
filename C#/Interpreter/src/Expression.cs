using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter
{
    public abstract class Expr
    {
        public interface Visitor<T>
        {
            T visitAssignExpr(Assign expr);
            T visitBinaryExpr(Binary expr);
            T visitCallExpr(Call expr);
            T visitGetExpr(Get expr);
            T visitGroupingExpr(Grouping expr);
            T visitLiteralExpr(Literal expr);
            T visitLogicalExpr(Logical expr);
            T visitSetExpr(Set expr);
            T visitSuperExpr(Super expr);
            T visitThisExpr(This expr);
            T visitUnaryExpr(Unary expr);
            T visitVariableExpr(Variable expr);
        }

        public class Assign : Expr
        {
            public Assign(Token name, Expr value)
            {
                this.name = name;
                this.value = value;
            }

            public override Expresion accept<Expresion>(Visitor<Expresion> visitor)
            {
                return visitor.visitAssignExpr(this);
            }

            public Token name;
            public Expr value;
        }

        public class Binary : Expr
        {
            public Binary(Expr left, Token oper, Expr right)
            {
                this.left = left;
                this.oper = oper;
                this.right = right;
            }

            public override Expresion accept<Expresion>(Visitor<Expresion> visitor)
            {
                return visitor.visitBinaryExpr(this);
            }

            public Expr left;
            public Token oper;
            public Expr right;
        }

        public class Call : Expr
        {
            public Call(Expr callee, Token paren, List<Expr> arguments)
            {
                this.callee = callee;
                this.paren = paren;
                this.arguments = arguments;
            }

            public override Expresion accept<Expresion>(Visitor<Expresion> visitor)
            {
                return visitor.visitCallExpr(this);
            }

            public Expr callee;
            public Token paren;
            public List<Expr> arguments;
        }

        public class Get : Expr
        {
            public Get(Expr obj, Token name)
            {
                this.obj = obj;
                this.name = name;
            }

            public override Expresion accept<Expresion>(Visitor<Expresion> visitor)
            {
                return visitor.visitGetExpr(this);
            }

            public Expr obj;
            public Token name;
        }

        public class Grouping : Expr
        {
            public Grouping(Expr expression)
            {
                this.expression = expression;
            }

            public override Expresion accept<Expresion>(Visitor<Expresion> visitor)
            {
                return visitor.visitGroupingExpr(this);
            }

            public Expr expression;
        }

        public class Literal : Expr
        {
            public Literal(Object value)
            {
                this.value = value;
            }

            public override Expresion accept<Expresion>(Visitor<Expresion> visitor)
            {
                return visitor.visitLiteralExpr(this);
            }

            public Object value;
        }

        public class Logical : Expr
        {
            public Logical(Expr left, Token oper, Expr right)
            {
                this.left = left;
                this.oper = oper;
                this.right = right;
            }

            public override Expresion accept<Expresion>(Visitor<Expresion> visitor)
            {
                return visitor.visitLogicalExpr(this);
            }

            public Expr left;
            public Token oper;
            public Expr right;
        }

        public class Set : Expr
        {
            public Set(Expr obj, Token name, Expr value)
            {
                this.obj = obj;
                this.name = name;
                this.value = value;
            }

            public override Expresion accept<Expresion>(Visitor<Expresion> visitor)
            {
                return visitor.visitSetExpr(this);
            }

            public Expr obj;
            public Token name;
            public Expr value;
        }

        public class Super : Expr
        {
            public Super(Token keyword, Token method)
            {
                this.keyword = keyword;
                this.method = method;
            }

            public override Expresion accept<Expresion>(Visitor<Expresion> visitor)
            {
                return visitor.visitSuperExpr(this);
            }

            public Token keyword;
            public Token method;
        }

        public class This : Expr
        {
            public This(Token keyword)
            {
                this.keyword = keyword;
            }

            public override Expresion accept<Expresion>(Visitor<Expresion> visitor)
            {
                return visitor.visitThisExpr(this);
            }

            public Token keyword;
        }

        public class Unary : Expr
        {
            public Unary(Token oper, Expr right)
            {
                this.oper = oper;
                this.right = right;
            }

            public override Expresion accept<Expresion>(Visitor<Expresion> visitor)
            {
                return visitor.visitUnaryExpr(this);
            }

            public Token oper;
            public Expr right;
        }

        public class Variable : Expr
        {
            public Variable(Token name)
            {
                this.name = name;
            }

            public override Expresion accept<Expresion>(Visitor<Expresion> visitor)
            {
                return visitor.visitVariableExpr(this);
            }

            public Token name;
        }

        public abstract Expr accept<Expr>(Visitor<Expr> visitor);
    }
}
