using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Codific.Mvc567.FeatureProviders
{
    public class ApplicationControllerFeatureProvider : ControllerFeatureProvider
    {
        public ApplicationControllerFeatureProvider(List<Type> types)
            : base()
        {
            this.TypesToDisable = types;
        }

        public List<Type> TypesToDisable { get; }

        protected override bool IsController(TypeInfo typeInfo)
        {
            var isDisabled = this.TypesToDisable.Select(x => x == typeInfo).Contains(true);
            var isDisabledController = !typeInfo.IsAbstract && isDisabled;
            if (isDisabledController)
            {
                return false;
            }

            return base.IsController(typeInfo);
        }
    }
}
