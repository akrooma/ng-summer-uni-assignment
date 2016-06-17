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
	}
}
