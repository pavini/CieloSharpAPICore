using System;
using System.Linq;
using Cielo.Core.Configurations;
using Cielo.Core.Enums;
using Cielo.Core.Exceptions;
using Cielo.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cielo.Core.Tests
{
    [TestClass]
    public class CieloApiTest
    {
        private ICieloApi _api;
        private DateTime _validExpirationDate;
        private DateTime _invalidExpirationDate;

        [TestInitialize]
        public void ConfigEnvironment()
        {
            _api = new CieloApi(CieloEnvironment.Sandbox, Merchant.Sandbox);
            _validExpirationDate = new DateTime(DateTime.Now.Year + 1, 12, 1);
            _invalidExpirationDate = new DateTime(DateTime.Now.Year - 1, 12, 1);
        }

        [TestMethod]
        public void CriaUmaAutorizacaoResultadoAutorizada()
        {
            var customer = new Customer(name: "Fulano da Silva");

            var creditCard = new CreditCard(
                cardNumber: SandboxCreditCard.Authorized1,
                holder: "Teste Holder",
                expirationDate: _validExpirationDate,
                securityCode: "123",
                brand: CardBrand.Visa);

            var payment = new Payment(
                amount: 150.00M,
                currency: Currency.BRL,
                installments: 1,
                capture: false,
                softDescriptor: "DOTNETPROJECT",
                creditCard: creditCard);

            var merchantOrderId = new Random().Next();

            var transaction = new Transaction(
                merchantOrderId: merchantOrderId.ToString(),
                customer: customer,
                payment: payment);

            var returnTransaction = _api.CreateTransaction(Guid.NewGuid(), transaction);

            Assert.IsTrue(returnTransaction.Payment.Status == Status.Authorized, "Transação não foi autorizada");
        }

        [TestMethod]
        public void CriaUmaTransacaoCapturadaResultadoPagamentoConfirmado()
        {
            var customer = new Customer(name: "Fulano da Silva");

            var creditCard = new CreditCard(
                cardNumber: SandboxCreditCard.Authorized1,
                holder: "Teste Holder",
                expirationDate: _validExpirationDate,
                securityCode: "123",
                brand: CardBrand.Visa);

            var payment = new Payment(
                amount: 2500,
                currency: Currency.BRL,
                installments: 1,
                capture: true,
                softDescriptor: "DOTNETPROJECT",
                creditCard: creditCard);

            var merchantOrderId = new Random().Next();

            var transaction = new Transaction(
                merchantOrderId: merchantOrderId.ToString(),
                customer: customer,
                payment: payment);

            var returnTransaction = _api.CreateTransaction(Guid.NewGuid(), transaction);

            Assert.IsTrue(returnTransaction.Payment.Status == Status.PaymentConfirmed, "Transação não teve pagamento confirmado");
        }

        [TestMethod]
        public void CriaUmaTransacaoCapturadaComCartaoNaoAutorizadaResultadoNaoAutorizada()
        {
            var customer = new Customer(name: "Fulano da Silva");

            var creditCard = new CreditCard(
                cardNumber: SandboxCreditCard.NotAuthorized,
                holder: "Teste Holder",
                expirationDate: _validExpirationDate,
                securityCode: "123",
                brand: CardBrand.Visa);

            var payment = new Payment(
                amount: 150.00M,
                currency: Currency.BRL,
                installments: 1,
                capture: true,
                softDescriptor: "DOTNETPROJECT",
                creditCard: creditCard);

            var merchantOrderId = new Random().Next();

            var transaction = new Transaction(
                merchantOrderId: merchantOrderId.ToString(),
                customer: customer,
                payment: payment);

            var returnTransaction = _api.CreateTransaction(Guid.NewGuid(), transaction);

            Assert.IsTrue(returnTransaction.Payment.Status == Status.Denied, "Transação não foi negada");
        }

        [TestMethod]
        public void CriaUmaTransacaoCapturadaComCartaoBloqueadoResultadoNaoAutorizada()
        {
            var customer = new Customer(name: "Fulano da Silva");

            var creditCard = new CreditCard(
                cardNumber: SandboxCreditCard.NotAuthorizedCardBlocked,
                holder: "Teste Holder",
                expirationDate: _validExpirationDate,
                securityCode: "123",
                brand: CardBrand.Visa);

            var payment = new Payment(
                amount: 150.00M,
                currency: Currency.BRL,
                installments: 1,
                capture: true,
                softDescriptor: "DOTNETPROJECT",
                creditCard: creditCard);

            var merchantOrderId = new Random().Next();

            var transaction = new Transaction(
                merchantOrderId: merchantOrderId.ToString(),
                customer: customer,
                payment: payment);

            var returnTransaction = _api.CreateTransaction(Guid.NewGuid(), transaction);

            Assert.IsTrue(returnTransaction.Payment.Status == Status.Denied, "Transação não foi negada");
        }

        [TestMethod]
        public void CriaUmaTransacaoCapturadaComCartaoCanceladoResultadoNaoAutorizada()
        {
            var customer = new Customer(name: "Fulano da Silva");

            var creditCard = new CreditCard(
                cardNumber: SandboxCreditCard.NotAuthorizedCardCanceled,
                holder: "Teste Holder",
                expirationDate: _validExpirationDate,
                securityCode: "123",
                brand: CardBrand.Visa);

            var payment = new Payment(
                amount: 150.00M,
                currency: Currency.BRL,
                installments: 1,
                capture: true,
                softDescriptor: "DOTNETPROJECT",
                creditCard: creditCard);

            var merchantOrderId = new Random().Next();

            var transaction = new Transaction(
                merchantOrderId: merchantOrderId.ToString(),
                customer: customer,
                payment: payment);

            var returnTransaction = _api.CreateTransaction(Guid.NewGuid(), transaction);

            Assert.IsTrue(returnTransaction.Payment.Status == Status.Denied, "Transação não foi negada");
        }

        [TestMethod]
        public void CriaUmaTransacaoCapturadaComCartaoExpiradoResultadoNaoAutorizada()
        {
            var customer = new Customer(name: "Fulano da Silva");

            var creditCard = new CreditCard(
                cardNumber: SandboxCreditCard.NotAuthorizedCardExpired,
                holder: "Teste Holder",
                expirationDate: _invalidExpirationDate,
                securityCode: "123",
                brand: CardBrand.Visa);

            var payment = new Payment(
                amount: 150.00M,
                currency: Currency.BRL,
                installments: 1,
                capture: true,
                softDescriptor: "DOTNETPROJECT",
                creditCard: creditCard);

            var merchantOrderId = new Random().Next();

            var transaction = new Transaction(
                merchantOrderId: merchantOrderId.ToString(),
                customer: customer,
                payment: payment);

            try
            {
                var returnTransaction = _api.CreateTransaction(Guid.NewGuid(), transaction);

                Assert.IsTrue(returnTransaction.Payment.Status == Status.Denied, "Transação não foi negada");
            }
            catch (CieloException e)
            {
                Assert.IsTrue(e.CieloErrors.Any(i => i.Code == 126));
            }
        }

        [TestMethod]
        public void CriaUmaTransacaoCapturadaComCartaoComProblemasResultadoNaoAutorizada()
        {
            var customer = new Customer(name: "Fulano da Silva");

            var creditCard = new CreditCard(
                cardNumber: SandboxCreditCard.NotAuthorizedCardProblems,
                holder: "Teste Holder",
                expirationDate: _validExpirationDate,
                securityCode: "123",
                brand: CardBrand.Visa);

            var payment = new Payment(
                amount: 150.00M,
                currency: Currency.BRL,
                installments: 1,
                capture: true,
                softDescriptor: "DOTNETPROJECT",
                creditCard: creditCard);

            var merchantOrderId = new Random().Next();

            var transaction = new Transaction(
                merchantOrderId: merchantOrderId.ToString(),
                customer: customer,
                payment: payment);

            var returnTransaction = _api.CreateTransaction(Guid.NewGuid(), transaction);

            Assert.IsTrue(returnTransaction.Payment.Status == Status.Denied, "Transação não foi negada");
        }

        [TestMethod]
        public void CriaUmaTransacaoCapturadaComCartaoDeTimeOutInternoCieloResultadoNaoAutorizada()
        {
            var customer = new Customer(name: "Fulano da Silva");

            var creditCard = new CreditCard(
                cardNumber: SandboxCreditCard.NotAuthorizedTimeOut,
                holder: "Teste Holder",
                expirationDate: _validExpirationDate,
                securityCode: "123",
                brand: CardBrand.Visa);

            var payment = new Payment(
                amount: 150.00M,
                currency: Currency.BRL,
                installments: 1,
                capture: true,
                softDescriptor: "DOTNETPROJECT",
                creditCard: creditCard);

            var merchantOrderId = new Random().Next();

            var transaction = new Transaction(
                merchantOrderId: merchantOrderId.ToString(),
                customer: customer,
                payment: payment);

            var returnTransaction = _api.CreateTransaction(Guid.NewGuid(), transaction);

            Assert.IsTrue(returnTransaction.Payment.Status == Status.Denied, "Transação não foi negada");
        }

        [TestMethod]
        public void CriaUmaAutorizacaoDepoisCapturaResultadoPagamentoConfirmado()
        {
            var customer = new Customer(name: "Fulano da Silva");

            var creditCard = new CreditCard(
                cardNumber: SandboxCreditCard.Authorized1,
                holder: "Teste Holder",
                expirationDate: _validExpirationDate,
                securityCode: "123",
                brand: CardBrand.Visa);

            var payment = new Payment(
                amount: 150.00M,
                currency: Currency.BRL,
                installments: 1,
                capture: false,
                softDescriptor: "DOTNETPROJECT",
                creditCard: creditCard);

            var merchantOrderId = new Random().Next();

            var transaction = new Transaction(
                merchantOrderId: merchantOrderId.ToString(),
                customer: customer,
                payment: payment);

            var createTransaction = _api.CreateTransaction(Guid.NewGuid(), transaction);
            var captureTransaction = _api.CaptureTransaction(Guid.NewGuid(), createTransaction.Payment.PaymentId.Value);

            Assert.IsTrue(captureTransaction.Status == Status.PaymentConfirmed, "Captura não teve pagamento confirmado");
        }

        [TestMethod]
        public void CriaUmaAutorizacaoDepoisCapturaDepoisCancelaResultadoCancelado()
        {
            var customer = new Customer(name: "Fulano da Silva");

            var creditCard = new CreditCard(
                cardNumber: SandboxCreditCard.Authorized1,
                holder: "Teste Holder",
                expirationDate: _validExpirationDate,
                securityCode: "123",
                brand: CardBrand.Visa);

            var payment = new Payment(
                amount: 150.00M,
                currency: Currency.BRL,
                installments: 1,
                capture: false,
                softDescriptor: "DOTNETPROJECT",
                creditCard: creditCard);

            var merchantOrderId = new Random().Next();

            var transaction = new Transaction(
                merchantOrderId: merchantOrderId.ToString(),
                customer: customer,
                payment: payment);

            var createTransaction = _api.CreateTransaction(Guid.NewGuid(), transaction);
            var captureTransaction = _api.CaptureTransaction(Guid.NewGuid(), createTransaction.Payment.PaymentId.Value);
            var cancelationTransaction = _api.CancellationTransaction(Guid.NewGuid(), createTransaction.Payment.PaymentId.Value);

            Assert.IsTrue(cancelationTransaction.Status == Status.Voided, "Cancelamento não teve sucesso");
        }

        [TestMethod]
        public void CriaUmaAutorizacaoDepoisCapturaParcialResultadoPagamentoAprovado()
        {
            var customer = new Customer(name: "Fulano da Silva");

            var creditCard = new CreditCard(
                cardNumber: SandboxCreditCard.Authorized1,
                holder: "Teste Holder",
                expirationDate: _validExpirationDate,
                securityCode: "123",
                brand: CardBrand.Visa);

            var payment = new Payment(
                amount: 150.25M,
                currency: Currency.BRL,
                installments: 1,
                capture: false,
                softDescriptor: "DOTNETPROJECT",
                creditCard: creditCard);

            var merchantOrderId = new Random().Next();

            var transaction = new Transaction(
                merchantOrderId: merchantOrderId.ToString(),
                customer: customer,
                payment: payment);

            var createTransaction = _api.CreateTransaction(Guid.NewGuid(), transaction);
            var captureTransaction = _api.CaptureTransaction(Guid.NewGuid(), createTransaction.Payment.PaymentId.Value, 25.00M);

            Assert.IsTrue(captureTransaction.Status == Status.PaymentConfirmed, "Transação não teve pagamento aprovado");
        }

        [TestMethod]
        public void CriaUmaAutorizacaoComTokenizacaoDoCartaoResultadoComToken()
        {
            var customer = new Customer(name: "Fulano da Silva");

            var creditCard = new CreditCard(
                cardNumber: SandboxCreditCard.Authorized2,
                holder: "Teste Holder",
                expirationDate: _validExpirationDate,
                securityCode: "123",
                brand: CardBrand.Visa,
                saveCard: true);

            var payment = new Payment(
                amount: 157.37M,
                currency: Currency.BRL,
                installments: 1,
                capture: false,
                softDescriptor: "DOTNETPROJECT",
                creditCard: creditCard);

            var merchantOrderId = new Random().Next();

            var transaction = new Transaction(
                merchantOrderId: merchantOrderId.ToString(),
                customer: customer,
                payment: payment);

            var createTransaction = _api.CreateTransaction(Guid.NewGuid(), transaction);

            Assert.IsNotNull(createTransaction.Payment.CreditCard.CardToken, "Não foi criado o token");
        }

        [TestMethod]
        public void CriaUmaTransacaoCapturadaComTokenizacaoDoCartaoResultadoComToken()
        {
            var customer = new Customer(name: "Fulano da Silva");

            var creditCard = new CreditCard(
                cardNumber: SandboxCreditCard.Authorized2,
                holder: "Teste Holder",
                expirationDate: _validExpirationDate,
                securityCode: "123",
                brand: CardBrand.Visa,
                saveCard: true);

            var payment = new Payment(
                amount: 150.00M,
                currency: Currency.BRL,
                installments: 1,
                capture: true,
                softDescriptor: "DOTNETPROJECT",
                creditCard: creditCard);

            var merchantOrderId = new Random().Next();

            var transaction = new Transaction(
                merchantOrderId: merchantOrderId.ToString(),
                customer: customer,
                payment: payment);

            var createTransaction = _api.CreateTransaction(Guid.NewGuid(), transaction);

            Assert.IsNotNull(createTransaction.Payment.CreditCard.CardToken, "Não foi criado o token");
        }

        [TestMethod]
        public void CriaUmaTransacaoRecorrenteAgendadaParaOProximoMesResultadoPagamentoProgramado()
        {
            var customer = new Customer(name: "Fulano da Silva");

            var creditCard = new CreditCard(
                cardNumber: SandboxCreditCard.Authorized2,
                holder: "Teste Holder",
                expirationDate: _validExpirationDate,
                securityCode: "123",
                brand: CardBrand.Visa,
                saveCard: true);

            var recurrentPayment = new RecurrentPayment(
                interval: Interval.Monthly,
                startDate: DateTime.Now.AddMonths(1),
                endDate: DateTime.Now.AddMonths(7));

            var payment = new Payment(
                amount: 150.00M,
                currency: Currency.BRL,
                installments: 1,
                softDescriptor: "DOTNETPROJECT",
                creditCard: creditCard,
                recurrentPayment: recurrentPayment);

            var merchantOrderId = new Random().Next();

            var transaction = new Transaction(
                merchantOrderId: merchantOrderId.ToString(),
                customer: customer,
                payment: payment);

            var createTransaction = _api.CreateTransaction(Guid.NewGuid(), transaction);

            Assert.IsTrue(createTransaction.Payment.Status == Status.Scheduled, "Recorrência não foi programada");
        }

        [TestMethod]
        public void CriaUmaTransacaoRecorrenteParaAgoraResultadoPagamentoAutorizado()
        {
            var customer = new Customer(name: "Fulano da Silva");

            var creditCard = new CreditCard(
                cardNumber: SandboxCreditCard.Authorized2,
                holder: "Teste Holder",
                expirationDate: _validExpirationDate,
                securityCode: "123",
                brand: CardBrand.Visa,
                saveCard: true);

            var recurrentPayment = new RecurrentPayment(
                interval: Interval.Monthly,
                endDate: DateTime.Now.AddMonths(6));

            var payment = new Payment(
                amount: 150.00M,
                currency: Currency.BRL,
                installments: 1,
                softDescriptor: "DOTNETPROJECT",
                creditCard: creditCard,
                recurrentPayment: recurrentPayment);

            var merchantOrderId = new Random().Next();

            var transaction = new Transaction(
                merchantOrderId: merchantOrderId.ToString(),
                customer: customer,
                payment: payment);

            var createTransaction = _api.CreateTransaction(Guid.NewGuid(), transaction);

            Assert.IsTrue(createTransaction.Payment.Status == Status.Authorized, "Recorrência não foi autorizada");
        }
    }
}
