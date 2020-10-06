using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace PickAWinner
{
	class Program
	{		
		static void Main(string[] args)
		{			
			if (args == null || args.Length == 0)
			{
				Console.WriteLine("No input file provided. Exiting.\n");
				Console.WriteLine("Press any key to exit...");
				Console.ReadKey();
			}
			else
			{				
				DataProcessor.LoadCSV(args[0]);
				DataProcessor.ListDonators();
				DataProcessor.ProcessEntries();
				DataProcessor.ListEntries();
				DataProcessor.PickWinner();

				Console.WriteLine("Press any key to exit...");				
				Console.ReadKey();
			}
		}

	}
}
