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
			var result = new DiseaseWithSymptomNames();

			result.DiseaseId = disease.DiseaseId;
			result.Name = disease.Name;

			foreach (var symptom in disease.Symptoms)
			{
				
			}

			return result;
		}
	}
}
