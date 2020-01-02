using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace codesnippets.tests
{
    [TestClass]
    public class UtilsTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            int a = 1, b = 1;
            int c = 2, d = 3;
            Assert.IsTrue(a.Equals(b));
            Assert.IsFalse(c.Equals(d));
        }

        [TestMethod]
        public void TestBinarySearch()
        {
            int[] arr;
            int index;

            arr = new int[] { 1 };
            index = Utils.BinarySearch(arr, 0, 0, 1);
            Assert.AreEqual(index, -1);
            index = Utils.BinarySearch(arr, 1, 0, 1);
            Assert.AreEqual(index, 0);
            arr = new int[] { 1, 2, 3 };
            index = Utils.BinarySearch(arr, 4, 0, 2);
            Assert.AreEqual(index, -1);
            index = Utils.BinarySearch(arr, 3, 0, 2);
            Assert.AreEqual(index, 2);
        }

        [TestMethod]
        public void SortWithHeap()
        {
            IEnumerable<int> values = Utils.GenerateRandomIntegers(2047, 30, 7);

            int[] arr1 = values.ToArray();
            Utils.SortWithMaxHeap(arr1);

        }

        [TestMethod]
        public void TestFindDuplicateNumber()
        {
            int[] arr = new int[] { 4, 1, 2, 3, 1 };
            int result;
            bool found = Utils.FindDuplicateNumber(arr, out result);
            Assert.IsTrue(found);
            Assert.AreEqual(result, 1);

            arr = new int[] { 5, 4, 1, 2, 3, 2 };
            found = Utils.FindDuplicateNumber(arr, out result);
            Assert.IsTrue(found);
            Assert.AreEqual(result, 2);

            arr = Utils.GenerateSequentialIntegers(1000, 1, 1).ToArray();
            arr[100] = 256;
            arr[900] = 256;

            found = Utils.FindDuplicateNumber(arr, out result);
            Assert.IsTrue(found);
            Assert.AreEqual(result, 256);

        }


        [TestMethod]
        public void TestFindDuplicateNumberOutOfRange()
        {
            int[] arr = new int[] { 1, 2 };
            int result;
            bool found = Utils.FindDuplicateNumber(arr, out result);

        }

        [TestMethod]
        public void TestGetCLSubstring()
        {
            var str1 = "abcabc0abcd0abc";
            var str2 = "bcd1abcd1bcd";

            string actual = Utils.GetCLSubstring(str1, str2);
            string expected = "abcd";
            Assert.AreEqual(expected, actual);

        }


        [TestMethod]
        public void TestGarbageCollection()
        {
            var obj = new Object();
            var expected = 0;
            var actual = GC.GetGeneration(obj);
            Assert.AreEqual(expected, actual);
            Utils.ForceGC();
            for (var i = 0; i < 3; i++)
            {
                expected = expected < 2 ? expected + 1 : 2;
                actual = GC.GetGeneration(obj);
                Assert.AreEqual(expected, actual);
                Utils.ForceGC();
            }

        }


        [TestMethod]
        public void TestRegexMatch()
        {
            Regex r = new Regex(@"^[0-9]+$");
            Assert.IsTrue(r.IsMatch("012345"));
            Assert.IsFalse(r.IsMatch("0123abc45"));

            r = new Regex(@"[A-Za-z]+");
            var matches1 = r.Matches("0123abc45def");
            Assert.AreEqual("abc", matches1[0].Value);
            Assert.AreEqual("def", matches1[1].Value);

            r = new Regex(@"([A-Za-z]*)([^A-Za-z]+[A-Za-z]+)");
            var matches2 = r.Matches("0123abc45def");
            Assert.AreEqual("", matches2[0].Groups[1].Value);
            Assert.AreEqual("0123abc", matches2[0].Groups[2].Value);

            r = new Regex(@"abc(?=def)");
            Assert.IsTrue(r.IsMatch("abcdefg"));
            Assert.IsFalse(r.IsMatch("abcdeg"));
        }

        [TestMethod]
        public void TestRegexReplace()
        {
            Regex r;
            
            r = new Regex(@"([0-9]+)([A-Za-z]+)");

            string actual = r.Replace("begin_1a1b_end", "$2$1_$`$&$'");
            string expected = "begin_a1_begin_1a1b_endb1_begin_1a1b_end_end";
            Assert.AreEqual(expected, actual);
        }

        volatile bool running = true;

        [TestMethod]
        public void TestTask()
        {
            var task1 = Task.Factory.StartNew(() => {
                SpinWait spinWait = new SpinWait();
        
                while (true)
                {
                    if (!running)
                        break;

                    spinWait.SpinOnce();
                }

                Trace.WriteLine(string.Format("spin count: {0}", spinWait.Count));
            });

            var task2 = Task.Run(() => {
                Thread.Sleep(10 * 1000);
                running = false;
            });

            Task.WaitAll(task1, task2);
        }

        [TestMethod]
        public void TestQsort()
        {
            int[] arr;
            arr = new int[]{ 3, 2, 4 };
            Utils.qsort(arr, 0, arr.Length - 1);
            Assert.IsTrue(Utils.IsSorted(arr));
            arr = Utils.GenerateRandomIntegers(1000).ToArray();
            Utils.qsort(arr, 0, arr.Length - 1);
            Assert.IsTrue(Utils.IsSorted(arr));
        }

        [TestMethod]
        public void TestColumnNameConversion()
        {
            int actual;
            actual = Utils.ColumnStringToInt("AA");
            Assert.AreEqual(27, actual);
            Assert.AreEqual("AA", Utils.ColumnIntToString(actual));

            actual = Utils.ColumnStringToInt("AAA");
            Assert.AreEqual(703, actual);
            Assert.AreEqual("AAA", Utils.ColumnIntToString(actual));

            Trace.WriteLine(Utils.ColumnIntToString(100));

        }

        [TestMethod]
        public void TestFindKthNumber()
        {
            int[] arr;
            int actual;
            /*
            arr = new int[]{ 3, 2, 1 };
            actual = Utils.GetKthNumber(arr, 0, arr.Length - 1, 3);
            Assert.AreEqual(1, actual);
            actual = Utils.GetKthNumberWithMinHeap(arr, 3);
            Assert.AreEqual(1, actual);
            actual = Utils.GetKthNumber(arr, 0, arr.Length - 1, 5);
            Assert.AreEqual(95, actual);
            */
            arr = Utils.GenerateRandomIntegers(100).ToArray();
            actual = Utils.GetKthNumberWithMinHeap(arr, 5);
            Assert.AreEqual(95, actual);
        }
        

        [TestMethod]
        public void TestStream()
        {
            string path = @"/Users/gangwan/stream_test.txt";
            using (FileStream stream = File.Open(path, FileMode.Create | FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine("Hello World");
                writer.Close();
            }
            using (FileStream stream = File.Open(path, FileMode.Create | FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine("Gang Wang");
                writer.Close();
            }

            string result = string.Empty;
            using (StreamReader reader = File.OpenText(path))
            {
                result = reader.ReadToEnd();
                reader.Close();
            }

            File.Delete(path);

            using (StringReader reader = new StringReader(result))
            {
                string line1 = reader.ReadLine();
                Assert.AreEqual("Hello World", line1);
                string line2 = reader.ReadLine();
                Assert.AreEqual("Gang Wang", line2);
            }
        }

        [TestMethod]
        public void TestFileInfo()
        {
            string path = @"/Users/gangwan/stream_test.txt";
            byte[] bytes = Encoding.UTF8.GetBytes("Hello World");
            File.WriteAllBytes(path, bytes);
            FileInfo fi = new FileInfo(path);
            Debug.WriteLine(fi.Name);
            Assert.AreEqual(bytes.Length, fi.Length);
        }
    }

}
