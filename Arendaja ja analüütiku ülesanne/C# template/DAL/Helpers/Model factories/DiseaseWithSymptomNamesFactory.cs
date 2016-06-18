using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Helpers.Models;
using Domain;

namespace DAL.Helpers.Model_factories
{
	public static class DiseaseWithSymptomNamesFactory
	{
		public static DiseaseWithSymptomNames createEntity(Disease disease)
		{
			var result = new DiseaseWithSymptomNames
			{
				DiseaseId = disease.DiseaseId,
				Name = disease.Name
			};

			foreach (var symptom in disease.Symptoms)
			{
				result.Symptoms.Add(symptom.Symptom.Name);
			}

			return result;
		}
	}
}
