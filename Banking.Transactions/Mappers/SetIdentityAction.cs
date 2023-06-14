using AutoMapper;
using System;
using Banking.Framework.Domain;
using Banking.Framework.Types;
using Banking.Transactions.Models;
using Banking.Transactions.Services;
using Banking.Framework.Extensions;

namespace Banking.Transactions.Mappers
{
    public class SetIdentityAction : IMappingAction<TransactionModel, AccountTransaction>
    {
        private readonly IIdentityService _identityService;

        public SetIdentityAction(IIdentityService identityService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        public void Process(TransactionModel source, AccountTransaction destination)
        {
            var identity = _identityService.GetIdentity();

            destination.AccountNumber = identity.AccountNumber;
            destination.Amount = new Money(source.Amount, identity.Currency.TryParseEnum<Currency>());
        }
    }
}
