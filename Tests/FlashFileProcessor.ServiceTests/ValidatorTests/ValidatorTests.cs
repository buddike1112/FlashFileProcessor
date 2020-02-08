using AutoFixture;
using FlashFileProcessor.Service.Helpers;
using FlashFileProcessor.Service.Interfaces;
using FlashFileProcessor.Service.Options;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace FlashFileProcessor.ServiceTests.ValidatorTests
{
   public class ValidatorTests
   {
      private IFixture Fixture { get; } = new Fixture();
      public IOptionsMonitor<FilesOptions> files { get; set; }
      public IRuleProcessor rules { get; set; }
      public IValidator validator { get; set; }
      public ILogger _logger { get; set; }

      public ValidatorTests()
      {
         FilesOptions fileOptions = new FilesOptions() { Columns = new string[] { "" } };
         files = Mock.Of<IOptionsMonitor<FilesOptions>>(_ => _.CurrentValue == fileOptions);
         _logger = Substitute.For<ILogger<Validator>>();
         rules = Substitute.For<IRuleProcessor>();
         Fixture.Register<IValidator>(() => Substitute.For<Validator>(files, rules, _logger));
         validator = Fixture.Create<IValidator>();
      }

      [Theory]
      [InlineData("^+[0-9]*$", "0123456789")]
      [InlineData("^[0-9]{3}$", "123")]
      public void CheckProcessingOfValidateAsyncMethodForValidItems(string regExpression, string input)
      {
         // Arrange

         // Act
         Task<bool> result = validator.ValidateAsync(regExpression, input);

         // Assert
         result.Should().NotBeNull();
         result.Result.Should().Be(true);
      }

      [Theory]
      [InlineData("^+[0-9]*$", "abcdefg")]
      [InlineData("^[0-9]{3}$", "abc")]
      [InlineData("^[0-9]{3}$", "123456789")]
      public void CheckProcessingOfValidateAsyncMethodForInvalidItems(string regExpression, string input)
      {
         // Arrange

         // Act
         Task<bool> result = validator.ValidateAsync(regExpression, input);

         // Assert
         result.Should().NotBeNull();
         result.Result.Should().Be(false);
      }
   }
}