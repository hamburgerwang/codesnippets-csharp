using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using gang;

namespace codesnippets.tests
{
    [TestClass]
    public class BinarySearchTreeTest
    {
        [DataTestMethod]
        [DataRow(new int[]{})]
        [DataRow(new int[]{1})]
        [DataRow(new int[]{1, 2, 3, 4})]
        public void TestBuild(int[] arr)
        {
            var tree = new BinarySearchTree<int>(arr);
            var node = tree.Root;
            while (node != null)
            {
                if (node.RightChild != null)
                {
                    Assert.IsTrue(node.Value < node.LeftChild.Value);
                }
            }
        }

        [TestMethod]
        public void TestIsBalanced()
        {
            var tree = new AVLTree<int>(new int[]{1, 2, 3, 4});
            
            Assert.IsTrue(tree.IsBalanced);

                tree.DeleteNode(1);
                Assert.IsTrue(tree.IsBalanced);
                Assert.AreEqual(tree.Root.Value, 3);
                tree.DeleteNode(2);
                Assert.IsTrue(tree.IsBalanced);
                Assert.AreEqual(tree.Root.Value, 3);
                tree.DeleteNode(3);
                Assert.IsTrue(tree.IsBalanced);
                Assert.AreEqual(tree.Root.Value, 4);
                tree.DeleteNode(4);
                Assert.IsTrue(tree.IsBalanced);
                Assert.IsNull(tree.Root);
        }


        [TestMethod]
        public void TestFindMinMax()
        {
            int[] arr = { 1, 2, 3, 4, 5, 6, 7 };
            var pair = Utils.FindMaxAndMin(arr);
            Assert.AreEqual(pair.Item1, 1);
            Assert.AreEqual(pair.Item2, 7);
        }
    }

}