﻿using Organization.Product.Domain.Authentications.Entities;
using Organization.Product.Domain.Authentications.Repositories;
using Organization.Product.Domain.Common.ValueObjects;
using Organization.Product.Shared.Utils.Hasher;

namespace Organization.Product.Gateways.Authentications
{
    public class Dummy_AppAuthenticatedUserRepository : IAppAuthenticatedUserRepository
    {
        readonly IHasher _hasher;

        public Dummy_AppAuthenticatedUserRepository(IHasher hasher)
        {
            this._hasher = hasher;
        }

        public AppAuthenticatedUser FindBy(string userCd, string password)
        {
            var salt = "this is salt3456";
            var hashed = this._hasher.Hash(password, salt);
            var isValid = userCd == "USER00" && hashed == this._hasher.Hash("USER00", salt);
            if (!isValid) throw AppException.Create(AppMessage.W5001());
            return new AppAuthenticatedUser { UserCd = userCd };
        }
    }
}
