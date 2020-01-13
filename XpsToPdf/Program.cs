using Syncfusion.XPS;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace XpsToPdf
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var memoryUsage = 1200000000;
            var step1 = 10000000;
            var step2 = 100000;
            Marshal.AllocHGlobal(memoryUsage);
            // warm up the application
            while (memoryUsage <= 1700000000)
            {

                memoryUsage += step1;
                try
                {
                    Marshal.AllocHGlobal(step1);
                    Console.WriteLine("Current Memory:" + memoryUsage);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Limit hit" + e.Message);
                }
            }

            while (true)
            {
                memoryUsage += step2;
                try
                {
                    Marshal.AllocHGlobal(step2);
                    Console.WriteLine("Current Memory:"+ memoryUsage);
                
                    var bytesOfXPS = File.ReadAllBytes(Directory.GetCurrentDirectory() + @"/Files/ToBeConverted.xps");
                    var converter = new XPSToPdfConverter();
                    var document = converter.Convert(bytesOfXPS);
                    using (var stream = new MemoryStream())
                    {
                        document.Save(stream);
                        stream.Position = 0;
                        document.Close(true);
                        if (!Directory.Exists("PDF"))
                        {
                            Directory.CreateDirectory("PDF");
                        }
                        File.WriteAllBytes($@"PDF/{Guid.NewGuid().ToString("N")}.pdf", stream.ToArray());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Limit hit" + e.Message);
                }
            }
        }
    }
}
