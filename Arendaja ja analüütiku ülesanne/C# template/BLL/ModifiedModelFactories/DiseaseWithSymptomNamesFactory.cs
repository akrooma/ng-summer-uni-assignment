using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.ModifiedDomainModels;
using Domain;

namespace BLL.ModifiedModelFactories
{
	public static class DiseaseWithSymptomNamesFactory
	{
		public static DiseaseWithSymptomNames createObject(Disease disease)
		{
			var result = new DiseaseWithSymptomNames
			{
				DiseaseId = disease.DiseaseId,
				Name = disease.Name,
				
				Symptoms = disease.Symptoms.Select(s => s.Symptom.Name).ToList()
			};
			
			return result;
		}
	}
}