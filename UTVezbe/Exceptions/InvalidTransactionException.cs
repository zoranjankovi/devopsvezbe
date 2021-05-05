using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTVezbe.Exceptions
{
	public class InvalidTransactionException : Exception
	{
		public InvalidTransactionException(string messTransactionType) : base(messTransactionType) { }
		public InvalidTransactionException() : this("Nevalidna transakcija") { }
	}
}
