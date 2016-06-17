using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
	public class Symptom
	{
		// PK
		public int SymptomId { get; set; }
		public string Name { get; set; } // could be called SymptomName if the need arises.

		public virtual ICollection<DiseaseSymptom> Diseases { get; set; }
	}
}
