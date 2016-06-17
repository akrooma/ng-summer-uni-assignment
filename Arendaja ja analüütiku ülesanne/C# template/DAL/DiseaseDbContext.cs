using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Domain;

namespace DAL
{
    public class DiseaseDbContext : DbContext, IDbContext
    {
	    public DiseaseDbContext() : base("name=DiseaseDbConnection")
	    {
		    Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DiseaseDbContext>());
	    }

		public DbSet<Disease> Diseases { get; set; }
		public DbSet<Symptom> Symptoms { get; set; }
		public DbSet<DiseaseSymptom> DiseaseSymptoms { get; set; }
    }
}
