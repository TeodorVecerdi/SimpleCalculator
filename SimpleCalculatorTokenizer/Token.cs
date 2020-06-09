using System;

namespace calc {
    public readonly struct Token : IEquatable<Token>, IComparable<Token> {
        public readonly TokenType TokenType;
        public readonly dynamic TokenValue;

        public Token(TokenType tokenType, dynamic tokenValue) {
            TokenType = tokenType;
            TokenValue = tokenValue;
        }

        public bool Equals(Token other) {
            return TokenType == other.TokenType && TokenValue == other.TokenValue;
        }
        
        public override bool Equals(object obj) {
            return obj is Token other && Equals(other);
        }

        public override int GetHashCode() {
            return HashCode.Combine((int)TokenType, TokenValue);
        }


        public int CompareTo(Token other) {
            if (TokenType.Value() < other.TokenType.Value()) 
                return -1;
            if (TokenType.Value() > other.TokenType.Value()) 
                return 1;
            // if (TokenType == TokenType.Integer)
            //     return ((int)TokenValue).CompareTo((int)other.TokenValue);
            return 0;
        }

        public static bool operator ==(Token lhs, Token rhs) {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Token lhs, Token rhs) {
            return !(lhs == rhs);
        }

        public static bool operator <(Token lhs, Token rhs) {
            return lhs.CompareTo(rhs) == -1;
        }

        public static bool operator >(Token lhs, Token rhs) {
            return lhs.CompareTo(rhs) == 1;
        }

        public static bool operator <=(Token lhs, Token rhs) {
            return lhs.CompareTo(rhs) <= 0;

        }

        public static bool operator >=(Token lhs, Token rhs) {
            return lhs.CompareTo(rhs) >= 0;
        }

        public override string ToString() {
            return $"{TokenType}({TokenValue ?? ""})";
        }
    }
}