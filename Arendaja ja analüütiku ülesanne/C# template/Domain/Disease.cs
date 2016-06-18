using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Disease
    {
		// PK
		public int DiseaseId { get; set; }
		public string Name { get; set; } // if more suited, could be called DiseaseName

		public virtual ICollection<DiseaseSymptom> Symptoms { get; set; } = new List<DiseaseSymptom>();
	}
}
