using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Helpers.Models;
using DAL.Interfaces;
using DAL.Interfaces.Domain_objects;
using Domain;

namespace DAL.Repositories.Domain_objects
{
	public class DiseaseRepository : EFRepository<Disease>, IDiseaseRepository
	{
		public DiseaseRepository(IDbContext dBContext) : base(dBContext) { }

		/// <summary>
		/// Checks of a disease with the given name exists already. If not, one is created.
		/// </summary>
		/// <param name="name">Name of the disease</param>
		/// <returns>Disease's id</returns>
		public int AddIfNotExists(string name)
		{
			var disease = DbSet.SingleOrDefault(s => s.Name == name);

			if (disease != null)
				return disease.DiseaseId;

			disease = new Disease { Name = name };
			Add(disease);
			SaveChanges();

			return disease.DiseaseId;
		}

		/// <summary>
		/// Finds 3 diseases with the most amount of symptoms.
		/// Ordered by symptom amount. If the amounts are equal, then the diseases are ordered alphabetically.
		/// </summary>
		/// <returns></returns>
		public List<Disease> topThreeDiseases()
		{
			var query = DbSet.OrderByDescending(d => d.Symptoms.Count).ThenBy(d => d.Name).Take(3);

			return query.ToList();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="symptoms"></param>
		/// <returns></returns>
		public List<Disease> possibleDiseases(string [] symptoms)
		{
			//var query = DbSet.Where(d => d.Symptoms)

			return null;
		}

		public List<DiseaseWithSymptomNames> getAllDiseasesWithJustSymptomNames()
		{
			return DbSet.Select(d => new DiseaseWithSymptomNames()).ToList();
		}
	}
}
