using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using PickAWinner.Model;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace PickAWinner
{
	static class DataProcessor
	{
		private static int numDataColumns = int.Parse(ConfigurationManager.AppSettings["numDataColumns"]);
		private static int chanceCost = int.Parse(ConfigurationManager.AppSettings["costOfChance"]);
		private static DebugWriter debug = new DebugWriter();

		public static List<Donation> donations = new List<Donation>();
		public static List<Entry> entries = new List<Entry>();

		public static void LoadCSV(string filePath)
		{
			debug.Write($"Loading CSV File: {filePath}");

			foreach (var s in File.ReadAllLines(filePath))
			{
				var splitLine = s.Split(new char[] { ',' });

				// If the line doesn't have the correct amount of data columns or the email address is invalid, skip this record
				if (splitLine.Length < numDataColumns || !splitLine[5].Contains("@")) { continue; }

				ReportData.AddDonation(splitLine[0], double.Parse(splitLine[2]), splitLine[5]);									
			}

			debug.Write($"Finished loading CSV - {ReportData.Donations.Count} donators loaded.");
		}

		public static void ListDonators()
		{
			debug.Write("---------------------");
			debug.Write("Donator List");
			debug.Write("---------------------");
			
			TypeConsole.WriteHeader("Generating Donator List");
			
			if (ReportData.Donations.Count == 0)
			{
				debug.Write("No donators to list.");
			}
			else
			{
				debug.Write("Name".PadRight(30) + "Total".PadRight(10) + "Email".PadRight(40) + "Entries".PadRight(20));
				debug.Write("----".PadRight(30) + "-----".PadRight(10) + "-----".PadRight(40) + "-------".PadRight(20));
				foreach (var d in ReportData.Donations)
				{
					debug.Write($"{d.Name.PadRight(30)}{d.Amount.ToString().PadRight(10)}{d.Email.PadRight(40)}{Convert.ToInt32((d.Amount / chanceCost)).ToString().PadRight(20)}");
				}

				TypeConsole.WriteLine("Donator list generated!");
			}

			Console.WriteLine();
			System.Threading.Thread.Sleep(2000);
		}

		public static void ProcessEntries()
		{
			debug.Write("---------------------");
			debug.Write("Process Entries");
			debug.Write("---------------------");
			
			TypeConsole.WriteHeader("Processing Entries");
			
			if (ReportData.Donations.Count == 0)
			{
				debug.Write("No donations to process.");

				TypeConsole.WriteLine("No donations to process.");
			}
			else
			{
				foreach(var d in ReportData.Donations)
				{
					var email = d.Email;
					var name = d.Name;
					var numChances = Convert.ToInt32(d.Amount) / chanceCost;										

					// Divide each donations amount by the cost of each entry to split the donation out into a list of entries.
					for(int i=0; i<numChances; i++)
					{
						entries.Add(
							new Entry()
							{
								Name = name,
								Email = email
							}
						);
					}
				}

				// Assign ID # to each entry
				for (int i = 0; i < entries.Count; i++)
				{
					entries[i].Id = i+1;
				}

				debug.Write($"{entries.Count} entries created out of {ReportData.Donations.Count} donators that had valid emails.");
				TypeConsole.WriteLine($"{entries.Count} entries created out of {ReportData.Donations.Count} donators that had valid emails.");
			}

			Console.WriteLine();
			System.Threading.Thread.Sleep(2000);
		}

		public static void ListEntries()
		{			
			debug.WriteHeader("Listing Entries");			
			
			TypeConsole.WriteHeader("Generating Entries List");
			
			if (entries.Count == 0)
			{
				debug.Write("No entries to list.");
				TypeConsole.WriteLine("No entries to list.");
			}
			else
			{
				foreach (var e in entries)
				{
					debug.Write($"[{e.Id}]".PadRight(10) + $"{e.Email}");					
				}

				TypeConsole.WriteLine("Entries list generated!");
			}

			Console.WriteLine();
			System.Threading.Thread.Sleep(2000);
		}

		public static void PickWinner()
		{
			Console.Clear();			
			
			debug.WriteHeader("Picking A Winner");						
			TypeConsole.WriteHeader("Picking A Winner!!!");			
			
			debug.Write("Shuffling entries...");
			TypeConsole.WriteLine("Shuffling entries...");

			entries.Shuffle();

			System.Threading.Thread.Sleep(2000);

			TypeConsole.WriteLine("Entries shuffled!");			
			debug.WriteHeader("Shuffled Entry List");			

			for(int i = 0; i<entries.Count; i++)
			{
				debug.Write($"[{i}]".PadRight(15) + $"{entries[i].Name.PadRight(30)} {entries[i].Email}");
			}

			var tick = (int)DateTime.Now.Ticks;
			var rand = new Random(tick);
			var winningId = rand.Next(1, entries.Count);
			
			debug.WriteHeader("RNG Creation");			
			debug.Write($"Tick used as seed: {tick}");
			debug.Write($"Winning Entry Element #: {winningId}");

			var winner = entries[winningId];
			
			debug.WriteHeader("Winner Information");			
			debug.Write($"Name: {winner.Name}");
			debug.Write($"Email: {winner.Email}");
			debug.Write("");
			debug.Write($"Total number of donations: { ReportData.Donations.Count}");
			debug.Write($"Total number of entries: { entries.Count}");
			debug.Write($"Winning Entry Element #: {winningId}");

			debug.Close();

			System.Threading.Thread.Sleep(2000);			
			
			Console.Clear();

			Console.SetCursorPosition((Console.WindowWidth / 2)-9, Console.WindowHeight / 2);
			TypeConsole.WriteLine("Drumroll please....");			

			System.Threading.Thread.Sleep(5000);						

			for (int i = 10; i > 0; i--)
			{
				Console.Clear();
				Console.CursorVisible = false;
				Console.SetCursorPosition((Console.WindowWidth / 2)-5, Console.WindowHeight / 2);
				TypeConsole.WriteLine(i + "...");
				System.Threading.Thread.Sleep(1000);
			}			

			DisplayWinner(winner.Name);

			Console.WriteLine();						
		}

		// Thanks StackOverflow!
		public static void Shuffle<T>(this IList<T> list)
		{
			var rng = new Random((int)DateTime.Now.Ticks);

			for(int i = list.Count - 1; i > 0; i--)
			{
				var nxt = rng.Next(i + 1);
				var tmp = list[nxt];
				list[nxt] = list[i];
				list[i] = tmp;
			}
		}

		private static void DisplayWinner(string winnerName)
		{
			var randColor = new Random((int)DateTime.Now.Ticks);

			var winnerText = "!!!CONGRATULATIONS!!! -- " + winnerName;

			Console.Clear();
			
			
			for (int i = 0; i < 100; i++)
			{
				Console.SetCursorPosition((Console.WindowWidth - winnerText.Length) / 2, Console.WindowHeight / 2);
				Console.ForegroundColor = (ConsoleColor)randColor.Next(1, 15);				
				Console.WriteLine(winnerText.ToUpperInvariant());
				System.Threading.Thread.Sleep(100);
				//Console.SetCursorPosition((Console.WindowWidth - winnerText.Length) / 2, Console.WindowHeight / 2);
				
			}

			Console.ResetColor();
		}
	}
}
