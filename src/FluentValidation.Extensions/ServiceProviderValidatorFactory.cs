using System;

namespace FluentValidation.Extensions
{
    /// <summary>
    /// Simple implementation of <see cref="IValidatorFactory"/> that uses the provided <see cref="IServiceProvider"/> to create validators.
    /// Most popular IoC containers implement <see cref="IServiceProvider"/> and therefore they can be used.
    /// This type does assume that if a <see cref="IServiceProvider"/> cannot create the requested instance, that <see langword="null"/> is returned.
    /// </summary>
    public class ServiceProviderValidatorFacotry : ValidatorFactoryBase
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes an instance of <see cref="ServiceProviderValidatorFacotry"/> with the provided <see cref="IServiceProvider"/>.  
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ServiceProviderValidatorFacotry(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Uses the provided <see cref="IServiceProvider"/> to create the requested instance. 
        /// </summary>
        /// <param name="validatorType">The validator type to construct</param>
        /// <returns>An instance that implements the requested type. If one cannot be constructed, <see langword="null"/> is returned.</returns>
        public override IValidator CreateInstance(Type validatorType)
        {
            return _serviceProvider.GetService(validatorType) as IValidator;
        }
    }
}
