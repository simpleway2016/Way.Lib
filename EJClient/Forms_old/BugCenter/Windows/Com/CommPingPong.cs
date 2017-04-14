using System;
using System.Threading;
using System.IO;
using System.Xml.Serialization;
using WayControls.Windows.CommBase;

namespace WayControls.Windows.CommBase
{
	/// <summary>
	/// Overlays CommBase to provide byte-level ping-pong communications were each transmitted byte
	/// illicits a single byte response which must be absorbed before sending the next byte.
	/// There is a default response timeout of 500ms after which a Timeout exception will be raised.
	/// This timeout can be changed by changing the transactTimeout parameter in the settings object.
	/// Use the Transact method for all communications.
	/// </summary>
	public abstract class CommPingPong : CommBase 
	{
		private byte[] RxByte;
		private ManualResetEvent TransFlag = new ManualResetEvent(true);
		private uint TransTimeout;

		/// <summary>
		/// Extends CommBaseSettings to add the settings used by CommLine.
		/// </summary>
		public class CommPingPongSettings : CommBase.CommBaseSettings 
		{
			/// <summary>
			/// Maximum time (ms) for the Transact method to complete (default: 500)
			/// </summary>
			public int transactTimeout = 500;

			public static new CommPingPongSettings LoadFromXML(Stream s)
			{
				return (CommPingPongSettings)LoadFromXML(s, typeof(CommPingPongSettings));
			}
		}
	
		/// <summary>
		/// Transmits a byte and waits for and returns the response byte.
		/// </summary>
		/// <param name="toSend">The byte to be sent.</param>
		/// <returns>The response byte.</returns>
		protected byte Transact(byte toSend) 
		{
			if (RxByte == null) RxByte = new byte[1];
			Send(toSend);
			TransFlag.Reset();
			if (!TransFlag.WaitOne((int)TransTimeout, false)) ThrowException("Timeout");
			byte s;
			lock(RxByte) {s = RxByte[0];}
			return s;
		}
		
		/// <summary>
		/// If a derived class overrides ComSettings(), it must call this prior to returning the settings to
		/// the base class.
		/// </summary>
		/// <param name="s">Class containing the appropriate settings.</param>
		protected void Setup(CommPingPongSettings s) 
		{
			TransTimeout = (uint)s.transactTimeout;
		}

		protected override void OnRxChar(byte ch) 
		{
			lock(RxByte) {RxByte[0] = ch;}
			if (!TransFlag.WaitOne(0,false))
			{
				TransFlag.Set();
			}
		}
	}

}
