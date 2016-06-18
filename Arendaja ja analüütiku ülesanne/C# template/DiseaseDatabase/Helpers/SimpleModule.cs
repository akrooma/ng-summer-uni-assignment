using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Helpers;
using DAL.Interfaces;
using Ninject.Modules;

namespace DiseaseDatabase.Helpers
{
	public class SimpleModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IDbContext>().To<DiseaseDbContext>().InSingletonScope();
			Bind<EFRepositoryFactories>().To<EFRepositoryFactories>().InSingletonScope();
			Bind<IEFRepositoryProvider>().To<EFRepositoryProvider>().InSingletonScope();
			Bind<IUOW>().To<UOW>().InSingletonScope();
		}
	}
}
