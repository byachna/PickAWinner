using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PickAWinner
{
	public class DebugWriter
	{
		private FileStream output;
		private StreamWriter writer;

		public DebugWriter()
		{
			try
			{
				output = new FileStream("./Output.txt", FileMode.OpenOrCreate, FileAccess.Write);
				writer = new StreamWriter(output);
			}
			catch (Exception e)
			{
				Console.WriteLine("Cannot open Output.txt for writing");
				Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {e.Message}");
				return;
			}
		}

		public void Write(string s)
		{
			if(output == null || writer == null) { return; }
			writer.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {s}");
		}

		public void WriteHeader(string text)
		{
			Write("---------------------");
			Write($"{text}");
			Write("---------------------");
		}

		public void Close()
		{
			writer.Close();
			output.Close();
		}
	}
}
