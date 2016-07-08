using System;

namespace FluentValidation.Extensions
{
    public class ServiceProviderValidatorFacotry : ValidatorFactoryBase
    {
        private readonly IServiceProvider _serviceProvider;
        public ServiceProviderValidatorFacotry(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public override IValidator CreateInstance(Type validatorType)
        {
            return _serviceProvider.GetService(validatorType) as IValidator;
        }
    }
}
