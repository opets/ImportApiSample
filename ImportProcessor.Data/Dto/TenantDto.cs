using System.ComponentModel;

namespace ImportProcessor.Data.Dto {

	[ImmutableObject( immutable: true )]
	public sealed class TenantDto {

		public int Id { get; }

		public string Name { get; }

		public TenantDto( int id, string name ) {
			Id = id;
			Name = name;
		}

	}
}
