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
	public class DiseaseSymptomRepository : EFRepository<DiseaseSymptom>, IDiseaseSymptomRepository
	{
		public DiseaseSymptomRepository(IDbContext dBContext) : base(dBContext) { }
	}
}
