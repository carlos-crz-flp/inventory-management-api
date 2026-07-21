using FluentValidation;
using Inventory.Application.Features.Products.CreateProduct;
using MediatR;
using NetArchTest.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Architecture.Tests
{
    public class ApplicationArchitectureTests
    {
        private static readonly Assembly ApplicationAssembly =
            typeof(CreateProductCommand).Assembly;

        [Fact]
        public void Application_Should_Not_Depend_On_Infrastructure()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .ShouldNot()
                .HaveDependencyOn("Inventory.Infrastructure")
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Application_Should_Not_Depend_On_Api()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .ShouldNot()
                .HaveDependencyOn("Inventory.Api")
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void All_Commands_Should_End_With_Command()
        {
            var invalidTypes = ApplicationAssembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    typeof(IRequest).IsAssignableFrom(t) &&
                    !t.Name.EndsWith("Command"))
                .ToList();

            Assert.Empty(invalidTypes);
        }

        [Fact]
        public void All_Queries_Should_End_With_Query()
        {
            var invalidTypes = ApplicationAssembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    t.GetInterfaces().Any(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IRequest<>)) &&
                    !t.Name.EndsWith("Query") &&
                    !t.Name.EndsWith("Command"))
                .ToList();

            Assert.Empty(invalidTypes);
        }

        [Fact]
        public void All_Handlers_Should_End_With_Handler()
        {
            var invalidTypes = ApplicationAssembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    ImplementsRequestHandler(t) &&
                    !t.Name.EndsWith("Handler"))
                .ToList();

            Assert.Empty(invalidTypes);
        }

        [Fact]
        public void All_Validators_Should_End_With_Validator()
        {
            var invalidTypes = ApplicationAssembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    IsValidator(t) &&
                    !t.Name.EndsWith("Validator"))
                .ToList();

            Assert.Empty(invalidTypes);
        }

        [Fact]
        public void Every_Command_Should_Have_A_Handler()
        {
            var commands = ApplicationAssembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    t.Name.EndsWith("Command"))
                .ToList();

            var handlers = ApplicationAssembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    t.Name.EndsWith("Handler"))
                .Select(t => t.Name.Replace("Handler", ""))
                .ToHashSet();

            var missingHandlers = commands
                .Where(c => !handlers.Contains(c.Name))
                .ToList();

            Assert.Empty(missingHandlers);
        }

        [Fact]
        public void Every_Query_Should_Have_A_Handler()
        {
            var queries = ApplicationAssembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    t.Name.EndsWith("Query"))
                .ToList();

            var handlers = ApplicationAssembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    t.Name.EndsWith("Handler"))
                .Select(t => t.Name.Replace("Handler", ""))
                .ToHashSet();

            var missingHandlers = queries
                .Where(q => !handlers.Contains(q.Name))
                .ToList();

            Assert.Empty(missingHandlers);
        }

        [Fact]
        public void Every_Command_Should_Have_A_Validator()
        {
            var commands = ApplicationAssembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    t.Name.EndsWith("Command"))
                .ToList();

            var validators = ApplicationAssembly
                .GetTypes()
                .Where(IsValidator)
                .Select(v => v.BaseType!.GetGenericArguments()[0])
                .ToHashSet();

            var missingValidators = commands
                .Where(c => !validators.Contains(c))
                .ToList();

            Assert.Empty(missingValidators);
        }

        [Fact]
        public void Every_Command_Handler_Should_Implement_IRequestHandler()
        {
            var handlers = ApplicationAssembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    t.Name.Contains("Command") &&
                    t.Name.EndsWith("Handler"))
                .ToList();

            var invalidHandlers = handlers
                .Where(h => !ImplementsRequestHandler(h))
                .ToList();

            Assert.Empty(invalidHandlers);
        }

        [Fact]
        public void Every_Query_Handler_Should_Implement_IRequestHandler()
        {
            var handlers = ApplicationAssembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    t.Name.Contains("Query") &&
                    t.Name.EndsWith("Handler"))
                .ToList();

            var invalidHandlers = handlers
                .Where(h => !ImplementsRequestHandler(h))
                .ToList();

            Assert.Empty(invalidHandlers);
        }

        private static bool IsValidator(Type type)
        {
            while (type.BaseType != null)
            {
                if (type.BaseType.IsGenericType &&
                    type.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>))
                {
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        }

        private static bool ImplementsRequestHandler(Type type)
        {
            return type
                .GetInterfaces()
                .Any(i =>
                    i.IsGenericType &&
                    (
                        i.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
                        i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)
                    ));
        }
    }
}