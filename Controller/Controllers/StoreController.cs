using DTO;
using Model;
using System;
using Microsoft.AspNetCore.Mvc;
namespace Controller.Controllers;


[ApiController]
[Route("store")]

public class StoreController : ControllerBase
{

    [HttpGet]
    [Route("get/{CNPJ}")]
    public object getStore(String CNPJ){

        var store = Model.Store.find(CNPJ);

        return store;
    }



    [HttpGet]
    [Route("get/all")]
    public List<object> getAllStore(){

        var allStore = Model.Store.findAll();

        return allStore;
    }


    [HttpGet]
    [Route("get/allByOwner/{document}")]
    public List<object> getAllStoreByOwner(string document){

        var allStore = Model.Store.findAllByOwner(document);

        return allStore;
    }

    [HttpPost]
    [Route("register")]
    public object registerStore([FromBody]StoreDTO store){

        var storeModel = Model.Store.convertDTOToModel(store);
        var ownerDTO = Model.Owner.convertDTOToModel(store.owner);
        storeModel.save(Model.Store.GetOwnerId(ownerDTO));

        return new {
                response = "salvou no banco"
        };
    }

}
