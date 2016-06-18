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

		/// <summary>
		/// Checks if a symptom with given name exists. If not, one is created.
		/// </summary>
		/// <param name="name">Symptom's name</param>
		/// <returns>Id of the symptom</returns>
		public int AddIfNotExists(string name)
		{
			var symptom = DbSet.SingleOrDefault(s => s.Name == name);
			
			if (symptom != null)
				return symptom.SymptomId;
			
			symptom = new Symptom { Name = name };
			Add(symptom);
			SaveChanges();
			
			return symptom.SymptomId;
		}

		/// <summary>
		/// Gets the three most popular symptoms by disease count. Result is ordered by disease count(high -> low) 
		/// and symptom name.
		/// </summary>
		/// <returns></returns>
		public List<Symptom> TopThreeSymptoms()
		{
			var query = DbSet.OrderByDescending(s => s.Diseases.Count).ThenBy(s => s.Name).Take(3);

			return query.ToList();
		}
	}
}
