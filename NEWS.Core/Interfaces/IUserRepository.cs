﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using NEWS.Core.Entities;

namespace NEWS.Core.Interfaces
{
	public interface IUserRepository : IDisposable
	{
		Task<UserIdentity> GetUserByIdAsync(string userId);
		Task<UserIdentity> GetUserByNameAsync(string username);
		Task<IEnumerable<UserIdentity>> GetAllUsersAsync();
		Task CreateAsync(UserIdentity user, string password);
		Task DeleteAsync(UserIdentity user);
		Task UpdateAsync(UserIdentity user);
		bool VerifyUserPassword(string hashedPassword, string providedPassword);
		string HashPassword(string password);
		Task AddUserToRoleAsync(UserIdentity newUser, string p);
		Task<IEnumerable<string>> GetRolesForUserAsync(UserIdentity user);
		Task RemoveUserFromRoleAsync(UserIdentity user, params string[] roleNames);
		Task<UserIdentity> GetLoginUserAsync(string username, string password);
		Task<ClaimsIdentity> CreateIdentityAsync(UserIdentity user);
	}
}
