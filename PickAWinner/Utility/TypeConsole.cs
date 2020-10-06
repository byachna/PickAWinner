using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PickAWinner
{
	static class TypeConsole
	{
		public static void WriteLine(string text)
		{
			for(int i = 0; i<text.Length; i++)
			{
				Console.Write(text[i]);
				Thread.Sleep(10);				
			}
			Console.WriteLine();
		}

		public static void WriteHeader(string text)
		{
			WriteLine("===========================");
			WriteLine($"{text}");
			WriteLine("===========================");
		}
	}
}
