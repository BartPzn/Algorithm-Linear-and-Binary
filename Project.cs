using System;
using System.Diagnostics;

// Uwagi - czysty kod
// 1.Zmienne uzywane w klasie zawsze na poczatku klasy.
// 2.Bloki for, if ... zawsze w klamrach nawet jesli maja jedna linijke bo inaczej kod robi
// sie nie czytelny.
// 3.Komenatarze zawsze w odzielnej linijce jak kod.
// 4.Pomiedzy metodami linijka odstepu.
// 5. Linijki kodu w metodach odzielaj liniami jesli instrukcje sie nie grupuja logicznie(trzeba miec troszke wyczucia)
// 6. Nazwy zmiennych, parametrow(oprocz zmiennych statycznych) zaczynamy z malej litery!!! 
namespace project04
{
    class MainClass
    {
        static int[] TestVector;

        public static void Main(string[] args)
        {
            //for (int arraySize = 26843545; arraySize <= 268435450; arraySize += 26843545)
            Console.WriteLine("Size\tLMaxI\tLMaxT\tBMaxI\tBMaxT\tLAvgI\tLAvgT\tBAvgI\tBAvgT");
            for (int arraySize = 2000000; arraySize <= 268435450; arraySize += 6000000)
            {
                Console.Write(arraySize);

                // tworzymy tablicę
                TestVector = new int[arraySize];

                // wypełniamy tablicę
                for (int i = 0; i < TestVector.Length; ++i)
                {
                    TestVector[i] = i;
                }

                // liniowe max instrumentacja
                LinearMaxInstr();

                // liniowe max czas
                LinearMaxTim();

                // binarne max instrumentacja
                BinaryMaxInstr();

                // binarne max czas
                BinaryMaxTim();

                // liniowe średnia instrumentacja
                LinearAvgInstr();

                // liniowe średnia czas
                LinearAvgTim();

                // binarne średnia instrumentacja
                BinaryAvgInstr();

                // binarne średnia czas
                BinaryAvgTim();

                Console.Write("\n");
            }
        }

        static bool IsPresent_LinearTim(int[] vector, int number)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                if (vector[i] == number)
                {
                    return true;
                }
            }

            return false;
        }
        static long counter;
        static int IsPresent_LinearInstr(int[] vector, int number)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                if (vector[i] == number)
                {
                    return i++;
                }
            }

            return -1;
        }

        static bool IsPresent_BinaryTim(int[] vector, int number)
        {
            int left = 0;
            int right = vector.Length - 1;
            int middle;

            while (left <= right)
            {
                middle = (left + right) / 2;
                counter++;
                if (vector[middle] == number)
                {
                    return true;
                }
                else if (vector[middle] > number)
                {
                    right = middle - 1;
                }
                else
                {
                    left = middle + 1;
                }
            }

            return false;
        }

        static int IsPresent_BinaryInstr(int[] vector, int number)
        {
            // ten kod trzeba  dokończyć licząc liczbę porównań
            int left = 0;
            int right = vector.Length - 1;
            int middle;
            var cost = 0;

            while (left <= right)
            {
                middle = (left + right) / 2;
                cost++;

                if (vector[middle] == number)
                {
                    return cost;
                }
                else
                {
                    if (vector[middle] > number)
                    {
                        right = middle - 1;
                    }
                    else
                    {
                        left = middle + 1;
                    }
                }
            }

            return -1;
        }

        static void LinearMaxInstr()
        {
            var sum = IsPresent_LinearInstr(TestVector, TestVector.Length - 1);

            Console.Write("\t" + sum);
        }

        // Sprawdzic co robi.
        static void LinearMaxTim()
        {
            const int nIter = 10;
            double elapsedSeconds;
            long elapsedTime = 0;
            long minTime = long.MaxValue;
            long maxTime = long.MinValue;
            long iterationElapsedTime;

            for (int n = 0; n < (nIter + 1 + 1); n++)
            {
                // long StartingTime = Stopwatch.GetTimestamp();
                // Stopwatcha sie inaczej uzywa :)
                var stopWatch = Stopwatch.StartNew();
                stopWatch.Start();

                IsPresent_LinearInstr(TestVector, TestVector.Length - 1);

                stopWatch.Stop();

                // konwersja na sekundy, jesli sprawdza sie czas wykononia jakiejs instrukcji to bardziej miarodanje sa ms
                iterationElapsedTime = stopWatch.ElapsedMilliseconds;
                elapsedTime += iterationElapsedTime;

                if (iterationElapsedTime < minTime)
                {
                    minTime = iterationElapsedTime;
                }
                if (iterationElapsedTime > maxTime)
                {
                    maxTime = iterationElapsedTime;
                }
            }

            elapsedTime -= (minTime + maxTime);

            elapsedSeconds = ((double)elapsedTime / 1000) * (1.0 / (nIter * Stopwatch.Frequency));

            Console.Write("\t" + elapsedSeconds.ToString("F4"));
        }

        static void BinaryMaxInstr()
        {
            var cost = IsPresent_BinaryInstr(TestVector, TestVector.Length);

            Console.Write("\t" + cost);
        }

        static void LinearAvgInstr()
        {
            int sumSearchCost = 0;

            for (int i = 0; i < TestVector.Length; i++)
            {
                var stepCost = IsPresent_LinearInstr(TestVector, i);
                sumSearchCost = +stepCost != -1 ? stepCost : 0;
            }

            Console.Write("\t" + ((double)sumSearchCost / (double)TestVector.Length).ToString("F1"));
        }
        static void BinaryMaxTim()
        {
            Stopwatch St = new Stopwatch();
            St.Reset();
            St.Start();
            for (int i = 0; i < 1000000; i++)
            {
                IsPresent_BinaryInstr(TestVector, number: -1);
            }
            St.Stop();
            Console.Write(St.ElapsedTicks / 1000000 + "\t");
        }
        static void LinearAvgTim()
        {
            int longer = TestVector.Length;
            Stopwatch St = new Stopwatch();
            St.Reset();
            St.Start();
            for (int i = 0; i < 1000; i++)
            {
                IsPresent_BinaryInstr(TestVector, number: longer / 2);
            }
            St.Stop();
            Console.Write(St.ElapsedTicks / 1000 + "\t");
        }
        static void BinaryAvgInstr()
        {
            foreach (var premedio in TestVector)
            {
                IsPresent_BinaryInstr(TestVector, premedio);
            }
            Console.Write(counter / TestVector.Length + "\t");
            counter = 0;
        }
        static void BinaryAvgTim()
        {
            Stopwatch st = new Stopwatch();
            st.Restart();
            st.Start();
            foreach (var premedio in TestVector)
            {
                IsPresent_BinaryInstr(TestVector, premedio);
            }
            Console.Write(st.ElapsedTicks / TestVector.Length);
        }

    }
}
