using UnityEngine;

namespace USpring.Vectors
{
	public struct CustomVector4 : IVector
	{
		public Vector4 Vector4;

		public CustomVector4(Vector4 vector4)
		{
			this.Vector4 = vector4;
		}

		public float this[int index]
		{
			get
			{
				return Vector4[index];
			}
			set
			{
				Vector4[index] = value;
			}
		}

		public int GetSize()
		{
			int res = 4;
			return res;
		}
	}
}