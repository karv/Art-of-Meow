using System.Diagnostics;

namespace Debugging
{
	public class Logger : TextWriterTraceListener
	{
		public Logger (string fileName)
			: base (fileName)
		{
		}
	}
}