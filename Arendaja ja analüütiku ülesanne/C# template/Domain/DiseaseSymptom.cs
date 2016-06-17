using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
	/// <summary>
	/// Interim table for connecting diseases with it's symptoms.
	/// </summary>
	public class DiseaseSymptom
	{
		// PK
		public int DiseaseSymptomId { get; set; }

		// Relations
		public int DiseaseId { get; set; }
		public virtual Disease Disease { get; set; }

		public int SymptomId { get; set; }
		public virtual Symptom Symptom { get; set; }
	}
}
