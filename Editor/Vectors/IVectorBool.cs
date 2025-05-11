namespace USpring.Vectors
{
	public interface IVectorBool
	{
		int GetSize();

		bool this[int index]
		{
			get;
			set;
		}
	}
}