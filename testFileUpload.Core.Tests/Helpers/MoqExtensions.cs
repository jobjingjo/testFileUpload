using System;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace testFileUpload.Core.Tests.Helpers
{
    public static class MoqExtensions
    {
        public static IServiceCollection AddMock<T>(this IServiceCollection serviceCollection) where T : class
        {
            return serviceCollection.AddMock<T>(MockBehavior.Strict);
        }

        public static IServiceCollection AddMock<T>(this IServiceCollection serviceCollection,
            MockBehavior behavior) where T : class
        {
            var mock = new Mock<T>(behavior);
            serviceCollection.AddSingleton(mock);
            serviceCollection.AddSingleton(mock.Object);
            return serviceCollection;
        }

        public static Mock<T> GetMock<T>(this IServiceProvider provider) where T : class
        {
            return provider.GetRequiredService<Mock<T>>();
        }
    }
}