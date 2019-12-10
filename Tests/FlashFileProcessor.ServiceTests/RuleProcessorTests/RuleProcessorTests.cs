using AutoFixture;
using FlashFileProcessor.Service.Helpers;
using FlashFileProcessor.Service.Interfaces;
using FlashFileProcessor.Service.Models;
using FlashFileProcessor.Service.Options;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FlashFileProcessor.ServiceTests.RuleProcessorTests
{
   public class RuleProcessorTests
   {
      private IFixture Fixture { get; } = new Fixture();
      public IOptionsMonitor<FilesOptions> files { get; set; }
      public IRuleProcessor rules { get; set; }

      public RuleProcessorTests()
      {
         FilesOptions fileOptions = new FilesOptions() { 
            Columns = new string[] { "PhoneNumber;^+[0-9]*$;Invalid Phone Number", "AccountNumber;^[0-9]{3}$;Invalid Account Number" },
            Profiles = new ProfilesOptions[] { new ProfilesOptions() { Name = "AProf01", Validations = new string[] { "PhoneNumber", "AccountNumber" } } }
         
         };
         files = Mock.Of<IOptionsMonitor<FilesOptions>>(_ => _.CurrentValue == fileOptions);
         Fixture.Register<IRuleProcessor>(() => Substitute.For<RuleProcessor>(files));
         rules = Fixture.Create<IRuleProcessor>();
      }

      [Theory]
      [InlineData("PhoneNumber", "Invalid Phone Number")]
      [InlineData("AccountNumber", "Invalid Account Number")]
      public void ValidateOnGettingSpecificRuleByFieldName(string fieldName, string rejectReason)
      {
         // Arrange

         // Act
         Rule rule = rules.GetRule(fieldName);

         // Assert
         rule.Should().NotBeNull();
         rule.Field.Should().NotBeNullOrEmpty();
         rule.RejectReason.Should().Be(rejectReason);
      }

      [Fact]
      public void ValidateOnGettingAllRules()
      {
         // Arrange

         // Act
         List<Rule> actualRules = rules.GetRules();

         // Assert
         actualRules.Should().NotBeNull();
         actualRules.Count.Should().BeGreaterThan(0);
      }
   }
}
