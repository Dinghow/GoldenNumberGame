using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot2LastOrRandom
{
    class Program
    {
        static void Main(string[] args)
        {
        // 1. Read input

            // Read and parse input information
            string infoLine = Console.In.ReadLine();
            string[] infoArray = infoLine.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int rowCount = int.Parse(infoArray[0]);
            int columnCount = int.Parse(infoArray[1]);

            // Read input data from stdin
            var allData = new List<string[]>();
            // Read golden number
            var goldenNumberList = new List<double>();

            while (rowCount-- > 0)
            {
                // Each line is the history for one round
                var roundLine = Console.In.ReadLine();

                // The first value is golden number for this round
                // The 2N and 2N+1 are the values from player N
                // If the value is 0, means this value invalid
                var results = roundLine.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                allData.Add(results);
                goldenNumberList.Add(ConvertToDouble(results[0]));
            }

        // 2. process input and write output

            // This bot will return 2 values
            // One is the last golden number
            // Another one is the average of last golden number and last round value from this bot

            // If this is the first round and there is no history, return 50 * 0.618 for both 2 number
            if(goldenNumberList.Count() < 5)
            {
                int counts = goldenNumberList.Count();
                double number;

                if (counts == 0)
                    number = 10;
                else if (counts == 1)
                    number = 8;
                else if (counts == 2)
                    number = 6;
                else if (counts == 3)
                    number = 4;
                else
                    number = 3.5;

                Console.Out.Write(number);
                Console.Out.Write('\t');
                Console.Out.Write(number);
            }
            else
            {
                int listSize = goldenNumberList.Count();
                List<double> goldenNumbers1 = new List<double>();
                List<double> goldenNumbers2 = new List<double>();
                Random rd = new Random();
                double answer1,answer2;

                //Get last golden numbers
                if (listSize <= 10)
                {
                    goldenNumbers1 = goldenNumberList.GetRange(0, listSize);
                }
                else
                {
                    goldenNumbers1 = goldenNumberList.GetRange(listSize - 11, 10);
                }

                if (listSize <= 4)
                {
                    goldenNumbers2 = goldenNumberList.GetRange(0, listSize);
                }
                else
                {
                    goldenNumbers2 = goldenNumberList.GetRange(listSize - 5, 4);
                }

                double sum1 = 0,sum2 = 0;
                for(int i = 0; i < goldenNumbers1.Count(); i++)
                {
                    sum1 += goldenNumbers1[i];
                }

                for (int i = 0; i < goldenNumbers2.Count(); i++)
                {
                    sum2 += goldenNumbers2[i];
                }


                double average1 = sum1 / (double)(goldenNumbers1.Count());
                double average2 = sum2 / (double)(goldenNumbers2.Count());

                double random1 = rd.NextDouble() - 0.6;
                double random2 = rd.NextDouble() - 0.8;

                double delta1 = random1 * 0.05 * average1;
                double delta2 = random2 * 0.03 * average2;

                answer1 = average1 + delta1;
                answer2 = average2 + delta2;

                // Another one is the average of last golden number and last round value from this bot
                // Read file to get last round value from this bot
                //string fileContent = File.ReadAllText("data.txt");
                //double lastNumber = ConvertToDouble(fileContent);

                // Use average number as another number
                //double number2 = (lastNumber + goldenNumberList.LastOrDefault()) / 2;

                // Save this number to file
                //File.WriteAllText("data.txt", number2.ToString());

                // Write 2 numbers to stdout
                Console.Out.Write(answer1);
                Console.Out.Write('\t');
                Console.Out.Write(answer2);
            }
        }

        private static double ConvertToDouble(string str)
        {
            try
            {
                return double.Parse(str);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Convert \"{str}\" to double failed: {ex.Message}");
                return 0d;
            }
        }
    }
}
