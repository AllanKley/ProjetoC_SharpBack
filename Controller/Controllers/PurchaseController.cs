using DTO;

using Microsoft.AspNetCore.Mvc;

namespace Controller.Controllers;

[ApiController]
[Route("[controller]")]
public class PurchaseController : ControllerBase
{



    
    [HttpGet]
    [Route("get/client")]
    public List<object> getClientPurchase(){
        var ClientId = Lib.GetIdFromRequest( Request.Headers["Authorization"].ToString());

        var Purchase = Model.Purchase.FindClientPurchase(ClientId);
    
        return Purchase;
    }





    [HttpGet]
    [Route("get/store/{id}")]
    public object getStorePurchase(int id){

        var Purchase = Model.Purchase.FindStorePurchase(id);
    
        return Purchase;
    }



    [HttpPost]
    [Route("register")]
    public object makePurchase([FromBody] PurchaseDTO purchase)
    {
        var ClientId = Lib.GetIdFromRequest( Request.Headers["Authorization"].ToString());

        var client = Model.Client.find(ClientId);

        purchase.client =client.convertModelToDTO();

        var purchaseModel = Model.Purchase.convertDTOToModel(purchase);

      

        int id = purchaseModel.save();

        return new
        {
            response = "salvou on banco"
        };
    }
}


