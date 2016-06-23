using System.Collections.Generic;
using Domain;

namespace BLL.ModifiedDomainModels
{
	public class DiseaseForDiagnosis
	{
		public int DiseaseId { get; set; }
		public string Name { get; set; }
		public List<Symptom> Symptoms { get; set; } = new List<Symptom>();

		// the amount of diseases the least popular symptom (by disease count, for this specific disease) is a symptom of.
		public int LeastPopularSymptomOccurrences { get; set; }
	}
}