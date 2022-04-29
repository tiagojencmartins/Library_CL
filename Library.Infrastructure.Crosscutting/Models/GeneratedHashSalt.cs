namespace Library.Infrastructure.Crosscutting.Models
{
    public class GeneratedHashSalt
    {
        public string Hash { get; }

        public string Salt { get; }

        public GeneratedHashSalt(string hash, string salt)
        {
            Hash = hash;
            Salt = salt;
        }
    }
}