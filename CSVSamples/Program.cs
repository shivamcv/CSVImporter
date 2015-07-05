using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            Createsamples();

            Console.ReadLine();
        }

        static string source = @"B:\Workshop\DataImporter\Csv Sample\Csv Sample\Nest-Backup";
        static string Destination = @"B:\Workshop\DataImporter\Csv Sample\Csv Sample\Test";
       static int ctr = 20000;
        private static void Createsamples()
        {
            var samples = Directory.GetFiles(source);

            for (int i = 0; i < ctr; i+= samples.Count())
            {
                for (int j = 0; j < samples.Count(); j++)
                {
                    Console.Write("\rCreating " + (i + j));
                    File.Copy(samples[j],Path.Combine(Destination,(i+j) + Path.GetExtension(samples[j])));
                }
            }

        }
    }
}
