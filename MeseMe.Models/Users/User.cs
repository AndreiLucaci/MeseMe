using System;

namespace MeseMe.Models.Users
{
	[Serializable]
	public class User
	{
		public string Name { get; set; }
		public Guid Id { get; set; }

		public override bool Equals(object obj)
		{
			return obj is User user &&
				   Id.Equals(user.Id);
		}

		protected bool Equals(User other)
		{
			return Id.Equals(other.Id);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override string ToString()
		{
			return $"Name: {Name}, GUID: {Id}";
		}

	}
}
