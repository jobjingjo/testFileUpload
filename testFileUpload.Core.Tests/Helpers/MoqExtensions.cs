using System;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace testFileUpload.Core.Tests.Helpers
{
    public static class MoqExtensions
    {
        public static IServiceCollection AddMock<T>(
            this IServiceCollection service) where T : class
        {
            var mock = new Mock<T>(MockBehavior.Strict);
            service.AddSingleton(mock);
            service.AddSingleton(mock.Object);
            return service;
        }

        public static IServiceCollection AddMock<T>(
            this IServiceCollection service,
            MockBehavior behavior) where T : class
        {
            var mock = new Mock<T>(behavior);
            service.AddSingleton(mock);
            service.AddSingleton(mock.Object);
            return service;
        }

        public static Mock<T> GetMock<T>(this IServiceProvider provider) where T : class
        {
            return provider.GetRequiredService<Mock<T>>();
        }
    }
}