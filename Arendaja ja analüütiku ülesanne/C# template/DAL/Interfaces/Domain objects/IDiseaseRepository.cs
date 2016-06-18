using System.Collections.Generic;
using System.Linq;
using DAL.Helpers.Models;
using Domain;

namespace DAL.Interfaces.Domain_objects
{
	public interface IDiseaseRepository : IEFRepository<Disease>
	{
		/// <summary>
		/// Checks of a disease with the given diseaseName exists already. If not, one is created.
		/// </summary>
		/// <param name="diseaseName">Disease's name</param>
		/// <returns>Disease's id</returns>
		int addIfNotExists(string diseaseName);

		/// <summary>
		/// Finds 3 diseases with the most amount of symptoms.
		/// Ordered by symptom amount. If the amounts are equal, then the diseases are ordered alphabetically.
		/// </summary>
		/// <returns></returns>
		List<Disease> topThreeDiseases();

		/// <summary>
		/// Queries the database for diseases that fit the given symptoms.
		/// </summary>
		/// <param name="symptoms"></param>
		/// <returns>List of possible diseases</returns>
		List<Disease> possibleDiseases(string[] symptoms);
		//List<DiseaseWithSymptomNames> possibleDiseases(string[] symptoms);
	}
}
