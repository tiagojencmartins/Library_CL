using Library.Domain.Enums;
using Library.Infrastructure.Crosscutting;
using Library.Infrastructure.Crosscutting.Abstract;
using Library.Infrastructure.Crosscutting.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace CrossCutting.Tests;

public class CrosscuttingTests
{
    private IAuthService _authorizationService;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");

        services.AddAuthorizationDependencies(configuration.Build());

        using var serviceProvider = services.BuildServiceProvider();
        _authorizationService = serviceProvider.GetRequiredService<IAuthService>();
    }

    [Test]
    public void HashSaltEncryption_ValidConversion()
    {
        var data = Guid.NewGuid().ToString();
        var salted = HashSaltHelper.Salt(data);

        Assert.True(HashSaltHelper.ValidateSalt(data, salted));
    }

    [Test]
    public void HashSaltEncryption_InvalidConversion()
    {
        var salted = HashSaltHelper.Salt(Guid.NewGuid().ToString());

        Assert.False(HashSaltHelper.ValidateSalt(Guid.NewGuid().ToString(), salted));
    }

    [Test]
    public void JWT_ValidToken()
    {
        var token = _authorizationService.GenerateToken("tiagojencmartins@gmail.com", UserRoleEnum.Librarian.ToString());

        Assert.True(_authorizationService.ValidateToken(token));
    }
}