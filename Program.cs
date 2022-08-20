using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace AWSSecretsManager {
    public class SecretsManager {
        public static string GetSecret(string secretName, string regionName) 
        {
            string secret = "";

            MemoryStream memoryStream = new MemoryStream();
            IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(regionName));
            GetSecretValueRequest request = new GetSecretValueRequest();
            
            request.SecretId = secretName;
            request.VersionStage = "AWSCURRENT"; 
            
            GetSecretValueResponse response = null;
            try 
            {
                response = client.GetSecretValueAsync(request).Result;
            } 
            catch(Exception e)
            {
                throw e;
            }
            // Decrypts secret using the associated KMS key.
            // Depending on whether the secret is a string or binary, one of these fields will be populated.
            if (response.SecretString != null) 
            {
                return secret = response.SecretString;
            } 
            else 
            {
                memoryStream = response.SecretBinary;
                StreamReader reader = new StreamReader(memoryStream);
                string decodedBinarySecret = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
                return decodedBinarySecret;
            }
        }

        public static void Main()
        {
            var secretName = "periodic/dev/rds/admin";
            var regionName = "us-east-1";
            Console.WriteLine(GetSecret(secretName, regionName));
        }
    }
}