namespace LazZiya.ExpressLocalization.Messages
{
    /// <summary>
    /// These string values must be present in the resource file
    /// <para>
    /// Original mesages obtained from: <see cref="https://github.com/aspnet/AspNetCore/blob/master/src/Identity/Extensions.Core/src/Resources.resx"/>
    /// </para>
    /// </summary>
    internal struct IdentityErrorMessages
    {
        internal const string ConcurrencyFailure = "Optimistic concurrency failure, object has been modified.";
        internal const string DefaultError = "An unknown failure has occurred.";
        internal const string DuplicateEmail = "Email '{0}' is already taken.";
        internal const string DuplicateUserName = "User name '{0}' is already taken.";
        internal const string InvalidEmail = "Email '{0}' is invalid.";
        internal const string DuplicateRoleName = "Role name '{0}' is already taken.";
        internal const string InvalidRoleName = "Role name '{0}' is invalid.";
        internal const string InvalidToken = "Invalid token.";
        internal const string InvalidUserName = "User name '{0}' is invalid, can only contain letters or digits.";
        internal const string LoginAlreadyAssociated = "A user with this login already exists.";
        internal const string PasswordMismatch = "Incorrect password.";
        internal const string PasswordRequiresDigit = "Passwords must have at least one digit ('0'-'9').";
        internal const string PasswordRequiresLower = "Passwords must have at least one lowercase ('a'-'z').";
        internal const string PasswordRequiresNonAlphanumeric = "Passwords must have at least one non alphanumeric character.";
        internal const string PasswordRequiresUniqueChars = "Passwords must use at least {0} different characters.";
        internal const string PasswordRequiresUpper = "Passwords must have at least one uppercase ('A'-'Z').";
        internal const string PasswordTooShort = "Passwords must be at least {0} characters.";
        internal const string UserAlreadyHasPassword = "User already has a password set.";
        internal const string UserAlreadyInRole = "User already in role '{0}'.";
        internal const string UserNotInRole = "User is not in role '{0}'.";
        internal const string UserLockoutNotEnabled = "Lockout is not enabled for this user.";
        internal const string RecoveryCodeRedemptionFailed = "Recovery code redemption failed.";
    }
}
