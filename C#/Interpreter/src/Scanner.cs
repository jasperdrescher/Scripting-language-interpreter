﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter
{
    public class Scanner
    {
        private static Dictionary<string, TokenType> keywords;

        private string source;
        private List<Token> tokens = new List<Token>();
        private int start = 0;
        private int current = 0;
        private int line = 1;

        public Scanner(string source)
        {
            this.source = source;
        }

        public List<Token> scanTokens()
        {
            while (!isAtEnd())
            {
                // We are at the beginning of the next lexeme.
                start = current;
                scanToken();
            }

            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        private void scanToken()
        {
            char c = advance();
            switch (c)
            {
                case '(': addToken(TokenType.LEFT_PAREN); break;
                case ')': addToken(TokenType.RIGHT_PAREN); break;
                case '{': addToken(TokenType.LEFT_BRACE); break;
                case '}': addToken(TokenType.RIGHT_BRACE); break;
                case ',': addToken(TokenType.COMMA); break;
                case '.': addToken(TokenType.DOT); break;
                case '-': addToken(TokenType.MINUS); break;
                case '+': addToken(TokenType.PLUS); break;
                case ';': addToken(TokenType.SEMICOLON); break;
                case '*': addToken(TokenType.STAR); break;
                case '!': addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
                case '=': addToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
                case '<': addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '>': addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
                case '/':
                    if (match('/'))
                    {
                        // A comment goes until the end of the line.                
                        while (peek() != '\n' && !isAtEnd()) advance();
                    }
                    else
                    {
                        addToken(TokenType.SLASH);
                    }
                    break;
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace.                      
                    break;
                case '\n':
                    line++;
                    break;
                case '"': interpretString(); break;
                default:
                    if (isDigit(c))
                    {
                        number();
                    }
                    else if (isAlpha(c))
                    {
                        identifier();
                    }
                    else
                    {
                        Box.Box.error(line, "Unexpected character.");
                    }
                    break;
            }
        }

        private void identifier()
        {
            while (isAlphaNumeric(peek())) advance();

            // See if the identifier is a reserved word.   
            string text = source.Substring(start, current - start + 1);

            TokenType type = keywords[text];
            if (type == TokenType.NONE) type = TokenType.IDENTIFIER;
            addToken(type);
        }

        private bool isAlphaNumeric(char c)
        {
            return isAlpha(c) || isDigit(c);
        }

        private void number()
        {
            while (isDigit(peek())) advance();

            // Look for a fractional part.                            
            if (peek() == '.' && isDigit(peekNext()))
            {
                // Consume the "."                                      
                advance();

                while (isDigit(peek())) advance();
            }

            addToken(TokenType.NUMBER, double.Parse(source.Substring(start, current - start + 1)));
        }

        private void interpretString()
        {
            while (peek() != '"' && !isAtEnd()) {
                if (peek() == '\n') line++;
                advance();
            }

            // Unterminated string.                                 
            if (isAtEnd()) {
                Box.Box.error(line, "Unterminated string.");
                return;
            }

            // The closing ".                                       
            advance();

            // Trim the surrounding quotes.                         
            string value = source.Substring(start + 1, current - 1 - start + 1);
            addToken(TokenType.STRING, value);
        }

        private bool match(char expected)
        {
            if (isAtEnd()) return false;
            if (source[current] != expected) return false;

            current++;
            return true;
        }

        private char peek()
        {
            if (isAtEnd()) return '\0';
            return source[current];
        }

        private char peekNext()
        {
            if (current + 1 >= source.Length) return '\0';
            return source[current + 1];
        }

        private bool isAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                    c == '_';
        }

        private bool isDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool isAtEnd()
        {
            return current >= source.Length;
        }

        private char advance()
        {
            current++;
            return source[current - 1];
        }

        private void addToken(TokenType type)
        {
            addToken(type, null);
        }

        private void addToken(TokenType type, object literal)
        {
            string text = source.Substring(start, current - start + 1);
            tokens.Add(new Token(type, text, literal, line));
        }
    }
}