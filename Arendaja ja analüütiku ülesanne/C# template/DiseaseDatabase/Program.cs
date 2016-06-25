using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BLL.ModifiedDomainModels;
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

			
			firstTask();
			secondTask();

			/*
			 * This method doesn't loop by itself like secondTask() does. Turning off the console app
			 * or ctrl+c (for my machine at least) would terminate it.
			 */
			while (true)
			{
				thirdTask();
			}
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
					writeLineOnConsole(ex.Message);
				}
			}
			else
			{
				writeLineOnConsole("No arguments or too many given. Args[] length: " + args.Length);
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

				var diseaseId = _uow.Diseases.addIfNotExists(csvLinePieces[0].Trim());

				for (var i = 1; i < csvLinePieces.Length; i++)
				{
					var diseaseSymptom = new DiseaseSymptom
					{
						DiseaseId = diseaseId,
						SymptomId = _uow.Symptoms.addIfNotExists(csvLinePieces[i].Trim())
					};
					
					_uow.DiseaseSymptoms.Add(diseaseSymptom);
				}
			}

			_uow.Commit();

			writeLineOnConsole("Filling up the database was a success!");
		}

		/// <summary>
		/// Has no real functionality. Simply calls out the subtask methods.
		/// </summary>
		private static void firstTask()
		{
			topThreeDiseases();
			uniqueSymptomCount();
			topThreeSymptoms();
		}

		/// <summary>
		/// Subtask 1.1: top three diseases by symptom count, ordered by the count and then by disease name alphabetically.
		/// Linq expression is in the disease repo.
		/// </summary>
		private static void topThreeDiseases()
		{
			var diseases = _uow.Diseases.topThreeDiseases();

			Console.WriteLine("1.1. Top three diseases by symptom count:");

			foreach (var disease in diseases)
			{
				Console.WriteLine("\t" + disease.Name);
			}
		}

		/// <summary>
		/// Subtask 1.2: the amount of unique symptoms.
		/// </summary>
		private static void uniqueSymptomCount()
		{
			Console.WriteLine(Environment.NewLine + "1.2. The amount of unique symptoms:");
			writeLineOnConsole("\t" + _uow.Symptoms.All.Count);
		}

		/// <summary>
		/// Subtask 1.3: top three symptoms by disease count. Ordered by disease count and then by symptom name.
		/// Linq expression is in the symptom repo.
		/// </summary>
		private static void topThreeSymptoms()
		{
			var symptoms = _uow.Symptoms.topThreeSymptoms();

			Console.WriteLine("1.3. Top three symptoms by disease count:");

			foreach (var symptom in symptoms)
			{
				Console.WriteLine("\t" + symptom.Name);
			}
		}

		/// <summary>
		/// Solution for the second task. This method mainly holds code for console manipulation.
		/// Querying and such is done in the DiseaseRepo.
		/// Doesn't have full input validation yet. Expects correct input: empty string, 'exit loop' or for example:
		/// 'symptomName1,symptomName2,symptomName3' etc.
		/// </summary>
		private static void secondTask()
		{
			writeLineOnConsole(Environment.NewLine + "2.2. Potential diseases based on symptoms: " +
			                    Environment.NewLine + "(type 'Exit task' to exit task loop)");

			while (true)
			{
				writeLineOnConsole("Insert the list of symptoms separated by commas: ");

				var consoleInput = Console.ReadLine();

				// If the user entered 'exit loop'.
				if (inputIsExitCommand(consoleInput)) return;

				// Process starts all over again if the user pressed just enter.
				if (consoleInput.Length == 0) continue; 

				// Gets a list of diseases with given symptoms.
				var diseases = _uow.Diseases.possibleDiseases(consoleInput.Split(','));

				if (diseases.Any())
				{
					Console.WriteLine(Environment.NewLine + "!!!Result: possible disease(s) for given symptom(s):");

					foreach (var disease in diseases)
					{
						Console.WriteLine("\t" + disease.Name);
					}
				}
				else
				{
					Console.WriteLine(Environment.NewLine + "???Result: couldn't find any diseases with given symptoms: ");
					
					foreach (var symptom in consoleInput.Split(','))
					{
						Console.WriteLine("\t" + symptom);
					}
				}

				writeLineOnConsole("");
			}
		}

		/// <summary>
		/// Main work is done by the next method: <see cref="proposeSymptom(List&lt;DiseaseForDiagnosis&gt;)"/>
		/// </summary>
		private static void thirdTask()
		{
			var diseases = _uow.Diseases.allDiseasesOptimizedForDiagnosis();

			Console.WriteLine(Environment.NewLine + "3. Diagnosis for the patient: answer 'yes' or 'no' for presented symptoms.");

			proposeSymptom(diseases);
		}

		private static void proposeSymptom(List<DiseaseForDiagnosis> diseases)
		{
			//var disease = diseases.FirstOrDefault();
			//var symptom = disease.Symptoms.FirstOrDefault(); // cannot be null since every disease has at least 1 symptom.
			var symptom = getProposedSymptom(diseases);

			writeLineOnConsole(Environment.NewLine + "Does the patient have a symptom called: " + symptom.Name + "?");

			var consoleInput = Console.ReadLine();

			// Selects all the diseases that don't have the proposed symptom.
			if (consoleInput == "no")
				diseases = diseases.Where(d => !d.Symptoms.Contains(symptom)).ToList();

			/*
			 * Selects all the diseases that have the proposed symptom, removes the symptom from the disease's 
			 * symptom list so that the algoritm wouldn't ask about it again.
			 */
			else if (consoleInput == "yes")
				diseases = diseases.Where(d => d.Symptoms.Contains(symptom) && d.Symptoms.Remove(symptom)).ToList();

			// A disease was found.
			if (diseases.Count == 1)
			{
				writeLineOnConsole(Environment.NewLine + "!!!Result: Proposed diagnoses: " + diseases.FirstOrDefault().Name);
			}

			/*
			 * Just in case. Algorithm shouldn't come to this point. It will if the database holds just one disease and
			 * the user answers 'no' for a symptom of that disease. This count check block used to be at the beginning of 
			 * the method.
			 */
			else if (diseases.Count == 0)
			{
				writeLineOnConsole("???Result: Something went wrong proposing the diagnosis?");
			}
			// Algorithm hasn't narrowed it down to just 1 disease yet. 
			else
			{
				proposeSymptom(diseases);
			}
		}

		/// <summary>
		/// Since the symptoms with the answer 'yes' are removed from the disease's symptom list, some
		/// disease symptom lists might end up empty -- cannot grab first disease's first symptom, have to go beyond.
		/// </summary>
		/// <param name="diseases">List of possible diseases.</param>
		/// <returns>A symptom.</returns>
		public static Symptom getProposedSymptom(List<DiseaseForDiagnosis> diseases)
		{
			foreach (var disease in diseases)
			{
				if (disease.Symptoms.Count != 0)
					return disease.Symptoms.FirstOrDefault();
			}
			
			// this method never reaches this point. Exists only so that the compiler gives no error.
			return null;
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

			clearDatabase();
		}

		/// <summary>
		/// Every time the program is run, a new csv file is given. I presume the last file's
		/// data isn't 
		/// </summary>
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
