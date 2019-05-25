# Scripting-language-interpreter
A full-featured scripting language interpreter based on [Crafting Interpreters](http://craftinginterpreters.com/).
This interpreter handles scripts written in the Box language, a high-level scripting language based on the ECMA specifications of JavaScript.

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

## The Box language
Box is similair to high-level scripting languages liek JavaScript and Lua. Two of the main aspects are:
- Dynamic typing
- Automatic memory management
