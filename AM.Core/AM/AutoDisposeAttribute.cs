/* AutoDisposeAttribute.cs -- mark instance field as auto-disposable 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM
{
	/// <summary>
	/// Mark instance field as auto-disposable.
	/// </summary>
	[AttributeUsage ( AttributeTargets.Field | AttributeTargets.Property )]
	public sealed class AutoDisposeAttribute
		: Attribute
	{
	}
}