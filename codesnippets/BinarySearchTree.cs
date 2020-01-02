using System;
using System.Collections.Generic;
using System.IO;

namespace gang
{
    public enum TraversalEnum
    {
        PreOrder = 0,
        InOrder,
        PostOrder
    }
public interface ITreeNode<NodeType, ValueType>
{
    NodeType LeftChild { get; }
    NodeType RightChild { get; }
    ValueType Value { get; }
}

public interface IBinaryTree<NodeType> where NodeType : class
{
    NodeType Root { get; }

    void TraverseInOrder(Action<NodeType, int> action);
    void TraversePreOrder(Action<NodeType, int> action);
    void TraversePostOrder(Action<NodeType, int> action);
}

#pragma warning disable CS8625,CS8613
public class BSTNode<ValueType> : ITreeNode<BSTNode<ValueType>, ValueType>, IComparable<BSTNode<ValueType>> where ValueType : IComparable
{
    public ValueType Value { get; internal set; }
    public BSTNode<ValueType>? LeftChild { get; set; }
    public BSTNode<ValueType>? RightChild { get; set; }

    public BSTNode(ValueType val)
    {
        this.Value = val;
        this.LeftChild = null;
        this.RightChild = null;
    }

    public override string ToString() => this.Value.ToString()!;

        public int CompareTo(BSTNode<ValueType> other)
        {
            return this.Value.CompareTo(other.Value);
        }
    }



public class BinarySearchTree<ValueType> : IBinaryTree<BSTNode<ValueType>> where ValueType : struct, IComparable
{

    public BSTNode<ValueType>? Root { get; protected set; }

    public BinarySearchTree(IEnumerable<ValueType> values)
    {
        this.Root = null;
        foreach (var val in values)
        {
            AddNode(val);
        }
    }

    public virtual void AddNode(ValueType val)
    {
        var node = new BSTNode<ValueType>(val);
        this.Root = AddNode(this.Root, node);
    }

    protected virtual BSTNode<ValueType>? AddNode(BSTNode<ValueType>? parent, BSTNode<ValueType> node)
    {
        if (parent == null)
            return node;

        if (node.CompareTo(parent) < 0)
        {
            parent.LeftChild = AddNode(parent.LeftChild, node);
        }
        else
        {
            parent.RightChild = AddNode(parent.RightChild, node);
        }
        
        return parent;
    }

    public virtual bool DeleteNode(ValueType val)
    {
        var node = new BSTNode<ValueType>(val);
        bool found;
        this.Root = DeleteNode(this.Root, node, out found);
        return found;
    }

    protected virtual BSTNode<ValueType>? DeleteNode(BSTNode<ValueType>? current, BSTNode<ValueType> val, out bool found)
    {
        if (current == null)
        {
            found = false;
            return null;
        }
        
        if (val.CompareTo(current) == 0)
        {
            found = true;
            if (current.LeftChild == null)
                return current.RightChild;

            if (current.RightChild == null)
                return current.LeftChild;

            var minRight = current.RightChild;
            while (minRight.LeftChild != null)
                minRight = minRight.LeftChild;

            bool successful;
            current.Value = minRight.Value;
            current.RightChild = DeleteNode(current.RightChild, minRight, out successful);

        }
        else if (val.CompareTo(current) < 0)
        {
            current.LeftChild = DeleteNode(current.LeftChild, val, out found);
        }
        else
        {
            current.RightChild = DeleteNode(current.RightChild, val, out found);
        }

        return current;
    }

    protected BSTNode<ValueType> LeftRotate(BSTNode<ValueType> node)
    {
        BSTNode<ValueType> newRoot = node.RightChild!;
        BSTNode<ValueType>? oldRightLeft = newRoot.LeftChild;
        newRoot.LeftChild = node;
        newRoot.LeftChild.RightChild = oldRightLeft;
        Console.WriteLine("{0} rotated to be the left child of {1}, {2} moved to the right child of {0}", newRoot.LeftChild, newRoot, newRoot.LeftChild.RightChild);

        return newRoot;
    }

