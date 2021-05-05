using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTVezbe.Exceptions
{
	public class InvalidAgeException: Exception
	{
		public InvalidAgeException(string message) : base(message) { }
		public InvalidAgeException() : this("Nevalidna starosna dob osobe") { }
	}
}
