namespace GiocoSedia
{
    internal static class Program
    {
       
    static List<Thread> threads = new List<Thread>();
        static List<Thread> threadsDaRimuovere = new List<Thread>();
        static int numSedie;
        static int numSedieLibere;
        static object lockObject = new object();
        static Random random = new Random();
        static bool giocoFinito = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Benvenuti al gioco delle sedie musicali!");
            Console.WriteLine("Inserisci il numero di bambini: ");
            int numBambini = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Inserisci il numero di sedie: ");
            numSedie = Convert.ToInt32(Console.ReadLine());
            numSedieLibere = numSedie;

            if (numSedie < 1)
            {
                Console.WriteLine("Il numero di sedie deve essere maggiore di 0.");
                return;
            }

            for (int i = 0; i < numBambini; i++)
            {
                Thread thread = new Thread(Play);
                thread.Name = "Bambino" + (i + 1);
                threads.Add(thread);
                thread.Start();
            }

            while (!giocoFinito)
            {
                lock (lockObject)
                {
                    foreach (var thread in threadsDaRimuovere)
                    {
                        threads.Remove(thread);
                    }
                    threadsDaRimuovere.Clear();
                }

                foreach (var thread in threads)
                {
                    if (thread.IsAlive)
                    {
                        thread.Join();
                    }
                }
            }

            Console.WriteLine("Il gioco è finito. Tutti i bambini hanno vinto!");
            Console.ReadLine();
        }

        static void Play()
        {
            while (true)
            {
                lock (lockObject)
                {
                    if (giocoFinito)
                        return;

                    if (numSedieLibere > 0)
                    {
                        Console.WriteLine("La musica si è fermata! " + Thread.CurrentThread.Name + " si è seduto.");
                        numSedieLibere--;
                    }
                    else
                    {
                        Console.WriteLine("La musica si è fermata! " + Thread.CurrentThread.Name + " si è eliminato.");
                        threadsDaRimuovere.Add(Thread.CurrentThread);
                        return;
                    }
                }

                Thread.Sleep(1500);

                lock (lockObject)
                {
                    if (numSedieLibere == 0 && numSedie > 0)
                    {
                        numSedie--;
                        if (numSedie == 0)
                        {
                            Console.WriteLine(Thread.CurrentThread.Name + " ha vinto.");
                            giocoFinito = true;
                            return;
                        }
                        numSedieLibere = numSedie;
                    }
                }
            }
        }
    }

}