    protected BSTNode<ValueType> RLRotate(BSTNode<ValueType> node)
    {
        var newRoot = node.LeftChild!.RightChild!;
        var oldLRLeft = newRoot.LeftChild;
        var oldLRRight = newRoot.RightChild;
        newRoot.LeftChild = node.LeftChild;
        newRoot.RightChild = node;
        newRoot.LeftChild.RightChild = oldLRLeft;
        newRoot.RightChild.LeftChild = oldLRRight;
        return newRoot;
    }

    protected BSTNode<ValueType> RightRotate(BSTNode<ValueType> node)
    {
        BSTNode<ValueType> newRoot = node.LeftChild!;
        BSTNode<ValueType>? oldLeftRight = newRoot.RightChild;
        newRoot.RightChild = node;
        newRoot.RightChild.LeftChild = oldLeftRight;
        Console.WriteLine("{0} rotated to be the right child of {1}, {2} moved to the left child of {0}", newRoot.RightChild, newRoot, newRoot.RightChild.LeftChild);
        return newRoot;
    }


    protected int GetHeight(BSTNode<ValueType>? node)
    {
        if (node == null)
            return 0;

        int leftHeight = GetHeight(node.LeftChild);
        int rightHeight = GetHeight(node.RightChild);

        return Math.Max(leftHeight, rightHeight) + 1;
    }

    public void TraverseInOrder(Action<BSTNode<ValueType>, int> action)
    {
        TraverseRecursively(this.Root, action, TraversalEnum.InOrder, 0);
    }

    protected void TraverseRecursively(BSTNode<ValueType>? node, Action<BSTNode<ValueType>, int> action, TraversalEnum order, int depth)
    {
        if (node == null)
            return;

        if (order == TraversalEnum.PreOrder)
            action(node, depth);

        TraverseRecursively(node.LeftChild, action, order, depth + 1);

        if (order == TraversalEnum.InOrder)
            action(node, depth);

        TraverseRecursively(node.RightChild, action, order, depth + 1);

        if (order == TraversalEnum.PostOrder)
            action(node, depth);
    }

    public void TraverseInOrderWithoutRecursion(Action<BSTNode<ValueType>, int> action)
    {
        if (this.Root == null)
            return;

        Stack<BSTNode<ValueType>> stack = new Stack<BSTNode<ValueType>>();

        BSTNode<ValueType>? current = this.Root;
        while (current != null || stack.Count > 0)
        {
            while (current != null)
            {
                stack.Push(current);
                current = current.LeftChild;
            }

            current = stack.Pop();
            action(current, 0);

            current = current.RightChild;
        }
        
    }


    public void TraversePostOrderWithoutRecursion(Action<BSTNode<ValueType>, int> action)
    {
        if (this.Root == null)
            return;

        Stack<BSTNode<ValueType>> stack = new Stack<BSTNode<ValueType>>();
        var result = new Stack<BSTNode<ValueType>>();

        var current = this.Root;
        while (stack.Count > 0)
        {
            current = stack.Pop();
            result.Push(current);

            if (current.LeftChild != null)
                stack.Push(current.LeftChild);

            if (current.RightChild != null)
                stack.Push(current.RightChild);
        }

        while (result.Count > 0)
        {
            action(result.Pop(), 0);
        }
        
    }
    public void TraversePreOrderWithoutRecursion(Action<BSTNode<ValueType>, int> action)
    {
        if (this.Root == null)
            return;

        Stack<BSTNode<ValueType>> stack = new Stack<BSTNode<ValueType>>();

        BSTNode<ValueType>? current = this.Root;
        while (current != null || stack.Count > 0)
        {
            while (current != null)
            {
                stack.Push(current);

                action(current, 0);

                current = current.LeftChild;
            }

            current = stack.Pop();

            current = current.RightChild;
        }
        
    }

