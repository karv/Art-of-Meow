using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Debugging
{
	/// <summary>
	/// A debug logger
	/// </summary>
	public class Logger : TraceListener
	{
		/// <summary>
		/// Gets or set the extension of the files created
		/// </summary>
		public string Extension { get; set; } = "log";

		/// <summary>
		/// Name of the default output
		/// </summary>
		public string DefaultOutput { get; set; } = "default";

		/// <summary>
		/// Determines if the date and time should be logged before every log entry
		/// </summary>
		public bool IncludeDate { get; set; } = true;

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

		/// <summary>
		/// Close the streams
		/// </summary>
		public override void Close ()
		{
			foreach (var str in Streams)
				str.Value.Close ();
			Streams.Clear ();
			base.Close ();
		}

		/// <summary>
		/// Flush this instance.
		/// </summary>
		public override void Flush ()
		{
			foreach (var str in Streams)
				str.Value.Flush ();
			base.Flush ();
		}

		string completeMessage (string message)
		{
			return IncludeDate ? string.Format ("{0:T}\t{1}", DateTime.Now, message) : message;
		}

		/// <summary>
		/// Write a message to all the listeners
		/// </summary>
		/// <param name="message">Message.</param>
		public void WriteInAll (string message)
		{
			foreach (var x in Streams.Values)
				x.Write (message);
		}

		/// <summary>
		/// Writes the line to all the listeners
		/// </summary>
		/// <param name="message">Message.</param>
		public void WriteLineInAll (string message)
		{
			WriteInAll (message + "\n");
		}

		/// <summary>
		/// Write the specified message to the default listener
		/// </summary>
		/// <param name="message">Message.</param>
		public override void Write (string message)
		{
			defStream.Write (completeMessage (message));
		}

		/// <summary>
		/// Write the specified message to the default listener
		/// </summary>
		/// <param name="message">Message.</param>
		public override void WriteLine (string message)
		{
			defStream.WriteLine (completeMessage (message));
		}

		/// <summary>
		/// Write the specified message and category.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="category">Category.</param>
		public override void Write (string message, string category)
		{
			var stream = getStream (category);
			stream.Write (completeMessage (message));
		}

		/// <summary>
		/// Write the specified message and category.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="category">Category.</param>
		public override void WriteLine (string message, string category)
		{
			var stream = getStream (category);
			stream.WriteLine (completeMessage (message));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Debugging.Logger"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		public Logger (string name = "Logger")
			: base (name)
		{
			defStream = File.AppendText (DefaultOutput + "." + Extension);
			defStream.AutoFlush = true;
		}
	}
}