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
            var invoiceResponse = CreateInvoice();
            Console.WriteLine(invoiceResponse.PaymentRequest);
            Console.ReadLine();
        }


        public static AddInvoiceResponse CreateInvoice()
        {
            var helpers = new Helpers();
            var client = helpers.GetClient();
            var invoice = new Invoice();
            invoice.Memo = "Satoshis Talk Post";
            //Value in satoshis
            invoice.Value = 1000;
            var invoiceResponse = client.AddInvoice(invoice, new Metadata() { new Metadata.Entry("macaroon", helpers.GetMacaroon()) });

            return invoiceResponse;
        }
    }
}
