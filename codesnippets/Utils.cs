using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public static class Utils
{

    public static IEnumerable<int> GenerateSequentialIntegers(int length, int start = 0, int step = 1)
    {
        var list = new List<int>(length);
        for (int i = 0; i < length; i++)
        {
            list.Add(start + i * step);
        }

        return list;
    }

    public static IEnumerable<int> GenerateRandomIntegers(int length, int start = 0, int step = 1)
    {
        var list = new List<int>(length);
        var rand = new Random();
        for (int i = 0; i < length; i++)
        {
            list.Add(start + i * step);
        }

        for (int i = 0; i < length; i++)
        {
            int index = rand.Next(list.Count);
            yield return list.ElementAt(index);
            list.RemoveAt(index);
        }
    }
    public static void BuildMaxHeap<ValueType>(ValueType[] arr, int length = -1) where ValueType : IComparable
    {
        if (length < 0)
            length = arr.Length;

        for (var i = length / 2 - 1; i >= 0; i--)
        {
            int leftChild = 2 * i + 1;
            if (arr[i].CompareTo(arr[leftChild]) < 0)
            {
                Swap(ref arr[i], ref arr[leftChild]);

                int leftChildOfleftChild = 2 * leftChild + 1;
                int rightChildOfleftChild = 2 * leftChild + 2;

                if (leftChildOfleftChild < length && arr[leftChild].CompareTo(arr[leftChildOfleftChild]) < 0 || rightChildOfleftChild < length && arr[leftChild].CompareTo(arr[rightChildOfleftChild]) < 0)
                {
                    BuildMaxHeap(arr, length);
                }
            }

            int rightChild = 2 * i + 2;
            if (rightChild < length && arr[i].CompareTo(arr[rightChild]) < 0)
            {
                Swap(ref arr[i], ref arr[rightChild]);

                int leftChildOfRightChild = 2 * rightChild + 1;
                int rightChildOfRightChild = 2 * rightChild + 2;

                if (leftChildOfRightChild < length && arr[rightChild].CompareTo(arr[leftChildOfRightChild]) < 0 || rightChildOfRightChild < length && arr[rightChild].CompareTo(arr[rightChildOfRightChild]) < 0)
                {
                    BuildMaxHeap(arr, length);
                }
            }
        }
    }

    public static void SortWithMaxHeap<ValueType>(ValueType[] values, int length = -1) where ValueType : IComparable
    {
        if (length < 0)
            length = values.Length;

        if (length == 0)
            return;

        BuildMaxHeap(values, length);
        Swap(ref values[0], ref values[length - 1]);
        SortWithMaxHeap(values, length - 1);
    }

    public static void Swap<ValueType>(ref ValueType left, ref ValueType right)
    {
        ValueType tmp = left;
        left = right;
        right = tmp;
    }

    public static void QuickSort<ValueType>(ValueType[] arr, int startIndex = 0, int endIndex = int.MaxValue) where ValueType : IComparable
    {
        if (endIndex >= int.MaxValue)
            endIndex = arr.Length - 1;

        if (startIndex < endIndex)
        {
            int p = QuickSort_PartitionHigh(arr, startIndex, endIndex);
            QuickSort(arr, startIndex, p - 1);
            QuickSort(arr, p + 1, endIndex);
        }
    }

    private static int QuickSort_PartitionHigh<ValueType>(ValueType[] arr, int startIndex, int endIndex) where ValueType : IComparable
    {
        ValueType pivot = arr[endIndex];

        var i = startIndex - 1;

        for (int j = startIndex; j < endIndex; j++)
        {
            if (arr[j].CompareTo(pivot) < 0)
            {
                i++;
                Swap(ref arr[i], ref arr[j]);
            }
        }
        i++;

        Swap(ref arr[i], ref arr[endIndex]);
        return i;
    }



    private static int QuickSort_PartitionLow<ValueType>(ValueType[] arr, int startIndex, int endIndex) where ValueType : IComparable
    {
        ValueType pivot = arr[startIndex];

        var i = startIndex;
        var j = endIndex;

        while (i < j)
        {
            while (i < j && arr[j].CompareTo(pivot) > 0)
                j--;

            if (i < j)
            {
                arr[i] = arr[j];
            }
                
            while (i < j && arr[i].CompareTo(pivot) < 0)
                i++;

            if (i < j)
            {
                arr[j] = arr[i];
            }
        }

        arr[i] = pivot;

        return i;
    }


    public static int BinarySearch<ValueType>(ValueType[] values, ValueType target, int left, int right) where ValueType : IComparable
    {
        if (right < left)
            return -1;

        int mid = left + (right - left) / 2;

        if (target.CompareTo(values[mid]) == 0)
            return mid;
        else if (target.CompareTo(values[mid]) < 0)
            return BinarySearch(values, target, left, mid - 1);
        else 
            return BinarySearch(values, target, mid + 1, right);
    }


    /// <summary>
    /// Find duplicate value in an array from 1 to n
    /// </summary>
    /// <param name="arr"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool FindDuplicateNumber(int[] arr, out int result)
    {
        if (arr.Length == 0)
        {
            result = int.MaxValue;
            return false;
        }

        if (arr.Min() <= 0 || arr.Max() >= arr.Length)
            throw new ArgumentOutOfRangeException(string.Format("The array values should be in range [1, {0}", arr.Length - 1));
            
        var slow = arr[0];
        var fast = arr[0];

        slow = arr[slow];
        fast = arr[arr[fast]];
        while (slow != fast)
        {
            slow = arr[slow];
            fast = arr[arr[fast]];
        }

        fast = arr[0];
        while (slow != fast)
        {
            slow = arr[slow];
            fast = arr[fast];
        }
        result = slow;
        return true;

    }

    public static Tuple<int, int>FindMaxAndMin(int[] arr)
    {
        int min = int.MaxValue;
        int max = int.MinValue;
        int i;

        for (i = 0; i < arr.Length  - 1; i += 2)
        {
            if (arr[i] < arr[i + 1])
            {
                if (arr[i] < min)
                    min = arr[i];
                
                if (arr[i + 1] > max)
                    max = arr[i + 1];
            }
            else
            {
                if (arr[i + 1] < min)
                    min = arr[i + 1];
                
                if (arr[i] > max)
                    max = arr[i];
            }

        }

        if (i == arr.Length - 1)
        {
            if (arr[i] > max)
                max = arr[i];
            else if (arr[i] < min)
                min = arr[i];
        }

        return new Tuple<int, int>(min, max);
    }


    public static string GetCLSubstring(string str1, string str2)
    {
        int max = 0;
        string result = string.Empty;

        int[,] dp = new int[str1.Length + 1, str2.Length + 1];
        for (var i = 0; i < str1.Length + 1; i++)
            dp[i, 0] = 0;

        for (var i = 1; i < str2.Length + 1; i++)
            dp[0, i] = 0;

        for (var i = 1; i < str1.Length + 1; i++)
        {
            for (var j = 1; j < str2.Length + 1; j++)
            {
                if (str1.ElementAt(i - 1) == str2.ElementAt(j - 1))
                {
                    dp[i, j] = dp[i - 1, j - 1] + 1;
                    if (dp[i, j] > max)
                    {
                        max = dp[i, j];
                        result = str1.Substring(i - max, max);
                    }
                }
                else
                {
                    dp[i, j] = 0;
                }
            }
        }
        return result;
    }


    public static void ForceGC()
    {
        //test
        GC.Collect(2, GCCollectionMode.Forced);
        GC.WaitForPendingFinalizers();
    }

    public static bool IsSorted<T>(T[] values) where T : IComparable
    {
        if (values == null || values.Length <= 1)
            return true;

        for (var i = 0; i < values.Length - 1; i++)
        {
            if (values[i].CompareTo(values[i + 1]) > 0)
                return false;
        }
        return true;
    }

    public static void StartTcpServer(int port)
    {
        // using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        // {
        //     IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
        //     socket.Bind(endPoint);
        //     socket.Listen(1);
        //     while (true)
        //     {
        //         Socket client = socket.Accept();
        //         byte[] data = Encoding.UTF8.GetBytes("Please input send/recv:\n");
        //         client.BeginSend(data, 0, data.Length, SocketFlags.None, (asyncResult) =>{
        //             client.EndSend(asyncResult);
        //             byte[] receivedData = new byte[1024];
        //             client.BeginReceive(receivedData, 0, 1024, SocketFlags.None, (recvResult) => {
        //                 int bytes = client.EndReceive(recvResult);
        //                 if (bytes == 1024)
        //                     throw new InvalidOperationException("only 1024 bytes allowed");
        //             }, null);
        //         }, client);
        //     }
        // }
    }

    public static int ColumnStringToInt(string alphabeticIndex)
    {
        int baseNum = 1;
        int val = 0;
        for(var i = alphabeticIndex.Length - 1; i >= 0; i--)
        {
            val += (alphabeticIndex[i] - 'A' + 1) * baseNum;
            baseNum *= 26;
        }

        return val;
    }


    public static string ColumnIntToString(int numberIndex)
    {
        Stack<char> result = new Stack<char>();

        while (numberIndex > 0)
        {
            char val = (char)((numberIndex - 1) % 26 + 'A');
            numberIndex = (numberIndex - 1) / 26;
            result.Push(val);
        }
        return string.Concat(result);
    }

    public static void qsort(int[] array, int start, int end)
    {
        if (start >= end)
            return;

        var mid = start + (end - start) / 2;

        int index = start -1;
        int pivot = array[end];

        for (var i = start; i < end; i++)
        {
            if (array[i] < pivot)
            {
                index++;
                int tmp1 = array[index];
                array[index] = array[i];
                array[i] = tmp1;
            }
        }

        index++;
        int tmp = array[index];
        array[index] = array[end];
        array[end] = tmp;

        qsort(array, start, index - 1);
        qsort(array, index + 1, end);
    }


    public static int GetKthNumber(int[] arr, int begin, int end, int k)
    {
        if (begin == end && k == 1)
            return arr[begin];

        int pivot = arr[begin];
        int l = begin;
        int r = end;
        while (l < r)
        {
            while (l < r && arr[r] >= pivot) --r;

            arr[l] = arr[r];

            while (l < r && arr[l] <= pivot) ++l;

            arr[r] = arr[l];
        }
        arr[l] = pivot;

        var count = end - r + 1;
        if (count == k)
            return arr[l];
        else if(count < k)
            return GetKthNumber(arr, begin, l - 1, k - count);
        else 
            return GetKthNumber(arr, l + 1, end, k);
    }

    public static int GetKthNumberWithMinHeap(int[] arr, int k)
    {
        int[] minHeap = BuildMinHeap(arr.Take(k));
        for (int i = k; i < arr.Length; i++)
        {
            if (arr[i] > minHeap[0])
                SwapMinHeap(minHeap, arr[i]);
        }
        return minHeap[0];
    }


    public static int[] BuildMinHeap(IEnumerable<int> values)
    {
        int i = 0;
        int[] arr = new int[values.Count()];
        foreach (int val in values)
        {
            AddMinHeap(arr, val, i++);
        }
        return arr;
    }

    private static void AddMinHeap(int[] arr, int val, int index)
    {
        arr[index] = val;
        Func<int, int> getParent = (cur) => (cur - 1) / 2;
        for (int i = index, j = getParent(i); i > 0; i = j, j = getParent(i))
        {
            if (arr[i] < arr[j])
            {
                int tmp = arr[i];
                arr[i] = arr[j];
                arr[j] = tmp;
            }
        }
    }

    private static void SwapMinHeap(int[] arr, int val, int index = 0)
    {
        arr[index] = val;
        Func<int, int> getLChild = cur => cur * 2 + 1;
        for (int i = index, j = getLChild(i); i <= arr.Length / 2; )
        {
            if (j + 1 < arr.Length)
            {
                if (arr[i] > arr[j] || arr[i] > arr[j + 1])
                {
                    int minIndex = (arr[j] < arr[j + 1] ? j : j + 1);
                    int tmp = arr[minIndex];
                    arr[minIndex] = arr[i];
                    arr[i] = tmp;
                    i = minIndex;
                    j = getLChild(i);
                }
                else
                {
                    break;
                }
            }
            else if (j < arr.Length)
            {
                if (arr[i] > arr[j])
                {
                    int minIndex = j;
                    int tmp = arr[minIndex];
                    arr[minIndex] = arr[i];
                    arr[i] = tmp;
                    i = minIndex;
                    j = getLChild(i);
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }
    }

}