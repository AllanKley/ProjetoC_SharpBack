using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

using Model.Utils;

namespace Controller.Controllers;


[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{

    public IConfiguration _configuration; //add

    public ClientController(IConfiguration config){ //add
        _configuration = config;
    }

    [HttpPost]
    [Route("register")]
    public IActionResult registerClient([FromBody] ClientDTO client)
    {
        int id;
        var clientModel = Model.Client.convertDTOToModel(client);
        try
        {
            id  = clientModel.save();
        }
        catch (ValidationException ex)
        {   
            return BadRequest(ex.Errors);
        }
        var respose = new ObjectResult(id);
        Response.Headers.Add("Access-Control-Allow-Origin", "*");
        return respose;
    }



    [HttpGet]
    [Route("get")]
    public IActionResult getInformations()
    {
        var ClientId = Lib.GetIdFromRequest( Request.Headers["Authorization"].ToString());
        var client = Model.Client.find(ClientId);
        
        var clientobj = new{
            name = client.getName(),
            email = client.getEmail(),
            date_of_birth = client.getDateOfBirth(),
            document = client.getDocument(),
            phone = client.getPhone(),
            login = client.getLogin(),
            address = client.getAddress(),
            passwd = client.getPasswd()
        };

        return new ObjectResult(clientobj);
    }



    [HttpPost]
    [Route("login")]
    public IActionResult checkLogin([FromBody] ClientDTO client)
    {
        var clientDTO =  Model.Client.findByUser(client.login, client.passwd); // arrumar o metodo
       
        if(clientDTO != null){
            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                new Claim("UserId", clientDTO.Id.ToString()),
                new Claim("UserName", clientDTO.name.ToString()),
                new Claim("Email", clientDTO.email.ToString())

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials:singIn);
                Console.WriteLine(DateTime.Now.AddMinutes(1));
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
        else
        {
            return BadRequest();
        }
        
    }

    [Authorize]
    [HttpGet]
    [Route("verifytoken")]
    public int GetToken(){
        Console.WriteLine(DateTime.Now.AddMinutes(1));
        var rnd = new Random();
        return (rnd.Next(0,1001));
    }
}


