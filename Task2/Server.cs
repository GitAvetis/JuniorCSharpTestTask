namespace Task2
{
    public static class Server
    {
        private static readonly ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        private static int _count = 0;

        public static int GetCount()
        {
            cacheLock.EnterReadLock();
            try
            {
                Console.WriteLine($"GetCount Started by T{Task.CurrentId} at {Environment.TickCount}");
                Thread.Sleep(10);// имитация работы
                return _count;
            }
            finally
            {
                cacheLock.ExitReadLock();
                Console.WriteLine($"GetCount finished by T{Task.CurrentId} at {Environment.TickCount}");
            }
        }

        public static void AddToCount(int value)
        {
            cacheLock.EnterWriteLock();

            try
            {
                _count += value;
                Console.WriteLine($"AddToCount({value}) Started by T{Task.CurrentId} at {Environment.TickCount}");
                Thread.Sleep(50);// имитация работы

            }
            finally
            {
                cacheLock.ExitWriteLock();
                Console.WriteLine($"AddToCount({value}) finished by T{Task.CurrentId} at {Environment.TickCount}");
            }
        }
    }
}
