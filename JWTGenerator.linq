<Query Kind="Program">
  <NuGetReference>System.IdentityModel.Tokens.Jwt</NuGetReference>
  <Namespace>System.IdentityModel.Tokens.Jwt</Namespace>
  <Namespace>System.Security.Claims</Namespace>
  <Namespace>Microsoft.IdentityModel.Tokens</Namespace>
</Query>

void Main()
{
	var issuer = "https://limpidfox.com/auth";
	var audience = "https://limpidfox.com/auth";
	var key = Encoding.ASCII.GetBytes
	("!somesecretkeyvalue!!somesecretkeyvalue!!somesecretkeyvalue!!somesecretkeyvalue!");
	var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
	{
		Subject = new ClaimsIdentity(new[]
		{
				new Claim("Id", Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Sub, "SomeUser@limpidfox.com"),
				new Claim(JwtRegisteredClaimNames.Email, "SomeUser@limpidfox.com"),
				new Claim(JwtRegisteredClaimNames.Jti,
				Guid.NewGuid().ToString())
			 }),
		Expires = DateTime.UtcNow.AddMinutes(5),
		Issuer = issuer,
		Audience = audience,
		SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials
		(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
		SecurityAlgorithms.HmacSha512Signature)
	};
	var tokenHandler = new JwtSecurityTokenHandler();
	var token = tokenHandler.CreateToken(tokenDescriptor);
	token.Dump();
}

// You can define other methods, fields, classes and namespaces here
