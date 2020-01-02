
using System.Diagnostics;
using gang;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class MatrixTest
{
    [TestMethod]
    public void TestToString()
    {
        char[,] arr = { { '1' } };
        Matrix<char> charMatrix = new Matrix<char>(arr);
        string literal = charMatrix.ToString();
        Debug.WriteLine(literal);
    }

    [TestMethod]
    public void TestMultiply()
    {
        int[,] arr1 = { { 1, 2 } };
        int[,] arr2 = { { 3 }, { 4 } };
        var actual = new Matrix<int>(arr1).Multiply(new Matrix<int>(arr2));
        Debug.WriteLine(actual.ToString());

    }
}