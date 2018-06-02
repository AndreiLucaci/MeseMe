using System;
using MeseMe.Models.Users;

namespace MeseMe.MessageListModule.ViewModels
{
	public class SpecialPairViewModel : IEquatable<SpecialPairViewModel>
	{
		public SpecialPairViewModel(User from, User to)
		{
			From = from;
			To = to;
		}

		public User From { get; set; }
		public User To { get; set; }

		public override bool Equals(object obj)
		{
			return obj is SpecialPairViewModel model &&
			       (From.Equals(model.From) &&
			        To.Equals(model.To) ||
			        To.Equals(model.From) &&
			        From.Equals(model.To));
		}

		public bool Equals(SpecialPairViewModel other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return (other.From != null && ((Equals(From, other.From) && Equals(To, other.To)) ||
			                               (Equals(From, other.To) && Equals(To, other.From))));
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((From != null ? From.GetHashCode() : 0) * 397) + ((To != null ? To.GetHashCode() : 0) * 397);
			}
		}

		public static bool operator ==(SpecialPairViewModel left, SpecialPairViewModel right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(SpecialPairViewModel left, SpecialPairViewModel right)
		{
			return !Equals(left, right);
		}
	}
}
