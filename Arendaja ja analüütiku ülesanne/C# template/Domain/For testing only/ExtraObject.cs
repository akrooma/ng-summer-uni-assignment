using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
	public class ExtraObject
	{
		public ExtraObject(string name)
		{
			Id = 1;
			Name = name;
		}

		public int Id { get; set; }
		public string Name { get; set; }
	}
}
