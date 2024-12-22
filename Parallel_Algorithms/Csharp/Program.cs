using System.Diagnostics;

namespace Csharp
{
    class Program
    {
        static void GetPrimes(int from, int to, ref int primes, int idx, bool testMode)
        {
            int tmp = 0;
            if(from < 3)
            {
                primes++;
                tmp = 3;
            }
            for (int i = from + tmp; i < to; ++i)
            {
                bool divisible = false;
                for(int j = 2; j <= (int)Math.Pow(i, 0.5); j++)
                {
                    if(i%j==0)
                    {
                        divisible = true;
                        break;
                    }
                }
                if(!divisible) primes++;
            }
            if(!testMode) System.Console.WriteLine($"::{idx+1}\t{from}\t{to}\t{primes}");
        }

        static void Main(string[] args)
        {
            if(args.Length >= 2) 
            {
                int N = int.Parse(args[0]);
                int threadCount = int.Parse(args[1]);
                bool testMode = args.Length < 3?false:(args[2].ToLower()=="true"?true:false);
                bool optimized = args.Length < 4?false:(args[3].ToLower()=="true"?true:false);

                
                Thread[] threads = new Thread[threadCount];
                int[] primeArray = new int[threadCount];long runTime;
                int primes = 0;


                if(optimized)
                {
                    int[] end1 = new int[threadCount];
                    int[] end2 = new int[threadCount];
                    int sliceValue = N / (threadCount * 2);

                    for(int i = 0; i < threadCount; i++)
                    {
                        end1[i] = sliceValue * (i+1);
                        end2[i] = (threadCount*sliceValue) + sliceValue * (i+1);
                    }

                    if (end2[threadCount-1] != N) end2[threadCount-1] = N;

                    Stopwatch watch = new Stopwatch();

                    watch.Start();
                    for(int i = 0; i < threadCount; i++)
                    {
                        int start = i!=0?end1[i-1]:0;
                        int end = end1[i];
                        int idx = i;

                        threads[i] = new Thread(() =>{
                            GetPrimes(start,end, ref primeArray[idx], idx, testMode);
                        });
                        threads[i].Start();
                    }

                    for(int i = 0; i<threadCount; i++)
                    {
                        threads[i].Join();
                        int start = 0 + i!=0?end2[i-1]:end1[threadCount-1];
                        int end = end2[i];
                        int idx = i;

                        threads[i] = new Thread(() => { GetPrimes(start,end, ref primeArray[idx], idx, testMode); });
                        threads[i].Start();
                    }

                    for(int i = 0; i<threadCount; i++) threads[i].Join();

                    for(int i = 0; i<threadCount; i++)
                    {
                        threads[i].Join();
                        primes+= primeArray[i];
                    }

                    watch.Stop();
                    runTime = watch.ElapsedMilliseconds;
                }
                else
                {
                    for(int i = 0; i < threadCount; i++)
                    {
                        int s = i * N/threadCount;
                        int e = (i+1) * N/threadCount;
                        int idx = i;
                        threads[i] = new Thread(() =>{
                            GetPrimes(s,e, ref primeArray[idx], idx, testMode);
                        });
                    }

                    Stopwatch watch = Stopwatch.StartNew();
                    for(int i = 0; i<threadCount; i++) threads[i].Start();

                    for(int i = 0; i<threadCount; i++)
                    {
                        threads[i].Join();
                        primes+= primeArray[i];
                    }
                    watch.Stop();
                    runTime = watch.ElapsedMilliseconds;
                }

                if(!testMode)
                {
                    Console.WriteLine($"Prime count from 0 to {N} : {primes}");
                    Console.WriteLine($"Calculation time: {runTime}ms");
                }
                else System.Console.WriteLine($"{runTime}");

            }
        }
    }
}