using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace testFileUpload.Core.Tests.Helpers
{
    public static class MoqExtensions
    {
        public static IServiceCollection AddMock<T>(
            this IServiceCollection service,
            MockBehavior behavior = MockBehavior.Strict) where T : class
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