    public void TraversePreOrder(Action<BSTNode<ValueType>, int> action)
    {
        TraverseRecursively(this.Root, action, TraversalEnum.PreOrder, 0);
    }

    public void TraversePostOrder(Action<BSTNode<ValueType>, int> action)
    {
        TraverseRecursively(this.Root, action, TraversalEnum.PostOrder, 0);
    }

    public void TraverseBreadthFirstSearch(Action<BSTNode<ValueType>, int> action)
    {
        if (this.Root == null)
            return;
        var queue = new Queue<BSTNode<ValueType>>();
        queue.Enqueue(this.Root);
        int depth = 0;
        while (queue.Count > 0)
        {
            int count = queue.Count;
            for (var i = 0; i < count; i++)
            {
                var node = queue.Dequeue();
                action(node, depth);

                if (node.LeftChild != null)
                    queue.Enqueue(node.LeftChild);

                if (node.RightChild != null)
                    queue.Enqueue(node.RightChild);
            }
            depth++;
        }
    }

    public ValueType?[,] ToMatrix() {
        int height = GetHeight(this.Root);
        int width = (int)Math.Pow(2, height) - 1;
        ValueType?[,] result = new ValueType?[height, width];
        ToMatrix(this.Root, 0, 0, width - 1, result);
        return result;
    }
    
    private void ToMatrix(BSTNode<ValueType>? node, int rowIndex, int startPos, int endPos, ValueType?[,] result) {
        int height = result.GetLength(0);
        int width = result.GetLength(1);
        if (rowIndex >= height || node == null)
            return;
        
        int middle = startPos + (endPos - startPos) / 2;
        result[rowIndex, middle] = node.Value;
        
        ToMatrix(node.LeftChild, rowIndex + 1, startPos, middle - 1, result);
        ToMatrix(node.RightChild, rowIndex + 1, middle + 1, endPos, result);
    }

    public void Print(TextWriter output, string format = "{0,8:#0}") 
    {
        var matrix = this.ToMatrix();

        for(var i = 0; i < matrix.GetLength(0); i++)
        {
            for(var j = 0; j < matrix.GetLength(1); j++)
            {
                ValueType? val = matrix[i, j];
                output.Write(format, val);
            }
            output.WriteLine();
        }
    }
}

    public class AVLTree<ValueType> : BinarySearchTree<ValueType> where ValueType : struct, IComparable
    {
        public AVLTree(IEnumerable<ValueType> values) : base(values)
        {
        }

        public bool IsBalanced
        {
            get
            {
                return Math.Abs(GetBalance(this.Root)) < 2;
            }
        }

        private int GetBalance(BSTNode<ValueType>? node)
        {
            return node == null ? 0 : (GetHeight(node.LeftChild) - GetHeight(node.RightChild));
        }

        protected virtual BSTNode<ValueType>? Balance(BSTNode<ValueType>? parent)
        {
            if (parent != null)
            {
                int factor = GetBalance(parent);

                if (factor < -1)
                {
                    int factorChild = GetBalance(parent.RightChild);
                    if (factorChild > 0)
                    {
                        parent.RightChild = RightRotate(parent.RightChild!);
                    }
                    parent = LeftRotate(parent);
                }
                else if (factor > 1)
                {
                    int factorChild = GetBalance(parent.LeftChild);
                    if (factorChild < 0)
                    {
                        parent.LeftChild = LeftRotate(parent.LeftChild!);
                    }
                    parent = RightRotate(parent);
                }
            }
            return parent;
        }

        protected override BSTNode<ValueType>? AddNode(BSTNode<ValueType>? parent, BSTNode<ValueType> node)
        {
            return Balance(base.AddNode(parent, node));
        }
        
        protected override BSTNode<ValueType>? DeleteNode(BSTNode<ValueType>? current, BSTNode<ValueType> val, out bool found)
        {
            return Balance(base.DeleteNode(current, val, out found));
        }
    }
}

