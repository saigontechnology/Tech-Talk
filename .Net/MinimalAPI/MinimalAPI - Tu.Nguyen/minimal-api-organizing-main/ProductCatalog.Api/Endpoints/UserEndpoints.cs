using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Exceptions;
using ProductCatalog.Api.Filters;
using ProductCatalog.Api.Models;
using ProductCatalog.Api.Repository;
using ProductCatalog.Api.Services;

namespace ProductCatalog.Api.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserGroup(this RouteGroupBuilder group)
    {
        group.MapPost("/register", RegisterAsync)
            .WithName("Register")
            .WithSummary("Register account")
            .WithDescription("Register a new account")
            .Accepts<RegisterRequest>("application/json")
            .AddEndpointFilter<ValidationFilter<RegisterRequest>>();

        group.MapPost("/login", LoginAsync)
            .WithName("Login")
            .WithSummary("Login")
            .WithDescription("Log in to the system")
            .Accepts<LoginRequest>("application/json")
            .Produces<LoginResponse>(200).Produces(400);

        return group;
    }

    private static async Task<Results<Ok<UserInformation>, BadRequest>> RegisterAsync(IUserRepository _userRepository,
        CancellationToken cancellationToken,
        [FromBody] RegisterRequest registerRequest)
    {
        var isUserExists = await _userRepository.CheckUserExistsAsync(registerRequest.UserName, cancellationToken);

        if (isUserExists)
            throw new UserNameExistsException(registerRequest.UserName);

        var userInformation = await _userRepository.RegisterAsync(registerRequest, cancellationToken);

        return TypedResults.Ok(userInformation);
    }
    
    private static async Task<IResult> LoginAsync(IUserRepository _userRepository,
        IMapper _mapper,
        IJwtService _jwtService,
        CancellationToken cancellationToken,
        [FromBody] LoginRequest loginRequest)
    {
        var user = await _userRepository.GetUserByUserName(loginRequest.UserName, cancellationToken);

        if (user is null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user?.Password))
            throw new WrongUserNameOrPassword();

        var loginResponse = new LoginResponse()
        {
            UserInformation = _mapper.Map<UserInformation>(user),
            Token = await _jwtService.GenerateTokenAsync(user.UserName, user.Role.ToString(), cancellationToken)
        };

        return Results.Ok(loginResponse);
    }
}