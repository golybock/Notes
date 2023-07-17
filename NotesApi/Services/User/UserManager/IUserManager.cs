using System.Security.Claims;
using Blank.User;
using Domain.User;
using DomainBuilder.User;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Services.User.UserManager;

public interface IUserManager
{
    public Task<UserDomain?> Get(string email);

    public Task<UserDomain?> Get(Guid id);
    
    public Task<UserDomain?> Get(ClaimsPrincipal claimsPrincipal);

    public Task<Guid> Create(UserBlank userBlank, string hashedPassword);
    
    public Task Update(Guid id, UserBlank userBlank);
}