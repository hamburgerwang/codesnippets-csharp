using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

using gang;

public class TreeNodeWithWeight<T> : IComparable<TreeNodeWithWeight<T>> where T : struct
{
    public int Weight { get; set; }

    public T? Value { get; set; }
    public TreeNodeWithWeight<T>? LeftChild { get; set; }

    public TreeNodeWithWeight<T>? RightChild { get; set; }

    public int CompareTo(TreeNodeWithWeight<T> another) 
    {
        return this.Weight.CompareTo(another.Weight);
    }
}

public static class Functions
{
    public static List<List<int>> JumpWithAllowedSteps(int total, int[] allowed)
    {
        Dictionary<int, List<List<int>>> dp = new Dictionary<int, List<List<int>>>();
        
        dp.Add(0, new List<List<int>>());
        dp[0].Add(new List<int>());
        for (int i = 1; i <= total; i++)
        {
            List<List<int>> result = new List<List<int>>();

            foreach (int step in allowed)
            {
                if (i >= step && dp.ContainsKey(i - step))
                {
                    foreach(List<int> existingSteps in dp[i - step])
                    {
                        var newList = existingSteps.ToList();
                        newList.Add(step);
                        result.Add(newList);
                    }
                }
            }
            dp.Add(i, result);
        }

        return dp[total];
    }

    public static void DrawCPUUsage()
    {
        while(true)
        {
            long i = 100000000;
            while(i-- > 0);

            System.Threading.Thread.Sleep(20);

        }
    }

    public static void GetChessPosition()
    {
        int i = 0;
        while (i < 81)
        {
            if (i / 9 % 3 != i % 9 % 3)
            {
                System.Console.WriteLine("A: {0}, B: {1}", (i / 9) + 1, (i % 9) + 1);
            }

            i++;
        }
    }

    public static void ReversePanCake()
    {
        int cakeCount = 100;
        int[] sorted = Utils.GenerateRandomIntegers(cakeCount, 1).ToArray();

        int times = 0;
        DoReverse(sorted, 0, ref times, (arr, startIndex) => 
        {
            int indexOfMax = startIndex;

            for (int i = startIndex + 1; i < arr.Length; i++)
            {
                if (arr[i] > arr[indexOfMax])
                {
                    indexOfMax = i;
                }
            }
            return indexOfMax;
        });

        Console.WriteLine("Times: {0}", times);
            
    }

    public static void DoReverse(int[] sorted, int startIndex, ref int times, Func<int[], int, int> indexOfNextElementFunc)
    {
        if (startIndex >= sorted.Length - 1)
            return;

        int index = indexOfNextElementFunc(sorted, startIndex);

        Array.Reverse(sorted, index, sorted.Length - index);
        Array.Reverse(sorted, startIndex, sorted.Length - startIndex);
        times += 2;
        DoReverse(sorted, startIndex + 1, ref times, indexOfNextElementFunc);
    }

    public static void FindLostOneBackup()
    {
        int[] arr = new int[9] {1,2,3,4,5,1,2,3,4};
        int parity = 0;

        foreach(int num in arr)
        {
            parity ^= num;
        }
        Console.WriteLine("Result: {0}", parity);
    }

    public static void FindLostTwoBackups()
    {
        int[] arr = new int[10] {1,2,3,4,5,6,1,2,3,4};
        int parity = 0;

        foreach(int num in arr)
        {
            parity ^= num;
        }

        if (parity > 0)
        {
            int flag = 1;
            while (flag > 0 && (parity & flag) == 0)
            {
                flag <<= 1;
            }

            if (flag > 0)
            {
                int result1 = 0;
                int result2 = 0;
                foreach(int num in arr)
                {
                    if ((flag & num) == 0)
                    {
                        result1 ^= num;
                    }
                    else
                    {
                        result2 ^= num;
                    }
                }

                Console.WriteLine("Result: {0} and {1}", result1, result2);
       }
            else
            {
                Console.WriteLine("Result: {0}", "Not found");
            }
        }

    }

    public static void TestSpinLock()
    {
        int times = 1000000;
        Queue<int> q = new Queue<int>(times);
        Action<int> updateWithSpinLock = (val) => {
            SpinLock spinLock = new SpinLock();
            bool lockTaken = false;
            try{
                spinLock.Enter(ref lockTaken);
                q.Enqueue(val);
            }
            finally{
                spinLock.Exit(false);
            }
        };

        Action<int> updateWithLock = (val) => {
            lock (q)
            {
                q.Enqueue(val);
            }
        };

        q.Clear();
        Stopwatch sw = new Stopwatch();
        sw.Start();
        System.Threading.Tasks.Parallel.Invoke(
            () => {
                for (int i = 0; i < times; i++)
                    updateWithSpinLock(2 * i);
                }, 
            () => {
                for (int i = 0; i < times; i++)
                    updateWithSpinLock(2 * i + 1);
                }
        );
        sw.Stop();

        Console.WriteLine("time taken with SpinLock: {0}", sw.ElapsedMilliseconds);

        q.Clear();
        sw.Start();
        System.Threading.Tasks.Parallel.Invoke(
            () => {
                for (int i = 0; i < times; i++)
                    updateWithLock(2 * i);
                }, 
            () => {
                for (int i = 0; i < times; i++)
                    updateWithLock(2 * i + 1);
                }
        );
        sw.Stop();

        Console.WriteLine("time taken with lock: {0}", sw.ElapsedMilliseconds);
    }

