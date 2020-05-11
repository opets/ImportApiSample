using System.IO;

namespace ImportProcessor.Domain.Processors {

	public interface IMainProcessor {

		void Import( TextReader reader );

	}

}
