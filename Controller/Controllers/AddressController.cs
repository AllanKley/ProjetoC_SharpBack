using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Controller.Controllers;

[ApiController]
[Route("[controller]")]
public class AddressController: ControllerBase
{
   [Authorize]
    [HttpPost]
    [Route("register")]
    public object registerAddress([FromBody] AddressDTO address)
    {

        var addressModel = Model.Address.convertDTOToModel(address);
        int id = addressModel.save();
        return new
        {
            response = "salvou on banco"
        };

    }

    [HttpDelete]
    [Route("Delete")]
    public object removeAddress([FromBody] AddressDTO address)
    {
        var addressModel = Model.Address.convertDTOToModel(address);
        addressModel.delete();

        return new
        {
            status = "ok",
            mensagem = "Excluido com sucesso"
        };
    }


    [HttpPut]
    [Route("update")]
    public object updateAddress([FromBody]AddressDTO address)
    {
        
        var addressModel =  Model.Address.convertDTOToModel(address); 
        
        addressModel.update(address);
        return new { status = "sucess"};
    }

    
}
