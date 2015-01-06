using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tasks.Infrastructure.Validators;
using FluentAssertions;

namespace Tasks.Infrastructure.Tests
{
    [TestClass]
    public class TaskValidatorTest
    {
        private IDomainEntityValidator<DomainModel.Task> _validator = new DomainEntityValidator<DomainModel.Task>();

        [TestMethod]
        public void Title_Empty_ShouldNotBeValid()
        {
            // Arrange
            var entityToValidate = new DomainModel.Task()
            {
                Title = string.Empty,
                EstimatedHours = 10
            };

            // Act
            var isValid = _validator.Validate(entityToValidate);

            // Assert
            isValid.Should().BeFalse("because a Task without a title is not valid");
        }

        [TestMethod]
        public void EstimatedHours_Negative_ShouldNotBeValid()
        {
            // Arrange
            var entityToValidate = new DomainModel.Task()
            {
                Title = "Title",
                EstimatedHours = -1
            };

            // Act
            var isValid = _validator.Validate(entityToValidate);

            // Assert
            isValid.Should().BeFalse("because a Task with a negative estimated hours is not valid");
        }
    }
}
