using System;
using System.Collections.Generic;

namespace calc {
    public class Tokenizer {
        private int iterator;
        private string input;
        private List<Token> tokens;
        public string Input {
            get => input;
            set {
                iterator = 0;
                tokens = new List<Token>();
                input = value;
            }
        }
        public List<Token> Tokens => tokens;

        public Tokenizer(string input) {
            this.input = input;
            tokens = new List<Token>();
            iterator = 0;
        }

        public void Parse() {
            var tokens = new List<Token>();
            Token currToken;
            do {
                currToken = GetNextToken();
                if (currToken.TokenType == TokenType.EOF || currToken.TokenType == TokenType.Invalid)
                    break;
                tokens.Add(currToken);
            } while (true);

            this.tokens = tokens;
        }

        private Token GetNextToken() {
            if (iterator >= input.Length) {
                return new Token(TokenType.EOF, null);
            }

            if (char.IsWhiteSpace(input[iterator])) {
                iterator++;
                return GetNextToken();
            }

            if (char.IsDigit(input[iterator])) {
                var number = "";
                while (iterator < input.Length && char.IsDigit(input[iterator])) {
                    number += input[iterator++];
                }

                var token = new Token(TokenType.Integer, int.Parse(number));
                return token;
            }

            if (input[iterator] == '+') {
                var token = new Token(TokenType.Plus, "+");
                iterator++;
                return token;
            }

            if (input[iterator] == '-') {
                var token = new Token(TokenType.Minus, "-");
                iterator++;
                return token;
            }
            
            if (input[iterator] == '*') {
                var token = new Token(TokenType.Multiply, "*");
                iterator++;
                return token;
            }

            if (input[iterator] == '/') {
                var token = new Token(TokenType.Divide, "/");
                iterator++;
                return token;
            }
            
            if (input[iterator] == '^') {
                var token = new Token(TokenType.Power, "^");
                iterator++;
                return token;
            }

            iterator++;
            return new Token(TokenType.Invalid, null);
        }

        public bool IsValid() {
            for (int i = 0; i < tokens.Count; i++) {
                if (tokens[i].TokenType == TokenType.Integer) {
                    if (i > 0 && tokens[i - 1].TokenType == TokenType.Integer) return false;
                    if (i < tokens.Count - 1 && tokens[i + 1].TokenType == TokenType.Integer) return false;
                    continue;
                }
                if (i == 0 || i == tokens.Count - 1) return false;
                if (tokens[i - 1].TokenType != TokenType.Integer || tokens[i + 1].TokenType != TokenType.Integer) return false;
            }

            return true;
        }

        public BinaryTree<Token> BuildAST() {
            BinaryTree<Token> AST = new BinaryTree<Token>(tokens[0]);
            for (int i = 1; i < tokens.Count; i++) {
                var token = tokens[i];
                if (token >= AST.Value) {
                    var newAST = new BinaryTree<Token>(token);
                    AST.Parent = newAST;
                    newAST.Left = AST;

                    AST = newAST;
                } else if (token < AST.Value) {
                    var leaf = new BinaryTree<Token>(token) {Parent = AST};
                    AST.Right = leaf;
                }
            }

            return AST;
        }

        public double EvaluateAST(BinaryTree<Token> ast) {
            switch (ast.Value.TokenType) {
                case TokenType.Plus:
                    return EvaluateAST(ast.Left) + EvaluateAST(ast.Right);
                case TokenType.Minus:
                    return EvaluateAST(ast.Left) - EvaluateAST(ast.Right);
                case TokenType.Multiply:
                    return EvaluateAST(ast.Left) * EvaluateAST(ast.Right);
                case TokenType.Divide:
                    return EvaluateAST(ast.Left) / EvaluateAST(ast.Right);
                case TokenType.Power:
                    return Math.Pow(EvaluateAST(ast.Left), EvaluateAST(ast.Right));
                default:
                    return ast.Value.TokenValue;
            }
        }
    }
}