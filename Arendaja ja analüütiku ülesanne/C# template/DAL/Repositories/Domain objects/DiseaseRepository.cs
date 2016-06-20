using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Helpers.Models;
using DAL.Helpers.Model_factories;
using DAL.Helpers.Modified_domain_models;
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
		/// <param name="diseaseName"></param>
		/// <returns></returns>
		public int addIfNotExists(string diseaseName)
		{
			var disease = DbSet.SingleOrDefault(s => s.Name == diseaseName);

			if (disease != null)
				return disease.DiseaseId;

			disease = new Disease { Name = diseaseName };
			Add(disease);
			SaveChanges();

			return disease.DiseaseId;
		}

		public List<Disease> topThreeDiseases()
		{
			var query = DbSet.OrderByDescending(d => d.Symptoms.Count)
				.ThenBy(d => d.Name)
				.Take(3);

			return query.ToList();
		}

		public List<Disease> possibleDiseases(string [] symptoms)
		{
			var query = (IQueryable<Disease>) DbSet;
			
			foreach (var symptom in symptoms)
			{
				query = query.Where(c => c.Symptoms.Any(s => s.Symptom.Name == symptom));
			}
			
			return query.ToList();
			
			#region alternative_solution
			// Note: with this, the method's return type is List<DiseaseWithSymptomNamesFactory>
			// in order to use this, changes have to be made to the IDiseaseRepository file as well.

			//var result = DbSet.ToList().Select(d => DiseaseWithSymptomNamesFactory.createEntity(d));

			//foreach (var item in symptoms)
			//{
			//	result = result.Where(d => d.Symptoms.Contains(item));
			//}

			//return result.ToList();

			#endregion
		}

		public List<DiseaseWithSymptomNames> allDiseasesWithJustSymptomNames()
		{
			return All.Select(d => DiseaseWithSymptomNamesFactory.createObject(d)).ToList();
		}

		public List<OptimizedDisease> allDiseasesOptimizedForDiagnosis()
		{
			return All.Select(d => OptimizedDiseaseFactory.createObjectForDiagnosing(d))
				.OrderBy(d => d.LeastPopularSymptomOccurrences)
				.ThenBy(d => d.Symptoms.Count)
				.ToList();
		}

	} // DiseaseRepo class
} // namespace
