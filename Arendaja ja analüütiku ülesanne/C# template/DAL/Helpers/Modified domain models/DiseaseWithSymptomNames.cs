using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helpers.Models
{
	/// <summary>
	/// Rather than having a list of DiseaseSymptom objects where you'd have to access DiseaseSymptom's Symptom object
	/// and then that symptom's name, here's a "helper disease object" that has a list of symptoms by just their names.
	/// </summary>
	public class DiseaseWithSymptomNames
	{
		public int DiseaseId { get; set; }
		public string Name { get; set; }

		public List<string> Symptoms { get; set; } = new List<string>();
	}
}