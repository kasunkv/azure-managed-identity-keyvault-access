namespace ManagedIdentity.KeyVault.Secrets.Models
{
    public class HomeViewModel
    {
        public string SecretValue { get; set; }
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
    }
}
