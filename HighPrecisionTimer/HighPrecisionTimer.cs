using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighPrecisionTimer
{
	public sealed class HighPrecisionTimer
	{
		#region Fields

		private readonly Stopwatch sw = new Stopwatch();

		#endregion

		#region Events

		public delegate void ElapsedEventHandler(object sender, ElapsedEventArgs)

		public event ElapsedEventHandler Elapsed;

		#endregion

		#region Properties

		public bool Enabled { get; set; } = true;
		public bool AutoReset { get; set; } = false;
		public double Interval { get; set; }

		#endregion

		#region Constructors

		public HighPrecisionTimer()
		{
		}

		public HighPrecisionTimer(double interval)
		{
			Interval = interval;
		}

		#endregion
	}
}