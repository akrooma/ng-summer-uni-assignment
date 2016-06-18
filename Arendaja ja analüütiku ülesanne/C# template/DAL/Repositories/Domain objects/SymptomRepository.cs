using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.Interfaces.Domain_objects;
using Domain;

namespace DAL.Repositories.Domain_objects
{
	public class SymptomRepository : EFRepository<Symptom>, ISymptomRepository
	{
		public SymptomRepository(IDbContext dBContext) : base(dBContext) { }

		// Note: see interface for implementation summaries

		public int addIfNotExists(string symptomName)
		{
			var symptom = DbSet.SingleOrDefault(s => s.Name == symptomName);
			
			if (symptom != null)
				return symptom.SymptomId;
			
			symptom = new Symptom { Name = symptomName };
			Add(symptom);
			SaveChanges();
			
			return symptom.SymptomId;
		}

		public List<Symptom> topThreeSymptoms()
		{
			var query = DbSet.OrderByDescending(s => s.Diseases.Count).ThenBy(s => s.Name).Take(3);
			
			return query.ToList();
		}
	}
}
