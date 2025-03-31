namespace WebApi.Models
{
    public class EnvironmentConfig
    {
        private static readonly EnvironmentConfig instance = new EnvironmentConfig();

        private EnvironmentConfig() { }

        public static EnvironmentConfig Instance
        {
            get
            {
                return instance;
            }
        }

        public string? SslCertPath { get; set; } = Environment.GetEnvironmentVariable("SSL_CERT_PATH") ;
        public string? SslKeyPath { get; set; } = Environment.GetEnvironmentVariable("SSL_KEY_PATH") ;
        public string? WxAppId { get; set; } = Environment.GetEnvironmentVariable("WX_APP_ID") ;
        public string? WxAppSecret { get; set; } = Environment.GetEnvironmentVariable("WX_APP_SECRET") ;
        public string? TencentcloudSecretId { get; set; } = Environment.GetEnvironmentVariable("TENCENTCLOUD_SECRET_ID") ;
        public string? TencentcloudSecretKey { get; set; } = Environment.GetEnvironmentVariable("TENCENTCLOUD_SECRET_KEY") ;
        public string? TencentcloudSmsAppId { get; set; } = Environment.GetEnvironmentVariable("TENCENTCLOUD_SMS_APP_ID") ;
        public string? TencentcloudSmsCodeTemplateId { get; set; } = Environment.GetEnvironmentVariable("TENCENTCLOUD_SMS_CODE_TEMPLATE_ID") ;
        public string? TencentcloudSmsSignName { get; set; } = Environment.GetEnvironmentVariable("TENCENTCLOUD_SMS_SIGN_NAME") ;
        public string? MongoDbConnectionString { get; set; } = Environment.GetEnvironmentVariable("ConnectionString") ?? "mongodb://localhost:27017";
        public string? JwtKey { get; set; } = Environment.GetEnvironmentVariable("JWT_KEY") ?? "151837e8bf557de925ff631b5520affa";
        public string? Issuer { get; set; } = Environment.GetEnvironmentVariable("ISSUER") ?? "http://localhost:5003/";
        public string? Audience { get; set; } = Environment.GetEnvironmentVariable("AUDIENCE") ?? "http://localhost:5003/";
    }
}
