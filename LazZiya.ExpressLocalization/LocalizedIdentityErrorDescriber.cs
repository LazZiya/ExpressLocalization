using Microsoft.AspNetCore.Identity;
using System;
using System.Globalization;
using System.Resources;

namespace LazZiya.ExpressLocalization
{
    public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        private readonly ResourceManager _resMan;

        public LocalizedIdentityErrorDescriber(Type localizationResourceType)
        {
            _resMan = new ResourceManager(localizationResourceType);
        }

        private IdentityError LocalizedIdentityError(string code, params object[] args)
        {
            var msg = _resMan.GetString(code, CultureInfo.CurrentCulture);

            return new IdentityError
            {
                Code = code,
                Description = string.Format(msg, args)
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return LocalizedIdentityError(nameof(DuplicateEmail), email);
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return LocalizedIdentityError(nameof(DuplicateUserName), userName);
        }

        public override IdentityError InvalidEmail(string email)
        {
            return LocalizedIdentityError(nameof(InvalidEmail), email);
        }

        public override IdentityError DuplicateRoleName(string role)
        {
            return LocalizedIdentityError(nameof(DuplicateRoleName), role);
        }

        public override IdentityError InvalidRoleName(string role)
        {
            return LocalizedIdentityError(nameof(InvalidRoleName), role);
        }

        public override IdentityError InvalidToken()
        {
            return LocalizedIdentityError(nameof(InvalidToken));
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return LocalizedIdentityError(nameof(InvalidUserName), userName);
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            return LocalizedIdentityError(nameof(LoginAlreadyAssociated));
        }

        public override IdentityError PasswordMismatch()
        {
            return LocalizedIdentityError(nameof(PasswordMismatch));
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return LocalizedIdentityError(nameof(PasswordRequiresDigit));
        }

        public override IdentityError PasswordRequiresLower()
        {
            return LocalizedIdentityError(nameof(PasswordRequiresLower));
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return LocalizedIdentityError(nameof(PasswordRequiresNonAlphanumeric));
        }

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return LocalizedIdentityError(nameof(PasswordRequiresUniqueChars), uniqueChars);
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return LocalizedIdentityError(nameof(PasswordRequiresUpper));
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return LocalizedIdentityError(nameof(PasswordTooShort), length);
        }

        public override IdentityError UserAlreadyHasPassword()
        {
            return LocalizedIdentityError(nameof(UserAlreadyHasPassword));
        }

        public override IdentityError UserAlreadyInRole(string role)
        {
            return LocalizedIdentityError(nameof(UserAlreadyInRole), role);
        }

        public override IdentityError UserNotInRole(string role)
        {
            return LocalizedIdentityError(nameof(UserNotInRole), role);
        }

        public override IdentityError UserLockoutNotEnabled()
        {
            return LocalizedIdentityError(nameof(UserLockoutNotEnabled));
        }

        public override IdentityError RecoveryCodeRedemptionFailed()
        {
            return LocalizedIdentityError(nameof(RecoveryCodeRedemptionFailed));
        }

        public override IdentityError ConcurrencyFailure()
        {
            return LocalizedIdentityError(nameof(ConcurrencyFailure));
        }

        public override IdentityError DefaultError()
        {
            return LocalizedIdentityError(nameof(DefaultError));
        }
    }
}
