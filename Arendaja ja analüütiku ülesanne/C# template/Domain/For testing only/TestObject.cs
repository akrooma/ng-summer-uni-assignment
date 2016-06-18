using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
	/// <summary>
	/// Not a part of the database, just for testing in general.
	/// </summary>
	public class TestObject
	{
		public TestObject(string text)
		{
			Id = 1;
			Text = text;
			Objects = new List<ExtraObject>();
			Strings = new List<string>();
		}

		public int Id { get; set; }
		public string Text { get; set; }

		public ICollection<string> Strings { get; set; }

		public ICollection<ExtraObject> Objects { get; set; }
	}
}
