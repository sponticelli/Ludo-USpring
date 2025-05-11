using UnityEngine;

namespace USpring.Vectors
{
	public struct CustomVector3 : IVector
	{
		public Vector3 Vector3;

		public CustomVector3(Vector3 vector3)
		{
			this.Vector3 = vector3;
		}

		public float this[int index]
		{
			get
			{
				return Vector3[index];
			}
			set
			{
				Vector3[index] = value;
			}
		}

		public int GetSize()
		{
			int res = 3;
			return res;
		}
	}
}