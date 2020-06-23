using LazZiya.ExpressLocalization.Messages;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System;

namespace LazZiya.ExpressLocalization.Identity
{
    /// <summary>
    /// Identity describer errors localizer
    /// </summary>
    public class IdentityErrorsLocalizer : IdentityErrorDescriber 
    {
        private readonly Type ResxResourceType;
        private readonly IStringLocalizer Localizer;

        /// <summary>
        /// Initialize identity errors localization based on resx files
        /// </summary>
        /// <param name="resType"></param>
        public IdentityErrorsLocalizer(Type resType)
        {
            ResxResourceType = resType;
        }
        
        /// <summary>
        /// Initialize identity erroors localization based on DB locailzer
        /// </summary>
        /// <param name="localizer"></param>
        public IdentityErrorsLocalizer(IStringLocalizer localizer)
        {
            Localizer = localizer;
        }

        private IdentityError LocalizedIdentityError(string code, params object[] args)
        {
            var msg = Localizer[code, args];

            return new IdentityError { Code = code, Description = msg };
        }

        /// <summary>
        /// "Email '{0}' is already taken."
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public override IdentityError DuplicateEmail(string email) 
            => LocalizedIdentityError(IdentityErrorMessages.DuplicateEmail, email);

        /// <summary>
        /// "User name '{0}' is already taken."
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override IdentityError DuplicateUserName(string userName) 
            => LocalizedIdentityError(IdentityErrorMessages.DuplicateUserName, userName);

        /// <summary>
        /// "Email '{0}' is invalid."
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public override IdentityError InvalidEmail(string email) 
            => LocalizedIdentityError(IdentityErrorMessages.InvalidEmail, email);

        /// <summary>
        /// "Role name '{0}' is already taken."
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public override IdentityError DuplicateRoleName(string role) 
            => LocalizedIdentityError(IdentityErrorMessages.DuplicateRoleName, role);

        /// <summary>
        /// "Role name '{0}' is invalid."
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public override IdentityError InvalidRoleName(string role) 
            => LocalizedIdentityError(IdentityErrorMessages.InvalidRoleName, role);

        /// <summary>
        /// "Invalid token."
        /// </summary>
        /// <returns></returns>
        public override IdentityError InvalidToken() 
            => LocalizedIdentityError(IdentityErrorMessages.InvalidToken);

        /// <summary>
        /// "User name '{0}' is invalid, can only contain letters or digits."
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override IdentityError InvalidUserName(string userName) 
            => LocalizedIdentityError(IdentityErrorMessages.InvalidUserName, userName);

        /// <summary>
        /// "A user with this login already exists."
        /// </summary>
        /// <returns></returns>
        public override IdentityError LoginAlreadyAssociated() 
            => LocalizedIdentityError(IdentityErrorMessages.LoginAlreadyAssociated);

        /// <summary>
        /// "Incorrect password."
        /// </summary>
        /// <returns></returns>
        public override IdentityError PasswordMismatch() 
            => LocalizedIdentityError(IdentityErrorMessages.PasswordMismatch);

        /// <summary>
        /// "Passwords must have at least one digit ('0'-'9')."
        /// </summary>
        /// <returns></returns>
        public override IdentityError PasswordRequiresDigit() 
            => LocalizedIdentityError(IdentityErrorMessages.PasswordRequiresDigit);

        /// <summary>
        /// "Passwords must have at least one lowercase ('a'-'z')."
        /// </summary>
        /// <returns></returns>
        public override IdentityError PasswordRequiresLower() 
            => LocalizedIdentityError(IdentityErrorMessages.PasswordRequiresLower);

        /// <summary>
        /// "Passwords must have at least one non alphanumeric character."
        /// </summary>
        /// <returns></returns>
        public override IdentityError PasswordRequiresNonAlphanumeric() 
            => LocalizedIdentityError(IdentityErrorMessages.PasswordRequiresNonAlphanumeric);

        /// <summary>
        /// "Passwords must use at least {0} different characters."
        /// </summary>
        /// <param name="uniqueChars"></param>
        /// <returns></returns>
        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) 
            => LocalizedIdentityError(IdentityErrorMessages.PasswordRequiresUniqueChars, uniqueChars);

        /// <summary>
        /// "Passwords must have at least one uppercase ('A'-'Z')."
        /// </summary>
        /// <returns></returns>
        public override IdentityError PasswordRequiresUpper() 
            => LocalizedIdentityError(IdentityErrorMessages.PasswordRequiresUpper);

        /// <summary>
        /// "Passwords must be at least {0} characters."
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public override IdentityError PasswordTooShort(int length) 
            => LocalizedIdentityError(IdentityErrorMessages.PasswordTooShort, length);

        /// <summary>
        /// "User already has a password set."
        /// </summary>
        /// <returns></returns>
        public override IdentityError UserAlreadyHasPassword() 
            => LocalizedIdentityError(IdentityErrorMessages.UserAlreadyHasPassword);

        /// <summary>
        /// "User already in role '{0}'."
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public override IdentityError UserAlreadyInRole(string role) 
            => LocalizedIdentityError(IdentityErrorMessages.UserAlreadyInRole, role);

        /// <summary>
        /// "User is not in role '{0}'."
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public override IdentityError UserNotInRole(string role) 
            => LocalizedIdentityError(IdentityErrorMessages.UserNotInRole, role);

        /// <summary>
        /// "Lockout is not enabled for this user."
        /// </summary>
        /// <returns></returns>
        public override IdentityError UserLockoutNotEnabled() 
            => LocalizedIdentityError(IdentityErrorMessages.UserLockoutNotEnabled);

        /// <summary>
        /// "Recovery code redemption failed."
        /// </summary>
        /// <returns></returns>
        public override IdentityError RecoveryCodeRedemptionFailed() 
            => LocalizedIdentityError(IdentityErrorMessages.RecoveryCodeRedemptionFailed);

        /// <summary>
        /// "Optimistic concurrency failure, object has been modified."
        /// </summary>
        /// <returns></returns>
        public override IdentityError ConcurrencyFailure() 
            => LocalizedIdentityError(IdentityErrorMessages.ConcurrencyFailure);

        /// <summary>
        /// "An unknown failure has occurred."
        /// </summary>
        /// <returns></returns>
        public override IdentityError DefaultError() 
            => LocalizedIdentityError(IdentityErrorMessages.DefaultError);
    }
}
