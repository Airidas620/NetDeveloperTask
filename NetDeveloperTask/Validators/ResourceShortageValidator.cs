using AppData.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDeveloperTask.Validators
{
    public class ResourceShortageValidator : AbstractValidator<ResourceShortage>
    {
        public ResourceShortageValidator()
        {
            RuleFor(r => r.Title).NotNull();

            RuleFor(r => r.Name).NotNull();

            RuleFor(r => r.Room).NotNull().IsInEnum();

            RuleFor(r => r.Category).NotNull().IsInEnum();

            RuleFor(r => r.Priority).NotNull().GreaterThanOrEqualTo(1).LessThanOrEqualTo(10);
        }
    }
}
