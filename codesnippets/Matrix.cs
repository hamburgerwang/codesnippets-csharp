using System;
using System.Text;
using System.Diagnostics;

namespace gang
{
    public class Matrix<T>
    {
        T[,] val;

        public int Rows {
            get{
                return val.GetLength(0);
            }
        }

        public int Columns {
            get{
                return val.GetLength(1);
            }
        }

        public Matrix(T[][] rows)
        {
            Debug.Assert(rows.Length > 0);
            Debug.Assert(rows[0].Length > 0);
            val = new T[rows.Length, rows[0].Length];
            for(var i = 0; i < rows.Length; i++)
            {
                for (var j = 0; j < rows[i].Length; j++)
                {
                    val[i, j] = rows[i][j];
                }
            }
        }

        public Matrix(T[,] arr)
        {
            Debug.Assert(arr != null);
            val = arr;
        }


        public override string ToString() 
        {
            StringBuilder sb = new StringBuilder();
            for (var i = 0; i < val.GetLength(0); i++)
            {
                if (i > 0)
                    sb.Append(", ");
                sb.Append("[");
                for (var j = 0; j < val.GetLength(1); j++)
                {
                    if (j > 0)
                        sb.Append(", ");
                    sb.Append(val[i, j]);
                }
                sb.Append("]");
            }
            return sb.ToString();
        }

        public Matrix<T> Multiply(Matrix<T> other, Func<T, T, T> multiplier, Func<T, T, T> adder)
        {
            System.Diagnostics.Debug.Assert(this.Columns == other.Rows);
            var result = new T[this.Rows, other.Columns];

            for (var i = 0; i < result.GetLength(0); i++)
            {
                for (var j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = MultiplyVectors(this.val, i, other.val, j, multiplier, adder, default(T)!);
                }
            }

            return new Matrix<T>(result);
        }

        private static T MultiplyVectors(T[,] a, int row, T[,] b, int column, Func<T, T, T> multiplier, Func<T, T, T> adder, T init)
        {
            T sum = init;
            for (var i = 0; i < a.Length; i++)
            {
                sum = adder(sum, multiplier(a[row, i], b[i, column]));
            }
            return sum;
        }

        public T[,] Value
        {
            get 
            {
                return val;
            }
        }
    }

    public static class MatrixExtensions
    {
        public static Matrix<int> Multiply(this Matrix<int> one, Matrix<int> other)
        {
            return one.Multiply(other, (v1, v2) => v1 * v2, (v1, v2) => v1 + v2);
        }
    }

}