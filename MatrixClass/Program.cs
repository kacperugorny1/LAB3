namespace MatrixClass
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int threads_count = 5;
            int seed = 0;
            Matrix matrix1 = new(400, 400, new(seed));
            Matrix matrix2 = new(400, 400, new(seed + 2));
            
            Matrix ans = Matrix.Multiply(matrix1, matrix2);

            //Console.WriteLine(matrix1);
            //Console.WriteLine(matrix2);
            //Console.WriteLine(Matrix.Multiply(matrix1, matrix2)); 
            //Console.WriteLine(Matrix.MultiplyThread(matrix1,matrix2,threads_count));


            Console.WriteLine($"Thread - {ans.Equals(Matrix.MultiplyThread(matrix1, matrix2, threads_count))}");

            Console.WriteLine($"Parallel - {ans.Equals(Matrix.MultiplyParallel(matrix1, matrix2, threads_count))}");
        }
    }
}
