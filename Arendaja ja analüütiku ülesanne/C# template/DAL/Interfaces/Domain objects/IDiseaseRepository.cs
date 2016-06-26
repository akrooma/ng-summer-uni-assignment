using System.Collections.Generic;
using BLL.ModifiedDomainModels;
using Domain;

namespace DAL.Interfaces.Domain_objects
{
	public interface IDiseaseRepository : IEFRepository<Disease>
	{
		/// <summary>
		/// Checks of a disease with the given disease name exists already. If not, one is created.
		/// Note: right now it only creates a new disease with the given name, since the diseases given by the csv file
		/// are all unique.
		/// </summary>
		/// <param name="diseaseName">Disease's name</param>
		/// <returns>Disease's id</returns>
		int addIfNotExists(string diseaseName);

		/// <summary>
		/// Finds 3 diseases with the most amount of symptoms.
		/// Ordered by symptom amount. Then by disease name alphabetically.
		/// </summary>
		/// <returns></returns>
		List<Disease> topThreeDiseases();

		/// <summary>
		/// Queries the database for diseases that fit the given symptoms.
		/// </summary>
		/// <param name="symptoms"></param>
		/// <returns>List of possible diseases</returns>
		List<Disease> possibleDiseases(string[] symptoms);

		/// <summary>
		/// Returns a list of simpler disease objects; rather than having a list of compelex objects, each disease
		/// has a list of just symptom names as strings.
		/// </summary>
		/// <returns>List of all diseases</returns>
		List<DiseaseWithSymptomNames> allDiseasesWithJustSymptomNames();

		/// <summary>
		/// Gets all the diseases in a "simpler" format (a list of symptom objects). The diseases are ordered in a way
		/// I thought made sense for task three. Diseases are ordered by their least popular symptom -- for example, if a symptom 
		/// has just 1 disease in the system, that disease is first in the list.
		/// </summary>
		/// <returns>List of diseases in a specific order</returns>
		List<DiseaseForDiagnosis> allDiseasesOptimizedForDiagnosis();


		//List<DiseaseWithSymptomNames> possibleDiseases(string[] symptoms);
	}
}
