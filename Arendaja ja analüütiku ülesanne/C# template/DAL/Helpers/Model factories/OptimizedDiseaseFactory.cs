using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Helpers.Modified_domain_models;
using Domain;

namespace DAL.Helpers.Model_factories
{
	public static class OptimizedDiseaseFactory
	{
		public static OptimizedDisease createObjectForDiagnosing(Disease disease)
		{
			return new OptimizedDisease
			{
				DiseaseId = disease.DiseaseId,
				Name = disease.Name,
				LeastPopularSymptomOccurrences = disease.Symptoms.Min(s => s.Symptom.Diseases.Count),
				Symptoms = disease.Symptoms.Select(s => s.Symptom).OrderBy(s => s.Diseases.Count).ToList()
			};
		}
	}
}
