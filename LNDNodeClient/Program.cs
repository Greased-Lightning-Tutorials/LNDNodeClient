using Google.Protobuf;
using Grpc.Core;
using LNDNodeClient.LightningHelpers;
using Lnrpc;
using System;

namespace LNDNodeClient
{
    class Program
    {
        static void Main(string[] args)
        {
    
            Console.WriteLine("Total outbound balance = " + GetChannelOutboundBalance());
            Console.WriteLine("Total inbound balance = " + GetChannelInboundBalance());

            Console.WriteLine("New invoice for 1000 satoshis = " + CreateInvoice(1000).PaymentRequest);
            //NEEDS A INVOICE PAYMENT REQUEST TO WORK
            //SendLightning("lnbcrt500u1pswc2ewpp57kxnqsswh4sr4fwm7nx7hjuymkn30uqj29fcjg85474ruw5gqxwsdqqcqzpgsp5zk5u8jx5vena237mvelv79vynefh4080r6jrzxmaqzle08gr3snq9qyyssqrlf6pttvwzm8xl5r6wkmh626r6vp0fllfawqlksuv6vxtuusp0lph5t8v0mg2429xazt5s3n2vjdnsz7anad9n38407r94v8x96s8ncpws2kat");

            Console.ReadLine();
        }

 
        public static long GetWalletBalance()
        {
            var helpers = new Helpers();
            var client = helpers.GetClient();
            var response = client.WalletBalance(new WalletBalanceRequest(), new Metadata() { new Metadata.Entry("macaroon", helpers.GetMacaroon()) });
            return response.TotalBalance;
        }

        //Return total InboundBalanace to node (All channels)
        public static void SendLightning(string paymentRequest)
        {
            var helpers = new Helpers();
            var client = helpers.GetClient();
            var sendRequest = new SendRequest();
            sendRequest.Amt = 1000;
            sendRequest.PaymentRequest = paymentRequest;
            var response = client.SendPaymentSync(sendRequest, new Metadata() { new Metadata.Entry("macaroon", helpers.GetMacaroon()) });
          
        }


        //Return total InboundBalanace to node (All channels)
        public static ulong GetChannelInboundBalance()
        {
            var helpers = new Helpers();
            var client = helpers.GetClient();
            var channelBalanceRequest = new ChannelBalanceRequest();
            var response = client.ChannelBalance(channelBalanceRequest, new Metadata() { new Metadata.Entry("macaroon", helpers.GetMacaroon()) });
            return response.RemoteBalance.Sat;
        }
        //Return total Outbound balance from node (All channels)
        public static ulong GetChannelOutboundBalance()
        {
            var helpers = new Helpers();
            var client = helpers.GetClient();
            var channelBalanceRequest = new ChannelBalanceRequest();
            var response = client.ChannelBalance(channelBalanceRequest, new Metadata() { new Metadata.Entry("macaroon", helpers.GetMacaroon()) });
            return response.LocalBalance.Sat;
        }
        //Creates new invoice of 1000 sats
        public static AddInvoiceResponse CreateInvoice(long satoshis)
        {
            var helpers = new Helpers();
            var client = helpers.GetClient();
            var invoice = new Invoice();
            invoice.Memo = "A invoice memo";
            //Value in satoshis
            invoice.Value = satoshis;
            var invoiceResponse = client.AddInvoice(invoice, new Metadata() { new Metadata.Entry("macaroon", helpers.GetMacaroon()) });

            return invoiceResponse;
        }
    }
}
