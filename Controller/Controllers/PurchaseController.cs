using DTO;

using Microsoft.AspNetCore.Mvc;

namespace Controller.Controllers;

[ApiController]
[Route("[controller]")]
public class PurchaseController : ControllerBase
{



    
    [HttpGet]
    [Route("get/client/{document}")]
    public object getClientPurchase(String document){

        var Purchase = Model.Purchase.FindClientPurchase(document);
    
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
        Console.WriteLine(purchase.productsDTO[0].bar_code);

        var purchaseModel = Model.Purchase.convertDTOToModel(purchase);

        

        purchaseModel.setClient(Model.Client.find(ClientId));

        int id = purchaseModel.save();

        return new
        {
            response = "salvou on banco"
        };
    }
}


