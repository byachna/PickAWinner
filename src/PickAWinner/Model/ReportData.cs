using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PickAWinner.Model
{
	public static class ReportData
	{
		public static List<Donation> Donations { get; } = new List<Donation>();

		public static void AddDonation(string name, double amount, string email)
		{
			var donator = Donations.FirstOrDefault(x => x.Email == email);

			// If donator already exists, just sum the amounts. Otherwise, add new donator.
			if (donator != null)
			{
				Donations.Remove(donator);
				donator.Amount += amount;
				Donations.Add(donator);
			}
			else
			{
				Donations.Add(new Donation() { Name = name, Amount = amount, Email = email });
			}
		}
	}

	public class Donation
	{
		public string Name { get; set; }
		public double Amount { get; set; }
		public string Email { get; set; }
	}
}
