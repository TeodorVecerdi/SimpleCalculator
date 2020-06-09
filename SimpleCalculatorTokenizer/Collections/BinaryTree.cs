using System;

namespace calc {
    public class BinaryTree<T> {
        public readonly T Value;
        public BinaryTree<T> Parent;
        public BinaryTree<T> Left;
        public BinaryTree<T> Right;

        public BinaryTree<T> this[int i] {
            get {
                if (i == 0) return Left;
                if (i == 1) return Right;
                return null;
            }
        }

        public BinaryTree(T value) {
            Value = value;
        }

        public void Traverse(Action<T, int> action, int depth = 0) {
            action(Value, depth);
            Left?.Traverse(action, depth+1);
            Right?.Traverse(action, depth+1);
        }
    }
}