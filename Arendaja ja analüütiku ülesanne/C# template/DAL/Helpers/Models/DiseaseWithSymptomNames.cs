using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helpers.Models
{
	public class DiseaseWithSymptomNames
	{
		public int DiseaseId { get; set; }
		public string Name { get; set; }

		public List<string> Symptoms { get; set; } = new List<string>();
	}
}