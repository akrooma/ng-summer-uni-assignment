using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace DAL.Interfaces.Domain_objects
{
	public interface ISymptomRepository : IEFRepository<Symptom>
	{
		/// <summary>
		/// Checks if a symptom with given symptomName exists. If not, one is created.
		/// </summary>
		/// <param name="symptomName">Symptom's name</param>
		/// <returns>Symptom's id</returns>
		int addIfNotExists(string symptomName);

		/// <summary>
		/// Gets the three most popular symptoms by disease count. Result is ordered by disease count(high -> low) 
		/// and then by symptom's name alphabetically.
		/// </summary>
		/// <returns>List with 3 most popular symptoms</returns>
		List<Symptom> topThreeSymptoms();
	}
}
