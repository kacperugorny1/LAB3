namespace MatrixClass
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int threads_count = 12;
            int seed = 0;
            Matrix matrix1 = new(900, 1200, new(seed));
            Matrix matrix2 = new(1200, 900, new(seed + 2));
            
            Matrix ans = Matrix.Multiply(matrix1, matrix2);
            Matrix ans_thread = Matrix.MultiplyThread(matrix1, matrix2, threads_count);
            Matrix ans_parallel = Matrix.MultiplyParallel(matrix1, matrix2, threads_count);

            //Console.WriteLine("A:\n" + matrix1);
            //Console.WriteLine("B:\n" + matrix2);
            //Console.WriteLine("Mnożenie jednowątkowe\n" + ans);
            //Console.WriteLine("Mnożenie threads\n" + ans_thread);
            //Console.WriteLine("Mnożenie parallel\n" + ans_parallel);

            Console.WriteLine($"Thread - {ans.Equals(ans_thread)}");

            Console.WriteLine($"Parallel - {ans.Equals(ans_parallel)}");
        }
    }
}
