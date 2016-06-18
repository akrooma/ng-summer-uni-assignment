using System.Collections.Generic;
using Domain;

namespace DAL.Interfaces.Domain_objects
{
	public interface IDiseaseRepository : IEFRepository<Disease>
	{
		int AddIfNotExists(string name);
		List<Disease> topThreeDiseases();
		List<Disease> possibleDiseases(string[] symptoms);
	}
}
