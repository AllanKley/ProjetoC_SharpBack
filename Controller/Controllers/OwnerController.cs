using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
namespace Controller.Controllers;

[ApiController]
[Route("[controller]")]
public class OwnerController : ControllerBase
{
    public IConfiguration _configuration; //add

    public OwnerController(IConfiguration config)
    { //add
        _configuration = config;
    }

    [HttpPost]
    [Route("register")]
    public object registerOwner([FromBody] OwnerDTO owner)
    {
        var ownerModel = Model.Owner.convertDTOToModel(owner);
        var document  = ownerModel.save();

        return new
        {
            response = document
        };
    }


    [HttpGet]
    [Route("get")]
    public IActionResult getInformations()
    {
        var OwnerId = Lib.GetIdFromRequest( Request.Headers["Authorization"].ToString());
        var owner = Model.Owner.find(OwnerId);
        
        var clientobj = new{
            name = owner.getName(),
            email = owner.getEmail(),
            date_of_birth = owner.getDateOfBirth(),
            document = owner.getDocument(),
            phone = owner.getPhone(),
            login = owner.getLogin(),
            address = owner.getAddress(),
            passwd = owner.getPasswd()
        };

        return new ObjectResult(clientobj);
    }



    [HttpGet]
    [Route("get/{document}")]
    public object getInformations(String document)
    {

        var owner = Model.Owner.FindByDocument(document);

        return owner;
    }

    [HttpPost]
    [Route("login")]
    public IActionResult checkLogin([FromBody] OwnerDTO ownerDTO)
    {
        var owner = Model.Owner.CheckLogin(ownerDTO.login, ownerDTO.passwd);
        if (owner != null)
        {
            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                new Claim("UserId", owner.Id.ToString()),
                new Claim("UserName", owner.login.ToString()),
                new Claim("Email", owner.email.ToString()),
                new Claim("type", "owner")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: singIn);
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
        else
        {
            return BadRequest();
        }
    }
}
