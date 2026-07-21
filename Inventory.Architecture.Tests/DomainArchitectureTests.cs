using Inventory.Domain.Aggregates.Products;
using NetArchTest.Rules;

namespace Inventory.Architecture.Tests
{
    public class DomainArchitectureTests
    {
        [Fact]
        public void Domain_Should_Not_Depend_On_Application()
        {
            var result = Types.InAssembly(typeof(Product).Assembly)
                .ShouldNot()
                .HaveDependencyOn("Inventory.Application")
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Domain_Should_Not_Depend_On_Infrastructure()
        {
            var result = Types.InAssembly(typeof(Product).Assembly)
                .ShouldNot()
                .HaveDependencyOn("Inventory.Infrastructure")
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Domain_Should_Not_Depend_On_Api()
        {
            var result = Types.InAssembly(typeof(Product).Assembly)
                .ShouldNot()
                .HaveDependencyOn("Inventory.Api")
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Domain_Should_Not_Depend_On_MediatR()
        {
            var result = Types.InAssembly(typeof(Product).Assembly)
                .ShouldNot()
                .HaveDependencyOn("MediatR")
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Domain_Should_Not_Depend_On_FluentValidation()
        {
            var result = Types.InAssembly(typeof(Product).Assembly)
                .ShouldNot()
                .HaveDependencyOn("FluentValidation")
                .GetResult();

            Assert.True(result.IsSuccessful);
        }
    }
}