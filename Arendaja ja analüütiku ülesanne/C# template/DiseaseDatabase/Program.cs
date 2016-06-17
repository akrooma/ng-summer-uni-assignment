using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services;

namespace DiseaseDatabase
{
	class Program
	{
		private static DiseaseService _diseaseService;

		static void Main(string[] args)
		{
			Console.WriteLine(args.ToString());
			_diseaseService = new DiseaseService();
			return;
			

			var csvLines = getInputFileContent(args);
			
			
			foreach (var csvLine in csvLines)
			{
				// TODO: Read all lines to memory
				Console.WriteLine(csvLine);
			}

			// TODO: Implement homework points

			Console.ReadLine();

			
		}

		/// <summary>
		/// Tries to read lines from the specified file and return them as a string array. 
		/// User is prompted to re-enter the string if something goes wrong(no arguments or too many given,
		/// an exception occured when reading the file).
		/// </summary>
		/// <param name="args">Array containing command line parameter.</param>
		/// <returns>Array containing lines from file.</returns>
		private static string[] getInputFileContent(string[] args)
		{
			if (args.Length == 1)
			{
				try
				{
					string[] result = File.ReadAllLines(args[0]);

					return result;
				}
				catch (Exception ex)
				{
					Console.WriteLine("Something went wrong parsing the given filename/ -path: " + ex.Message);
				}
			}
			
			Console.WriteLine("No arguments or too many given: insert the file's name or path:");

			var userInput = Console.ReadLine();

			return getInputFileContent(new [] { userInput.Trim() });
		}
	}
}
