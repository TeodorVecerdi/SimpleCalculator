using System.Linq;

namespace calc {
    public static class Program {
        public static void Main(string[] args) {
            var tokenizer = new Tokenizer("12 ^ 2 + 55 - 311 * 31 / 3");
            tokenizer.Parse();
            if (!tokenizer.IsValid()) {
                Debug.Log($"Tokenizer input is invalid. {tokenizer.Input}");
                return;
            }
            
            var tree = tokenizer.BuildAST();
            tree.Traverse((token, depth) => { Debug.Log($"{string.Concat(Enumerable.Repeat("  ", depth))}{token}"); });
            var value = tokenizer.EvaluateAST(tree);
            // parentheses 
            Debug.LogInfo($"AST Evaluation Result: {value}");
        }
    }
}