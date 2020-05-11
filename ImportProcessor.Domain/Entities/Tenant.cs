namespace ImportProcessor.Domain.Entities {

	public sealed class Tenant {

		public int Id { get; }
		public string Name { get; }

		public Tenant( int id, string name ) {
			Id = id;
			Name = name;
		}

	}
}
