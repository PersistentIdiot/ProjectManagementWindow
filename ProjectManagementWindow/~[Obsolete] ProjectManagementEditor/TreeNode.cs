using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProjectManagementWindow.Core.Editor._Obsolete__ProjectManagementEditor {
    // From https://stackoverflow.com/questions/66893/tree-data-structure-in-c-sharp
    public class TreeNode<TValue> {
        private readonly TValue _value;
        private readonly List<TreeNode<TValue>> _children = new List<TreeNode<TValue>>();

        public TreeNode(TValue value) { _value = value; }

        public TreeNode<TValue> this[int i] { get { return _children[i]; } }

        public TreeNode<TValue> Parent { get; private set; }

        public TValue Value { get { return _value; } }

        public ReadOnlyCollection<TreeNode<TValue>> Children { get { return _children.AsReadOnly(); } }

        public TreeNode<TValue> AddChild(TValue value) {
            var node = new TreeNode<TValue>(value) {Parent = this};
            _children.Add(node);
            return node;
        }

        public TreeNode<TValue>[] AddChildren(params TValue[] values) { return values.Select(AddChild).ToArray(); }

        public bool RemoveChild(TreeNode<TValue> node) { return _children.Remove(node); }

        public void Traverse(Action<TValue> headAction) {
            headAction(Value);
            foreach (var child in _children)
                child.Traverse(headAction);
        }

        public void Traverse(Action<TValue> headAction, Action<TreeNode<TValue>> childAction, bool depthFirst = true) {
            headAction(Value);
            foreach (var child in _children) {
                if (depthFirst) { childAction?.Invoke(child); }
                child.Traverse(headAction);
                if (!depthFirst) { childAction?.Invoke(child); }
            }
        }

        public IEnumerable<TValue> Flatten() { return new[] {Value}.Concat(_children.SelectMany(x => x.Flatten())); }
    }
}