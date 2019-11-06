﻿//----------------------------------------------------------------------------- 
// <copyright file="PinnedObject.cs" company="KEYENCE">
//	 Copyright (c) 2013 KEYENCE CORPORATION.  All rights reserved.
// </copyright>
//----------------------------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace PipeModel
{
	/// <summary>
	/// Object pinning class
	/// </summary>
	public sealed class PinnedObject : IDisposable
	{
		#region Field

		private GCHandle _Handle;	   // Garbage collector handle

		#endregion

		#region Property

		/// <summary>
		/// Get the address.
		/// </summary>
		public IntPtr Pointer
		{
			// Get the leading address of the current object that is pinned.
			get { return _Handle.AddrOfPinnedObject(); }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="target">Target to protect from the garbage collector</param>
		public PinnedObject(object target)
		{
			// Pin the target to protect it from the garbage collector.
			_Handle = GCHandle.Alloc(target, GCHandleType.Pinned);
		}

		#endregion

		#region Interface
		/// <summary>
		/// Interface
		/// </summary>
		public void Dispose()
		{
			_Handle.Free();
			_Handle = new GCHandle();
		}

		#endregion
	}
}
