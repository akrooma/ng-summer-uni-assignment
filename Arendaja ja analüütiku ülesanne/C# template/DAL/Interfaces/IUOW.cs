﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces.Domain_objects;
using Domain;

namespace DAL.Interfaces
{
	public interface IUOW
	{
		//save pending changes to the data store
		void Commit();

		//get repository for type
		T GetRepository<T>() where T : class;

		//Standard repos, autogenerated. Repos without custom methods.
		IEFRepository<DiseaseSymptom> DiseaseSymptoms { get; }

		//Customs repos, manually implemented. Repos with custom methos.
		IDiseaseRepository Diseases { get; }
		ISymptomRepository Symptoms { get; }
	}
}
