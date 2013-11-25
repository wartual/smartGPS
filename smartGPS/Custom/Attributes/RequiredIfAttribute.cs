using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace smartGPS.Custom.Attributes
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        private RequiredAttribute innerAttribute = new RequiredAttribute();
        public string DependentUpon { get; set; }
        public object Value { get; set; }

        public RequiredIfAttribute(string dependentUpon, object value)
        {
            this.DependentUpon = dependentUpon;
            this.Value = value;
        }

        public RequiredIfAttribute(string dependentUpon)
        {
            this.DependentUpon = dependentUpon;
            this.Value = null;
        }

        public override bool IsValid(object value)
        {
            return innerAttribute.IsValid(value);
        }
    }

    public class RequiredIfValidator : DataAnnotationsModelValidator<RequiredIfAttribute>
    {
        public RequiredIfValidator(ModelMetadata metadata, ControllerContext context, RequiredIfAttribute attribute)
            : base(metadata, context, attribute)
        { }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            // no client validation - I might well blog about this soon!
            return base.GetClientValidationRules();
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            // get a reference to the property this validation depends upon
            var field = Metadata.ContainerType.GetProperty(Attribute.DependentUpon);

            if (field != null)
            {
                // get the value of the dependent property
                var value = field.GetValue(container, null);

                // compare the value against the target value
                if ((value != null && Attribute.Value == null) || (value != null && value.Equals(Attribute.Value)))
                {
                    // match => means we should try validating this field
                    if (!Attribute.IsValid(Metadata.Model))
                        // validation failed - return an error
                        yield return new ModelValidationResult { Message = ErrorMessage };
                }
            }
        }
    }
}