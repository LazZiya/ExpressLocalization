namespace LazZiya.ExpressLocalization.DB.Models
{
    /// <summary>
    /// Define the working mode for recursive localization of entries
    /// </summary>
    public enum RecursiveMode
    {
        /// <summary>
        /// Don't run recursive mode
        /// </summary>
        None,

        /// <summary>
        /// Run recursive mode for adding missed resource keys only
        /// </summary>
        KeyOnly,

        /// <summary>
        /// Run ercursive mode for adding missed keys and translations
        /// </summary>
        Full
    }
}
