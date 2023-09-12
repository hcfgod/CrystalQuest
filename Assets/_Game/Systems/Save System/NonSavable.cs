using System;

namespace SaveSystem
{

	/// <summary>
	/// Mark a field or property as non savable.
	/// </summary>
	[AttributeUsage (
		AttributeTargets.Property | AttributeTargets.Field,
		Inherited = false,
		AllowMultiple = false )]
	public sealed class NonSavable : Attribute
	{
		public NonSavable ()
		{

		}
	}

}