using System.ComponentModel;

namespace ImportProcessor.Data.Dto {

	[ImmutableObject( immutable: true )]
	public sealed class AccountDto {

		public int Tenant { get; }
		public int Id { get; }

		public string Name { get; }
		public short Type { get; }

		public AccountDto( int tenant, int id, string name, short type ) {
			Id = id;
			Name = name;
			Type = type;
			Tenant = tenant;
		}

	}
}
