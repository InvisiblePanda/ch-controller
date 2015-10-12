using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClickerHeroesControl
{
	/// <summary>
	/// The exception that is thrown when the target window (handle) cannot be found.
	/// </summary>
	public class TargetWindowNotFoundException : Exception
	{
		public TargetWindowNotFoundException()
		{
		}

		public TargetWindowNotFoundException(string message)
			: base(message)
		{
		}

		public TargetWindowNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}