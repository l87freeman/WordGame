namespace WordGame.Dictionary
{
    using Microsoft.Extensions.Options;

    public class DictionaryConfiguration : IOptions<DictionaryConfiguration>
    {
        public string DictionaryFile { get; set; }

        public string DictionaryFolder { get; set; }

        public DictionaryConfiguration Value => this;
    }
}