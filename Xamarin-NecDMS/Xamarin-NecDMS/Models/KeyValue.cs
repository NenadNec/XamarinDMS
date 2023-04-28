using System;
using System.Collections.Generic;
using System.Text;

namespace NecDMS.Models
{
	public class KeyValue
	{
		public int key { get; set; }
		public string value { get; set; }
		public List<int> key_group { get; set; }
	}
}
