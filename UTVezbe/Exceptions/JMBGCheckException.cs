using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTVezbe.Exceptions
{
	public class JMBGCheckException : Exception
	{
		public JMBGCheckException(string message) : base(message) { }
		public JMBGCheckException() : this("Nevalidan JMBG") { }
	}
}
