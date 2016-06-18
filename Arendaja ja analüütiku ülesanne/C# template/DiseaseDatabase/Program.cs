using System;
using System.IO;
using System.Reflection;
using DAL;
using DAL.Helpers;
using DAL.Interfaces;
using DiseaseDatabase.Helpers;
using Domain;
using Ninject;

namespace DiseaseDatabase
{
	class Program
	{
		private static IUOW _uow;

		static void Main(string[] args)
		{
			init();
			//var csvLines = getInputFileContent(args);
			//var csvLines = getInputFileContent(new []{ "Diseases.csv" });
			//populateDatabase(csvLines);

			firstTask();


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
					Console.WriteLine(ex.Message + System.Environment.NewLine);
				}
			}
			else
			{
				Console.WriteLine("No arguments or too many given. Args[] length: " + args.Length);
			}
			
			Console.WriteLine("Insert the file's name or path:" + System.Environment.NewLine);

			var userInput = Console.ReadLine();

			return getInputFileContent(new [] { userInput.Trim() });
		}

		/// <summary>
		/// Populates the database with information based on the given file. Expects the file format to be such:
		/// Each line has data about a disease and it's symptoms. Disease and symptom names are separated with a comma.
		/// First string of a line is the disease, rest of the strings are symptoms.
		/// </summary>
		/// <param name="csvLines">String array containing lines from a file.</param>
		private static void populateDatabase(string[] csvLines)
		{
			Console.WriteLine("File lines retrieved, will begin populating the database with data." + System.Environment.NewLine);

			foreach (var csvLine in csvLines)
			{
				var csvLinePieces = csvLine.Split(',');

				var diseaseId = _uow.Diseases.AddIfNotExists(csvLinePieces[0].Trim());

				for (int i = 1; i < csvLinePieces.Length; i++)
				{
					var diseaseSymptom = new DiseaseSymptom
					{
						DiseaseId = diseaseId,
						SymptomId = _uow.Symptoms.AddIfNotExists(csvLinePieces[i].Trim())
					};

					_uow.DiseaseSymptoms.Add(diseaseSymptom);
				}
			}

			_uow.Commit();

			Console.WriteLine("Filling up the database was a success!" + System.Environment.NewLine);

			Console.WriteLine("Number of diseases: " + _uow.Diseases.All.Count + System.Environment.NewLine + 
					"Number of symptoms: " + _uow.Symptoms.All.Count + System.Environment.NewLine + 
					"Number of combinations: " + _uow.DiseaseSymptoms.All.Count);

		}

		/// <summary>
		/// Method to solve the first task.
		/// </summary>
		private static void firstTask()
		{
			topThreeDiseases();
			uniqueSymptomCount();
			topThreeSymptoms();
		}

		/// <summary>
		/// Subtask 1.1: top three diseases by symptom count, ordered by the count and then by disease name alphabetically.
		/// </summary>
		public static void topThreeDiseases()
		{
			var diseases = _uow.Diseases.topThreeDiseases();

			Console.WriteLine(System.Environment.NewLine + "1.1. Top three diseases by symptom count:");

			foreach (var disease in diseases)
			{
				Console.WriteLine("\t" + disease.Name);
			}
		}

		/// <summary>
		/// Subtask 1.2: the amount of unique symptoms.
		/// </summary>
		public static void uniqueSymptomCount()
		{
			var uniqueSymptomCount = _uow.Symptoms.All.Count;

			Console.WriteLine(System.Environment.NewLine + "1.2. The amount of unique symptoms:" + System.Environment.NewLine +
				"\t" + uniqueSymptomCount);
		}

		/// <summary>
		/// Subtask 1.3: top three symptoms by disease count. Ordered by disease count and then by symptom name.
		/// </summary>
		public static void topThreeSymptoms()
		{
			var symptoms = _uow.Symptoms.TopThreeSymptoms();

			Console.WriteLine(System.Environment.NewLine + "1.3. Top three symptoms by disease count:");

			foreach (var symptom in symptoms)
			{
				Console.WriteLine("\t" + symptom.Name);
			}
		}

		/// <summary>
		/// Method to solve the second task. 
		/// </summary>
		public static void secondTask()
		{
			Console.WriteLine(System.Environment.NewLine + "2.2. Potential diseases based on symptoms: (type 'Exit loop' to end this task)");

			while (true)
			{
				Console.WriteLine(System.Environment.NewLine + "Insert the list of symptoms separated by commas: ");
				var consoleInput = Console.ReadLine();
				
				if (inputIsExitCommand(consoleInput)) return;
				
				
			}
		}

		/// <summary>
		/// Things to do when the program starts: 
		/// a) load ninject kernel, get the UOW,
		/// b) clear the database for the new csv file,
		/// </summary>
		private static void init()
		{
			var kernel = new StandardKernel();
			kernel.Load(Assembly.GetExecutingAssembly());
			_uow = kernel.Get<IUOW>();

			//clearDatabase();
		}

		private static void clearDatabase()
		{
			_uow.Diseases.Clear();
			_uow.DiseaseSymptoms.Clear();
			_uow.Symptoms.Clear();
			_uow.Commit();
		}

		/// <summary>
		/// http://stackoverflow.com/questions/6371150/comparing-two-strings-ignoring-case-in-c-sharp
		/// </summary>
		/// <param name="input">String input from console</param>
		/// <returns>Boolean indicating if input string is exit command</returns>
		private static bool inputIsExitCommand(string input)
		{
			return string.Equals(input.Trim(), ConsoleCommands.ExitLoop, StringComparison.OrdinalIgnoreCase);
		}

	}// program.cs
}// namespace