    public static void GetCLS()
    {
        int[] arr1 = new int[] { 1, 2, 3, 4, 5, 6, 7 };
        int[] arr2 = new int[] { 2, 5, 6, 8, 7 };

        int[,] dp = new int[arr1.Length + 1, arr2.Length + 1];
        for (int i = 0; i <= arr2.Length; i++)
        {
            dp[0, i] = 0;
        }

        for (int i = 1; i <= arr1.Length; i++)
        {
            dp[i, 0] = 0;
        }

        for (int i = 0; i < arr1.Length; i++)
        {
            for (int j = 0; j < arr2.Length; j++)
            {
                if (arr1[i] == arr2[j])
                {
                    dp[i + 1, j + 1] = dp[i, j] + 1;
                }
                else
                {
                    if (dp[i, j+1] < dp[i + 1, j])
                        dp[i + 1, j + 1] = dp[i + 1, j];
                    else
                        dp[i + 1, j + 1] = dp[i, j + 1];
                }
            }
        }

        Console.WriteLine("Common subsequence length: {0}", dp[arr1.Length, arr2.Length]);

        Stack<int> values = new Stack<int>();
        for (int i = arr1.Length, j = arr2.Length; i > 0 & j > 0; )
        {
            if (dp[i, j] == dp[i - 1, j])
            {
                i--;
            }
            else if (dp[i, j] == dp[i, j - 1])
            {
                j--;
            }
            else if (dp[i, j] == dp[i - 1, j - 1] + 1)
            {
                values.Push(arr1[i - 1]);
                i--;
                j--;
            }
            else 
            {
                Console.WriteLine("error happened");
            }

        }
        values.ToList().ForEach((val) => Console.Write("{0},", val));
    }



    public static void CreateHuffmanTree()
    {
        var frequencies = new Dictionary<char, int>();

        frequencies.Add('E', 100);
        frequencies.Add('A', 50);
        frequencies.Add('O', 25);
        frequencies.Add('I', 25);

        List<TreeNodeWithWeight<char>> forest = new List<TreeNodeWithWeight<char>>();
        
        foreach (var pair in frequencies)
        {
            TreeNodeWithWeight<char> node = new TreeNodeWithWeight<char>();
            node.Weight = pair.Value;
            node.Value = pair.Key;
            node.LeftChild = null;
            node.RightChild = null;
            forest.Add(node);
        }

        while (forest.Count > 1)
        {
            TreeNodeWithWeight<char> node1 = forest.Min();
            forest.Remove(node1);

            TreeNodeWithWeight<char> node2 = forest.Min();
            forest.Remove(node2);


            TreeNodeWithWeight<char> node = new TreeNodeWithWeight<char>();
            node.LeftChild = node1;
            node.RightChild = node2;
            node.Weight = node1.Weight + node2.Weight;
            forest.Add(node);
        }

        Stack<TreeNodeWithWeight<char>> stack = new Stack<TreeNodeWithWeight<char>>();

        StringBuilder prefix = new StringBuilder();
        Dictionary<char, string> results = new Dictionary<char, string>();
    }

    public static void CreateAVLTree()
    {
        var tree = new AVLTree<int>(Utils.GenerateSequentialIntegers(16));
        Action<int?[,]> printMatrix = (matrix) => {
            for(var i = 0; i < matrix.GetLength(0); i++)
            {
                for(var j = 0; j < matrix.GetLength(1); j++)
                {
                    int? val = matrix[i, j];
                    Console.Write("{0,2:#0}", val);
                }
                Console.WriteLine();
            }
        };
        Console.WriteLine("Is Balanced {0}", tree.IsBalanced);
        printMatrix(tree.ToMatrix());

/*
        for (int i = 5; i < 20; i++)
        {
            bool found = tree.DeleteNode(i);
            Console.WriteLine("{0} {1}found. Is Balanced: {2}", i, found ? string.Empty : "not ", tree.IsBalanced);
            printMatrix(tree.ToMatrix());
        }

*/

        Console.WriteLine();
        int lastDepth = 0;
        tree.TraverseBreadthFirstSearch((node, depth) => {
            if (lastDepth != depth)
            {
                Console.WriteLine();
                lastDepth = depth;
            }
            Console.Write("{0:}{1,6}\t", new string(' ', 0 * depth), node.Value);
        });

        Console.WriteLine();
        tree.TraversePreOrder((node, depth) => Console.Write("{0:}{1,6}\t", new string(' ', 0 * depth), node.Value));
        Console.WriteLine();
        tree.TraversePreOrderWithoutRecursion((node, depth) => Console.Write("{0:}{1,6}\t", new string(' ', 0 * depth), node.Value));
    }


    public static void QuickFindMissingElements()
    {
        int[] arr = Utils.GenerateRandomIntegers(1000, 0).ToArray();
        int[] expected = new int[2];
        expected[0] = arr[137];
        arr[137] = 2;
        expected[1] = arr[999];
        arr[999] = 2;

        for (int i = 0; i < arr.Length; i++)
        {
            int temp = arr[i];
            while (temp >= 0)
            {
                int next = arr[temp];
                arr[temp] = -1 * temp;
                temp = next;
            }
        }

        List<int> result = new List<int>();
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] > 0)
                result.Add(i);
        }
        Console.WriteLine(result.ToArray());
    }

    public static void TestSwap()
    {
        int[] arr =  new int[] { 1, 2, 3 };
        Utils.Swap(ref arr[0], ref arr[2]);
        Array.ForEach(arr, Console.WriteLine);
    }


}