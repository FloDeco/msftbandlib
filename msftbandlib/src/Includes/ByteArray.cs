using System;
using System.IO;

namespace MSFTBandLib.Includes {

/// <summary>
/// Byte array include
/// 
/// This is a thin wrapper around `MemoryStream` and `BinaryWriter`; 
/// 	you should write directly by accessing `BinaryWriter` 
/// 	(avoid having to reimplement all `BinaryWriter.Write(...)` 
/// 	overloads due to types).
/// </summary>
public class ByteArray : IDisposable {

	///	<summary>Disposed</summary>
	public bool disposed  { get; protected set; }

	/// <summary>Memory stream</summary>
	public MemoryStream MemoryStream;

	/// <summary>Binary reader</summary>
	public BinaryReader BinaryReader;

	/// <summary>Binary writer</summary>
	public BinaryWriter BinaryWriter;


	/// <summary>Construct.</summary>
	public ByteArray() {
		this.MemoryStream = new MemoryStream();
		this.BinaryReader = new BinaryReader(this.MemoryStream);
		this.BinaryWriter = new BinaryWriter(this.MemoryStream);
	}


	/// <summary>Construct and write bytes.</summary>
	/// <param name="bytes">bytes</param>
	/// <returns>public</returns>
	public ByteArray(byte[] bytes) : this() {
		this.Write(bytes);
	}


	/// <summary>Dispose of the resources.</summary>
	public void Dispose() {
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}


	/// <summary>Dispose of the resources.</summary>
	/// <param name="disposing">Disposing (not used)</param>
	protected virtual void Dispose(bool disposing) {
		this.BinaryReader.Dispose();
		this.BinaryWriter.Dispose();
		this.MemoryStream.Dispose();
		this.disposed = true;
	}


	/// <summary>Write bytes.</summary>
	/// <param name="bytes">bytes</param>
	/// <returns>public</returns>
	public void Write(byte[] bytes) {
		this.BinaryWriter.Write(bytes);
	}


	/// <summary>Get the current byte array.</summary>
	/// <returns>byte[]</returns>
	public byte[] GetBytes() {
		return this.MemoryStream.ToArray();
	}


	/// <summary>
	/// Read a 2-byte unsigned integer from the stream using 
	/// 	the `BinaryReader`. Sets stream to given position, 
	/// 	and advances stream position by 2 once done. Uses 
	/// 	little-endian encoding.
	/// </summary>
	/// <param name="position">Position to read from</position>
	/// <returns>ushort</returns>
	public ushort GetUshort(int position=0) {
		this.BinaryReader.BaseStream.Position = position;
		return this.BinaryReader.ReadUInt16();
	}


	/// <summary>
	/// Get an array of `ushort` from the stream using 
	/// 	the `BinaryReader`, reading sequentially from 
	/// 	the given start position. Does not verify 
	/// 	the specified number of `ushort` is available.
	/// </summary>
	/// <param name="count">Number of `ushort` to read</param>
	/// <param name="pos">Position to read from</param>
	/// <returns>ushort[]</returns>
	public ushort[] GetUshorts(int count, int pos=0) {
		ushort[] ushorts = new ushort[count];
		for (int i = 0; i < count; i++) {
			pos = (i == 0) ? pos : (pos + 2);
			ushorts[i] = this.GetUshort(pos);
		}
		return ushorts;
	}

}

}