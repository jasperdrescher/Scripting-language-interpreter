using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter
{
    public abstract class Stmt
    {
        public interface Visitor<T>
        {
            T visitBlockStmt(Block stmt);
            T visitClassStmt(Class stmt);
            T visitExpressionStmt(Expression stmt);
            T visitFunctionStmt(Function stmt);
            T visitIfStmt(If stmt);
            T visitPrintStmt(Print stmt);
            T visitReturnStmt(Return stmt);
            T visitVarStmt(Var stmt);
            T visitWhileStmt(While stmt);
        }

        public class Block : Stmt
        {
            Block(List<Stmt> statements)
            {
                this.statements = statements;
            }

            public override Stmt accept<Stmt>(Visitor<Stmt> visitor)
            {
                return visitor.visitBlockStmt(this);
            }

            public List<Stmt> statements;
        }

        public class Class : Stmt
        {
            Class(Token name, Expr.Variable superclass, List<Stmt.Function> methods)
            {
                this.name = name;
                this.superclass = superclass;
                this.methods = methods;
            }

            public override Stmt accept<Stmt>(Visitor<Stmt> visitor)
            {
                return visitor.visitClassStmt(this);
            }

            public Token name;
            public Expr.Variable superclass;
            public List<Stmt.Function> methods;
        }

        public class Expression : Stmt
        {
            Expression(Expression expression)
            {
                this.expression = expression;
            }

            public override Stmt accept<Stmt>(Visitor<Stmt> visitor)
            {
                return visitor.visitExpressionStmt(this);
            }

            public Expression expression;
        }

        public class Function : Stmt
        {
            Function(Token name, List<Token> parameters, List<Stmt> body)
            {
                this.name = name;
                this.parameters = parameters;
                this.body = body;
            }

            public override Stmt accept<Stmt>(Visitor<Stmt> visitor)
            {
                return visitor.visitFunctionStmt(this);
            }

            public Token name;
            public List<Token> parameters;
            public List<Stmt> body;
        }

        public class If : Stmt
        {
            If(Expr condition, Stmt thenBranch, Stmt elseBranch)
            {
                this.condition = condition;
                this.thenBranch = thenBranch;
                this.elseBranch = elseBranch;
            }

            public override Stmt accept<Stmt>(Visitor<Stmt> visitor)
            {
                return visitor.visitIfStmt(this);
            }

            public Expr condition;
            public Stmt thenBranch;
            public Stmt elseBranch;
        }

        public class Print : Stmt
        {
            Print(Expr expression)
            {
                this.expression = expression;
            }

            public override Stmt accept<Stmt>(Visitor<Stmt> visitor)
            {
                return visitor.visitPrintStmt(this);
            }

            public Expr expression;
        }

        public class Return : Stmt
        {
            Return(Token keyword, Expr value)
            {
                this.keyword = keyword;
                this.value = value;
            }

            public override Stmt accept<Stmt>(Visitor<Stmt> visitor)
            {
                return visitor.visitReturnStmt(this);
            }

            public Token keyword;
            public Expr value;
        }

        public class Var : Stmt
        {
            Var(Token name, Expr initializer)
            {
                this.name = name;
                this.initializer = initializer;
            }

            public override Stmt accept<Stmt>(Visitor<Stmt> visitor)
            {
                return visitor.visitVarStmt(this);
            }

            public Token name;
            public Expr initializer;
        }

        public class While : Stmt
        {
            public While(Expr condition, Stmt body)
            {
                this.condition = condition;
                this.body = body;
            }

            public override Stmt accept<Stmt>(Visitor<Stmt> visitor)
            {
                return visitor.visitWhileStmt(this);
            }

            public Expr condition;
            public Stmt body;
        }

        public abstract Stmt accept<Stmt>(Visitor<Stmt> visitor);
    }
}
