# Scripting-language-interpreter
A full-featured scripting language interpreter based on [Crafting Interpreters](http://craftinginterpreters.com/).
This interpreter handles scripts written in the [Lox](http://www.craftinginterpreters.com/the-lox-language.html) language, a high-level scripting language based on the ECMA specifications of JavaScript.

# About this project
This project is at the forefront of an open-source serious game (currently in pre-production). The goal is to write a scripting language interpreter that can perform runtime behaviour in game engines like Unity.

## The interpreter
This interpreter performs a few steps when receiving input:
1. Lexical analysis
2. Parsing
3. Static analysis
4. Intermediate representation
5. Optimization
6. Code generation
7. Runtime representation

## The Lox language
[Lox](http://www.craftinginterpreters.com/the-lox-language.html) is similair to high-level scripting languages like JavaScript and Lua. Two of the main aspects are:
- Dynamic typing
- Automatic memory management

Besides those aspects Box can handle:
- Data types
- Expressions
- Statements
- Variables
- Control flows
- Functions
- Classes

An example of Lox code:
```javascript
// This is a test
var test = 5;
for (var i = 0; i < test; i = i + 1)
{
    print i;
}
```
