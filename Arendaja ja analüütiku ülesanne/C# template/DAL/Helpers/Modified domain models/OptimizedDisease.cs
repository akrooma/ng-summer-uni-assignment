using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace DAL.Helpers.Modified_domain_models
{
	public class OptimizedDisease
	{
		public int DiseaseId { get; set; }
		public string Name { get; set; }
		public int LeastPopularSymptomOccurrences { get; set; }
		public List<Symptom> Symptoms { get; set; } = new List<Symptom>();
	}
}
