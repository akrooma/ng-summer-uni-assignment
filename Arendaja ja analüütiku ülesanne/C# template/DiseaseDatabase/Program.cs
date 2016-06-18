using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Reflection;
using DAL;
using DAL.Helpers;
using DAL.Helpers.Model_factories;
using DAL.Interfaces;
using DiseaseDatabase.Helpers;
using Domain;
using Ninject;

namespace DiseaseDatabase
{
	class Program
	{
		/// <summary>
		/// Unit of work to hold all the repos that handle all the database tables.
		/// </summary>
		private static IUOW _uow;

		static void Main(string[] args)
		{
			init();
			//var csvLines = getInputFileContent(args);
			//var csvLines = getInputFileContent(new []{ "Diseases.csv" });
			//populateDatabase(csvLines);

			//firstTask();
			secondTask();
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
					var result = File.ReadAllLines(args[0]);

					return result;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message + System.Environment.NewLine);
				}
			}
			else
			{
				writeLineOnConsole("No arguments or too many given. Args[] length: ");
			}
			
			writeLineOnConsole("Insert the file's name or path:");

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
			writeLineOnConsole("File lines retrieved, will begin populating the database with data.");

			foreach (var csvLine in csvLines)
			{
				var csvLinePieces = csvLine.Split(',');

				for (var i = 1; i < csvLinePieces.Length; i++)
				{
					var diseaseSymptom = new DiseaseSymptom
					{
						DiseaseId = _uow.Diseases.addIfNotExists(csvLinePieces[0].Trim()),
						SymptomId = _uow.Symptoms.addIfNotExists(csvLinePieces[i].Trim())
					};
					
					_uow.DiseaseSymptoms.Add(diseaseSymptom);
				}
			}

			_uow.Commit();

			writeLineOnConsole("Filling up the database was a success!");

			/*
			Console.WriteLine("Number of diseases: " + _uow.Diseases.All.Count + System.Environment.NewLine + 
					"Number of symptoms: " + _uow.Symptoms.All.Count + System.Environment.NewLine + 
					"Number of combinations: " + _uow.DiseaseSymptoms.All.Count);
			*/
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
		/// Actual method is in the DiseaseRepo.
		/// </summary>
		public static void topThreeDiseases()
		{
			var diseases = _uow.Diseases.topThreeDiseases();

			writeLineOnConsole("1.1. Top three diseases by symptom count:");

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
			writeLineOnConsole("1.2. The amount of unique symptoms:");
			writeLineOnConsole("\t" + _uow.Symptoms.All.Count);
		}

		/// <summary>
		/// Subtask 1.3: top three symptoms by disease count. Ordered by disease count and then by symptom name.
		/// Actual querying method is in the SymptomRepository.
		/// </summary>
		public static void topThreeSymptoms()
		{
			var symptoms = _uow.Symptoms.topThreeSymptoms();

			writeLineOnConsole("1.3. Top three symptoms by disease count:");

			foreach (var symptom in symptoms)
			{
				Console.WriteLine("\t" + symptom.Name);
			}
		}

		/// <summary>
		/// Solution for the second task. This method mainly holds code for console manipulation.
		/// Querying and such is done in the DiseaseRepo.
		/// </summary>
		public static void secondTask()
		{
			writeLineOnConsole("2.2. Potential diseases based on symptoms: (type 'Exit task' to stop the cycle)");

			while (true)
			{
				writeLineOnConsole("Insert the list of symptoms separated by commas: ");

				var consoleInput = Console.ReadLine();

				if (inputIsExitCommand(consoleInput)) return;

				if (consoleInput.Length == 0) continue; // process starts all over again if the user pressed just enter.

				var diseases = _uow.Diseases.possibleDiseases(consoleInput.Split(','));

				if (diseases.Any())
				{
					Console.WriteLine(Environment.NewLine + "Result: possible diseases for given symptoms:");

					foreach (var disease in diseases)
					{
						Console.WriteLine("\t" + disease.Name);
					}
				}
				else
				{
					Console.WriteLine(Environment.NewLine + "Couldn't find any diseases for given symptoms: ");
					
					foreach (var symptom in consoleInput.Split(','))
					{
						Console.WriteLine("\t" + symptom);
					}
				}
				
				writeLineOnConsole(""); // so the console "looks pretty".
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void thirdTask()
		{
			
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
			return string.Equals(input.Trim(), ConsoleCommands.ExitTask, StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// So I wouldn't have to put '+ System.Environment.NewLine' into every Console.WriteLine command.
		/// </summary>
		/// <param name="text"></param>
		private static void writeLineOnConsole(string text)
		{
			Console.WriteLine(text + Environment.NewLine);
		}

	}// program.cs
}// namespace
