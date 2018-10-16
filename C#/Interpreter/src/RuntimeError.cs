using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter
{
    public class RuntimeError
    {
        private Token token;
        private string message;

        public RuntimeError(Token token, string message)
        {
            this.token = token;
            this.message = message;
        }

        public Token GetToken()
        {
            return token;
        }

        public string GetMessage()
        {
            return message;
        }
    }
}
