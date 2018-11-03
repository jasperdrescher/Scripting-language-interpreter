using System;
using System.Collections.Generic;
using System.Text;
using Interpreter.Box;
using Interpreter.Utils;

namespace Interpreter
{
    class Clock : BoxCallable
    {
        public int arity() { return 0; }

        public object call(Interpreter interpreter, List<object> arguments)
        {
            return (double)DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public string toString()
        {
            return "<native fn>";
        }
    }

    public class Interpreter : Expr.Visitor<object>, Stmt.Visitor<object>
    {
        public static Environment globals = new Environment();
        private Environment environment = globals;
        private Dictionary<Expr, int> locals = new Dictionary<Expr, int>();
        private WritableOutput writableOutput;

        public Interpreter(WritableOutput writableOutput)
        {
            this.writableOutput = writableOutput;
            globals.define("clock", new Clock());
        }

        public void interpret(List<Stmt> statements)
        {
          //  try
          //  {
                foreach (Stmt statement in statements)
                {
                    execute(statement);
                }
            //   }
            //    catch (InterpreterError error)
            //    {
            //         Box.Box.ReportError(error);
            //   }
            // throw new InterpreterError(null, "ops");
        }

        public object visitLiteralExpr(Expr.Literal expr)
        {
            return expr.value;
        }

        public object visitLogicalExpr(Expr.Logical expr)
        {
            object left = evaluate(expr.left);

            if (expr.oper.type == TokenType.OR)
            {
                if (isTruthy(left)) return left;
            }
            else
            {
                if (!isTruthy(left)) return left;
            }

            return evaluate(expr.right);
        }

        public object visitSetExpr(Expr.Set expr)
        {
            object obj = evaluate(expr.obj);

            if (!(obj is Box.BoxInstance))
            {
                throw new InterpreterError(expr.name, "Only instances have fields.");
            }

            object value = evaluate(expr.value);
            ((Box.BoxInstance)obj).set(expr.name, value);
            return value;
        }

        public object visitSuperExpr(Expr.Super expr)
        {
            int distance = locals[expr];
            Box.BoxClass superclass = (Box.BoxClass)environment.getAt(distance, "super");

            // "this" is always one level nearer than "super"'s environment.
            Box.BoxInstance obj = (Box.BoxInstance)environment.getAt(distance - 1, "this");

            Box.BoxFunction method = superclass.findMethod(obj, expr.method.lexeme);

            if (method == null)
            {
                throw new InterpreterError(expr.method, "Undefined property '" + expr.method.lexeme + "'.");
            }

            return method;
        }

        public object visitThisExpr(Expr.This expr)
        {
            return lookUpVariable(expr.keyword, expr);
        }

        public object visitUnaryExpr(Expr.Unary expr)
        {
            object right = evaluate(expr.right);

            switch (expr.oper.type)
            {  
	            case TokenType.BANG:
                    return !isTruthy(right);
	            case TokenType.MINUS:
                    checkNumberOperand(expr.oper, right);
                    return -(double)right;
            }

            // Unreachable.                              
            return null;
        }

        public object visitVariableExpr(Expr.Variable expr)
        {
            return lookUpVariable(expr.name, expr);
        }

        private object lookUpVariable(Token name, Expr expr)
        {
            int distance = -1;
            if (locals.ContainsKey(expr))
            {
                distance = locals[expr];
            }

            if (distance > -1)
            {
                return environment.getAt(distance, name.lexeme);
            }
            else
            {
                return globals.Get(name);
            }
        }

        private void checkNumberOperand(Token oper, object operand)
        {
            if (operand is double) return;
            throw new InterpreterError(oper, "Operand must be a number.");
        }

        private void checkNumberOperands(Token oper, object left, object right)
        {
            if (left is double && right is double) return;

            throw new InterpreterError(oper, "Operands must be numbers.");
        }

        private bool isTruthy(object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;
            return true;
        }

        private bool isEqual(object a, object b)
        {
            // nil is only equal to nil.               
            if (a == null && b == null) return true;
            if (a == null) return false;

            return a.Equals(b);
        }

        private string stringify(object obj)
        {
            if (obj == null) return "nil";
            
            if (obj is double)
            {
                string text = obj.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text.IndexedSubstring(0, text.Length - 2);
                }
                return text;
            }

            return obj.ToString();
        }

        public object visitGroupingExpr(Expr.Grouping expr)
        {
            return evaluate(expr.expression);
        }

        private object evaluate(Expr expr)
        {
            return expr.accept(this);
        }

        private void execute(Stmt stmt)
        {
            stmt.accept(this);
        }

        public void resolve(Expr expr, int depth)
        {
            locals[expr] = depth;
        }

        public void executeBlock(List<Stmt> statements, Environment environment)
        {
            Environment previous = this.environment;
            try
            {
                this.environment = environment;

                foreach (Stmt statement in statements)
                {
                    execute(statement);
                }
            }
            finally
            {
                this.environment = previous;
            }
        }

        public object visitBlockStmt(Stmt.Block stmt)
        {
            executeBlock(stmt.statements, new Environment(environment));
            return null;
        }

