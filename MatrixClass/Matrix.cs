using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MatrixClass
{
    internal class Matrix
    {
        int _x;
        int _y;
        int[,] _values; 
        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
        public int[,] Values { get => _values; set => _values = value; }


        public Matrix(int x, int y, Random generator)
        {
            X = x;
            Y = y;
            _values = new int[x, y];
            for(int i = 0; i < x; i++) 
                for(int j = 0; j < y; j++) Values[i, j] = generator.Next(10);
            
        }
        public Matrix(int x, int y)
        {
            X = x;
            Y = y;
            _values = new int[x, y];
        }
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(Matrix))
                return false;
            Matrix m = (Matrix)obj;
            if (m.X != this.X || m.Y != this.Y)
                return false;
            for (int i = 0; i < X; i++)
                for (int j = 0; j < Y; j++) if (m.Values[i, j] != this.Values[i, j]) return false;
            return true;
        }

        public static Matrix Multiply(Matrix A, Matrix B)
        {
            Matrix ans;
            if (A.Y != B.X) throw new Exception("Wrong dimensions");
            ans = new(A.X, B.Y);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < ans.X; ++i)
                for (int j = 0; j < ans.Y; ++j) ans.Values[i,j] = CountElement(A, B, i, j);
            Console.WriteLine($"Single thread finished in {watch.ElapsedMilliseconds} ms");
            return ans;
        }
        public static int CountElement(Matrix A, Matrix B, int x, int y)
        {
            int res = 0;
            for(int i = 0; i < A.Y; ++i)
                res += A.Values[x, i] * B.Values[i, y];
            return res;
        }
        public static Matrix MultiplyParallel(Matrix A, Matrix B, int thread_count)
        {
            int thread_howMany = (int)Math.Ceiling(((double)(A.X * B.Y)) / (double)thread_count);
            Matrix ans;
            if (A.Y != B.X) throw new Exception("Wrong dimensions");
            ParallelOptions opt = new ParallelOptions() { MaxDegreeOfParallelism = thread_count };
            ans = new(A.X, B.Y);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Parallel.For(0, A.X * B.Y, opt, x =>
            {
                ans.Values[x/B.Y, x%B.Y] = CountElement(A, B, x/B.Y, x%B.Y);
            });
            watch.Stop();
            Console.WriteLine($"{thread_count} Parallel finished in {watch.ElapsedMilliseconds} ms");

            return ans;
        }
        public static Matrix MultiplyThread(Matrix A, Matrix B, int thread_count)
        { 
            int thread_howMany = (int)Math.Ceiling(((double)(A.X * B.Y))/(double)thread_count);
            Matrix ans;
            List<Thread> thread = new();
            if (A.Y != B.X) throw new Exception("Wrong dimensions");
            ans = new(A.X, B.Y);
            for (int i = 0; i < thread_count; ++i)
            {
                int index = i;
                if((index * thread_howMany + thread_howMany) / ans.Y < ans.X)
                    thread.Add(new(() => CountElements(ans, A, B, index * thread_howMany, thread_howMany)));
                else
                {
                    int z = 0;
                    while ((index * thread_howMany + thread_howMany - z) / ans.Y >= ans.X)  ++z;
                    --z;
                    thread.Add(new(() => CountElements(ans, A, B, index * thread_howMany, thread_howMany - z)));

                }
            }
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < thread_count; ++i) thread[i].Start();
            for (int i = 0; i < thread_count; ++i) thread[i].Join();
            watch.Stop();
            Console.WriteLine($"{thread_count} threads finished in {watch.ElapsedMilliseconds} ms");

            return ans;
        }
        public static void CountElements(Matrix ans, Matrix A, Matrix B, int start, int elems)
        {
            for(int i = 0; i < elems; ++i)
            {
                ans.Values[start / ans.Y, start % ans.Y] = CountElement(A, B, start / ans.Y, start % ans.Y);
                start++;
            }
        }
        public override string ToString()
        {
            string ans = "";
            for (int i = 0; i < X; ++i)
            {
                ans += "[\t";
                for (int j = 0; j < Y; ++j)
                {
                    ans += $"{Values[i, j]}\t";
                } 
                ans += "]\n";

            }

            return ans;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
