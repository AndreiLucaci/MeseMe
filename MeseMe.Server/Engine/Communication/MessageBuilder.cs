using System.Text;
using Newtonsoft.Json;

namespace MeseMe.Server.Engine.Communication
{
	public class MessageBuilder
	{
		private readonly StringBuilder _stringBuilder;

		public MessageBuilder()
		{
			_stringBuilder = new StringBuilder();
		}

		public void AddPart(byte[] bytes)
		{
			var messagePart = Encoding.UTF8.GetString(bytes);
			_stringBuilder.Append(messagePart);
		}

		public T To<T>() 
			where T: class
		{ 
			var str = _stringBuilder.ToString();

			var obj = JsonConvert.DeserializeObject<T>(str);

			return obj;
		}
	}
}