        public object visitClassStmt(Stmt.Class stmt)
        {
            object superclass = null;
            if (stmt.superclass != null)
            {
                superclass = evaluate(stmt.superclass);
                if (!(superclass is Box.BoxClass))
                {
                    throw new InterpreterError(stmt.superclass.name, "Superclass must be a class.");
                }
            }

            environment.define(stmt.name.lexeme, null);

            if (stmt.superclass != null)
            {
                environment = new Environment(environment);
                environment.define("super", superclass);
            }

            Dictionary<string, Box.BoxFunction> methods = new Dictionary<string, Box.BoxFunction>();

            foreach (Stmt.Function method in stmt.methods)
            {
                Box.BoxFunction function = new Box.BoxFunction(method, environment, method.name.lexeme.Equals("init"));
                methods[method.name.lexeme] = function;
            }

            Box.BoxClass klass = new Box.BoxClass(stmt.name.lexeme, (Box.BoxClass)superclass, methods);

            if (superclass != null)
            {
                environment = environment.enclosing;
            }
            environment.assign(stmt.name, klass);
            return null;
        }

        public object visitExpressionStmt(Stmt.Expression stmt)
        {
            evaluate(stmt.expression);
            return null;
        }

        public object visitFunctionStmt(Stmt.Function stmt)
        {
            Box.BoxFunction function = new Box.BoxFunction(stmt, environment, false);
            environment.define(stmt.name.lexeme, function);
            return null;
        }

        public object visitIfStmt(Stmt.If stmt)
        {
            if (isTruthy(evaluate(stmt.condition)))
            {
                execute(stmt.thenBranch);
            }
            else if (stmt.elseBranch != null)
            {
                execute(stmt.elseBranch);
            }
            return null;
        }

        public object visitPrintStmt(Stmt.Print stmt)
        {
            object value = evaluate(stmt.expression);
            writableOutput.WriteLine(stringify(value));
            return null;
        }

        public object visitReturnStmt(Stmt.Return stmt)
        {
            object value = null;
            if (stmt.value != null) value = evaluate(stmt.value);

            throw new Return(value);
        }

        public object visitVarStmt(Stmt.Var stmt)
        {
            object value = null;
            if (stmt.initializer != null)
            {
                value = evaluate(stmt.initializer);
            }

            environment.define(stmt.name.lexeme, value);
            return null;
        }

        public object visitWhileStmt(Stmt.While stmt)
        {
            while (isTruthy(evaluate(stmt.condition)))
            {
                execute(stmt.body);
            }
            return null;
        }

        public object visitAssignExpr(Expr.Assign expr)
        {
            object value = evaluate(expr.value);

            int distance = -1;

            if (locals.ContainsKey(expr))
            {
                distance = locals[expr];
            }

            if (distance > -1)
            {
                environment.assignAt(distance, expr.name, value);
            }
            else
            {
                globals.assign(expr.name, value);
            }

            return value;
        }

        public object visitBinaryExpr(Expr.Binary expr)
        {
            object left = evaluate(expr.left);
            object right = evaluate(expr.right);

            switch (expr.oper.type)
            {
                case TokenType.BANG_EQUAL: return !isEqual(left, right);
                case TokenType.EQUAL_EQUAL: return isEqual(left, right);
                case TokenType.GREATER:
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left > (double)right;
                case TokenType.GREATER_EQUAL:
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left >= (double)right;
                case TokenType.LESS:
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left < (double)right;
                case TokenType.LESS_EQUAL:
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left <= (double)right;
                case TokenType.MINUS:
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left - (double)right;
                case TokenType.PLUS:
                    if (left is double && right is double)
                    {
                        return (double)left + (double)right;
                    }

                    if (left is string && right is string) {
                        return (string)left + (string)right;
                    }

                    throw new InterpreterError(expr.oper, "Operands must be two numbers or two strings.");
                case TokenType.SLASH:
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left / (double)right;
                case TokenType.STAR:
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left * (double)right;
            }

            // Unreachable.                                
            return null;
        }

        public object visitCallExpr(Expr.Call expr)
        {
            object callee = evaluate(expr.callee);

            List<object> arguments = new List<object>();

            foreach (Expr argument in expr.arguments)
            {
                arguments.Add(evaluate(argument));
            }

            if (!(callee is Box.BoxCallable))
            {
                throw new InterpreterError(expr.paren, "Can only call functions and classes.");
            }

            Box.BoxCallable function = (Box.BoxCallable)callee;

            if (arguments.Count != function.arity())
            {
                throw new InterpreterError(expr.paren, "Expected " + function.arity() + " arguments but got " + arguments.Count + ".");
            }

            return function.call(this, arguments);
        }

        public object visitGetExpr(Expr.Get expr)
        {
            object obj = evaluate(expr.obj);
            if (obj is Box.BoxInstance)
            {
                return ((Box.BoxInstance)obj).Get(expr.name);
            }

            throw new InterpreterError(expr.name, "Only instances have properties.");
        }
    }
}