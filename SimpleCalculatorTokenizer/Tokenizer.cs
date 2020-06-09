using System;
using System.Collections.Generic;
using System.Linq;

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
                if (currToken.TokenType == TokenType.EOF)
                    break;
                if (currToken.TokenType == TokenType.Invalid) {
                    tokens = null;
                    break;
                }
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
                var token = new Token(TokenType.Add, "+");
                iterator++;
                return token;
            }

            if (input[iterator] == '-') {
                var token = new Token(TokenType.Subtract, "-");
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
            if (tokens == null) return false;
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
            BinaryTree<Token> root = new BinaryTree<Token>(tokens[0]);
            for (var i = 1; i < tokens.Count; i++) {
                var token = tokens[i];
                if (token.TokenType.IsOperator()) {
                    var currRoot = root;
                    while (true) {
                        // become root if the current root is not operator
                        if (!currRoot.Value.TokenType.IsOperator()) {
                            var node = new BinaryTree<Token>(token);
                            node.Left = currRoot;
                            if (currRoot.Parent != null) {
                                node.Parent = currRoot.Parent;
                                currRoot.Parent.Right = node;
                            }
                            else
                                root = node;
                            currRoot.Parent = node;
                            break;
                        }
                        
                        // if curr < root => curr is root, root is curr.Left
                        if (token <= currRoot.Value) {
                            var node = new BinaryTree<Token>(token) {Parent = currRoot.Parent};
                            if (currRoot.Parent != null)
                                currRoot.Parent.Right = node;
                            else 
                                root = node;
                            currRoot.Parent = node;
                            node.Left = currRoot;
                            break;
                        }

                        if (token > currRoot.Value) {
                            // Set as right if possible
                            if (currRoot.Right == null) {
                                var node = new BinaryTree<Token>(token) {Parent = currRoot};
                                currRoot.Right = node;
                                break;
                            }

                            currRoot = currRoot.Right;
                        }
                    }
                } else {
                    var right = GetDeepestEmptyRight(root);
                    var leaf = new BinaryTree<Token>(token) {Parent = right};
                    right.Right = leaf;
                }
                /*if (token >= right.Value) {
                    var newAST = new BinaryTree<Token>(token);
                    if (right.Parent != null) {
                        right.Parent.Right = newAST;
                    }
                    newAST.Parent = right.Parent;
                    right.Parent = newAST;
                    newAST.Left = right;
                    if(right == AST)
                        AST = newAST;
                } else {
                    
                }*/
            }

            return root;
        }

        private BinaryTree<Token> GetDeepestEmptyRight(BinaryTree<Token> ast) {
            var curr = ast;
            while (curr.Right != null) curr = curr.Right;
            return curr;
        }

        public double EvaluateAST(BinaryTree<Token> ast) {
            switch (ast.Value.TokenType) {
                case TokenType.Add:
                    return EvaluateAST(ast.Left) + EvaluateAST(ast.Right);
                case TokenType.Subtract:
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