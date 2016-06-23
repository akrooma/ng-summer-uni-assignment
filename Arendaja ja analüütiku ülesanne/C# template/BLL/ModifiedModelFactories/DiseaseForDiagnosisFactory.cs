using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.ModifiedDomainModels;
using Domain;

namespace BLL.ModifiedModelFactories
{
	public static class DiseaseForDiagnosisFactory
	{
		public static DiseaseForDiagnosis createObjectForDiagnosing(Disease disease)
		{
			return new DiseaseForDiagnosis
			{
				DiseaseId = disease.DiseaseId,
				Name = disease.Name,
				LeastPopularSymptomOccurrences = disease.Symptoms.Min(s => s.Symptom.Diseases.Count),
				Symptoms = disease.Symptoms.Select(s => s.Symptom).OrderBy(s => s.Diseases.Count).ToList()
			};
		}
	}
}
