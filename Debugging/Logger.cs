using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Debugging
{
	public class Logger : TraceListener
	{
		public string Extension { get; set; } = "log";

		public string DefaultOutput { get; set; } = "default";

		public bool IncluirFecha { get; set; } = true;

		Dictionary<string, StreamWriter> Streams = new Dictionary<string, StreamWriter> ();
		readonly StreamWriter defStream;

		StreamWriter getStream (string category)
		{
			StreamWriter ret;
			if (Streams.TryGetValue (category, out ret))
				return ret;
			ret = File.AppendText (category + "." + Extension);
			ret.AutoFlush = true;
			Streams.Add (category, ret);
			return ret;
		}

		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose (bool disposing)
		{
			foreach (var str in Streams)
				str.Value.Dispose ();
			base.Dispose (disposing);
		}


		public override void Close ()
		{
			foreach (var str in Streams)
				str.Value.Close ();
			Streams.Clear ();
			base.Close ();
		}

		public override void Flush ()
		{
			foreach (var str in Streams)
				str.Value.Flush ();
			base.Flush ();
		}

		string completeMessage (string message)
		{
			return IncluirFecha ? string.Format ("{0:T}\t{1}", DateTime.Now, message) : message;
		}

		public void WriteInAll (string message)
		{
			foreach (var x in Streams.Values)
				x.Write (message);
		}

		public void WriteLineInAll (string message)
		{
			WriteInAll (message + "\n");
		}

		public override void Write (string message)
		{
			defStream.Write (completeMessage (message));
		}

		public override void WriteLine (string message)
		{
			defStream.WriteLine (completeMessage (message));
		}

		public override void Write (string message, string category)
		{
			var stream = getStream (category);
			stream.Write (completeMessage (message));
		}

		public override void WriteLine (string message, string category)
		{
			var stream = getStream (category);
			stream.WriteLine (completeMessage (message));
		}

		public Logger (string name = "Logger")
			: base (name)
		{
			defStream = File.AppendText (DefaultOutput + "." + Extension);
			defStream.AutoFlush = true;
		}
	}
}