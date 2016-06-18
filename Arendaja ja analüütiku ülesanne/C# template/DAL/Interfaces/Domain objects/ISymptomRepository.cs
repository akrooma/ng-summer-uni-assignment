using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace DAL.Interfaces.Domain_objects
{
	public interface ISymptomRepository : IEFRepository<Symptom>
	{
		int AddIfNotExists(string name);
		List<Symptom> TopThreeSymptoms();
	}
}
