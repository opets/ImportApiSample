namespace ImportProcessor.Domain.Entities {

	public sealed class Account {

		public int Tenant { get; }
		public int Id { get; }

		public string Name { get; }
		public short Type { get; }

		public Account( int tenant, int id, string name, short type ) {
			Id = id;
			Name = name;
			Type = type;
			Tenant = tenant;
		}

	}
}
