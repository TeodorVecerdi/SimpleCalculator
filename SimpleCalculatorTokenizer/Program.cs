using System.Linq;

namespace calc {
    public static class Program {
        public static void Main(string[] args) {
            var tokenizer = new Tokenizer("12 + 55 - 311 * 31 / 3");
            tokenizer.Parse();
            if (tokenizer.IsValid()) {
                Debug.Log("Tokens:");
                foreach (var token in tokenizer.Tokens) {
                    Debug.Log($"    {token}");
                }

                var tree = tokenizer.BuildAST();
                tree.Traverse((token, depth) => {
                    Debug.Log($"{string.Concat(Enumerable.Repeat("  ", depth))}{token}");
                });
            } else {
                Debug.Log($"Tokenizer input is invalid. {tokenizer.Input}");
            }
        }
    }
}