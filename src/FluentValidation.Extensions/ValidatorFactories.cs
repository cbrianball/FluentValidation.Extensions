namespace FluentValidation.Extensions
{
    /// <summary>
    /// A static class used to hold the instance of the Default <see cref="IValidatorFactory"/> for the current <see cref="System.AppDomain"/>. 
    /// </summary>
    public static class ValidatorFactories
    {
        /// <summary>
        /// The instance of <see cref="IValidatorFactory" used by <see cref="FluentValidation.Extensions"/> when one is not passed in to a method that requires one.
        /// </summary>
        public static IValidatorFactory Default { get; set; }
    }
}
