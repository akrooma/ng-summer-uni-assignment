using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.ModifiedDomainModels;
using BLL.ModifiedModelFactories;
using DAL.Interfaces;
using DAL.Interfaces.Domain_objects;
using Domain;

namespace DAL.Repositories.Domain_objects
{
	public class DiseaseRepository : EFRepository<Disease>, IDiseaseRepository
	{
		public DiseaseRepository(IDbContext dBContext) : base(dBContext) { }

		// Note: see interface summaries for info.

		/// <summary>
		/// Implements <see cref="IDiseaseRepository.addIfNotExists(string)"/>
		/// </summary>
		/// <param name="diseaseName">Name of a disease</param>
		/// <returns>Disease's id</returns>
		public int addIfNotExists(string diseaseName)
		{
			//var disease = DbSet.SingleOrDefault(s => s.Name == diseaseName);

			//if (disease != null)
			//	return disease.DiseaseId;

			//disease = new Disease { Name = diseaseName };
			var disease = new Disease { Name = diseaseName };
			Add(disease);
			SaveChanges();

			return disease.DiseaseId;
		}

		/// <summary>
		/// Implements <see cref="IDiseaseRepository.topThreeDiseases()"/>
		/// </summary>
		/// <returns>List of 3 most popular diseases by symptom count.</returns>
		public List<Disease> topThreeDiseases()
		{
			var query = DbSet.OrderByDescending(d => d.Symptoms.Count)
				.ThenBy(d => d.Name)
				.Take(3);

			return query.ToList();
		}

		/// <summary>
		/// Implements <see cref="IDiseaseRepository.possibleDiseases(string [])"/>
		/// </summary>
		/// <param name="symptoms">Array of symptom names.</param>
		/// <returns>List of diseases with given symptoms.</returns>
		public List<Disease> possibleDiseases(string [] symptoms)
		{
			var query = (IQueryable<Disease>) DbSet;
			
			foreach (var symptom in symptoms)
			{
				query = query.Where(c => c.Symptoms.Any(s => s.Symptom.Name == symptom.Trim()));
			}
			
			return query.ToList();

			#region alternative_solution
			// Note: with this, the method's return type is List<DiseaseWithSymptomNames>
			// in order to use this, changes have to be made to the IDiseaseRepository file as well.
			// It comes down to converting all the entries in the dbset to a new object format VS
			// the complexity of the active solution's sql statement + object tree traversal.

			//var result = DbSet.ToList().Select(d => DiseaseWithSymptomNamesFactory.createEntity(d));

			//foreach (var symptomName in symptoms)
			//{
			//	result = result.Where(d => d.Symptoms.Contains(symptomName));
			//}

			//return result.ToList();

			#endregion
		}

		/// <summary>
		/// Implements <see cref="IDiseaseRepository.allDiseasesWithJustSymptomNames()"/>
		/// </summary>
		/// <returns>List of diseases</returns>
		public List<DiseaseWithSymptomNames> allDiseasesWithJustSymptomNames()
		{
			return All.Select(d => DiseaseWithSymptomNamesFactory.createObject(d)).ToList();
		}

		/// <summary>
		/// Implements <see cref="IDiseaseRepository.allDiseasesOptimizedForDiagnosis()"/>
		/// </summary>
		/// <returns>List of diseases</returns>
		public List<DiseaseForDiagnosis> allDiseasesOptimizedForDiagnosis()
		{
			return All.Select(d => DiseaseForDiagnosisFactory.createObjectForDiagnosing(d))
				.OrderBy(d => d.LeastPopularSymptomOccurrences)
				.ThenBy(d => d.Symptoms.Count)
				.ToList();
		}

	} // DiseaseRepo class
} // namespace
