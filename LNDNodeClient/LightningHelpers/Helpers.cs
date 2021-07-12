using Grpc.Core;
using System;

namespace LNDNodeClient.LightningHelpers
{
    public class Helpers
    {
        private readonly string pathToMacaroon = @" PATH TO ADMIN MACAROON ";
        private readonly string pathToSslCertificate = @" PATH TO SSL CERTIFICATE ";
        private readonly string GRPCHost = " GRPC HOST ";
        public Lnrpc.Lightning.LightningClient GetClient()
        {
            var sslCreds = GetSslCredentials();

            //Create channel (Not a lightning channel but a channel to your node)
            var channel = new Grpc.Core.Channel(GRPCHost, sslCreds);
            var client = new Lnrpc.Lightning.LightningClient(channel);
            return client;
        }

        public string GetMacaroon()
        {
            byte[] macaroonBytes = System.IO.File.ReadAllBytes(pathToMacaroon);
            var macaroon = BitConverter.ToString(macaroonBytes).Replace("-", ""); // hex format stripped of "-" chars

            return macaroon;
        }

        public SslCredentials GetSslCredentials()
        {
            // Due to updated ECDSA generated tls.cert we need to let gprc know that
            // we need to use that cipher suite otherwise there will be a handshake
            // error when we communicate with the lnd rpc server.
            System.Environment.SetEnvironmentVariable("GRPC_SSL_CIPHER_SUITES", "HIGH+ECDSA");
            var cert = System.IO.File.ReadAllText(pathToSslCertificate);
            var sslCreds = new SslCredentials(cert);

            return sslCreds;
        }
    }
}
