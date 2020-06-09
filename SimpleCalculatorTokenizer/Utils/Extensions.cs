using System;
using System.Linq;

namespace calc {
    public static class TokenTypeExtensions {
        public static bool EqualsAny(this TokenType tokenType, params TokenType[] tokenTypes) {
            return tokenTypes.Any(type => tokenType == type);
        }

        public static int Value(this TokenType tokenType) {
            switch (tokenType) {
                case TokenType.Integer:
                    return 0;
                case TokenType.Plus:
                case TokenType.Minus:
                    return 1;
                case TokenType.Multiply:
                case TokenType.Divide:
                    return 2;
                case TokenType.Power:
                    return 3;
                default:
                    return -1;
            }
        }

        public static bool IsOperator(this TokenType tokenType) {
            return tokenType != TokenType.Integer;
        }
    }
}