namespace ProjectGym.Exceptions
{

	[Serializable]
	public class MissingIdColumnException : Exception
	{
		public MissingIdColumnException() { }
		public MissingIdColumnException(string message) : base(message) { }
		public MissingIdColumnException(string message, Exception inner) : base(message, inner) { }
	}
}
