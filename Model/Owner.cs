namespace Model;
using Interfaces;
using DAO;
using DTO;
using Microsoft.EntityFrameworkCore;
public class Owner : Person, IValidateDataObject, IDataController<OwnerDTO, Owner>
{
    private static Owner instance;

    private Guid uuid = Guid.NewGuid();
    public List<OwnerDTO> ownerDTO = new List<OwnerDTO>();
    private Owner(Address address) { this.address = address; }
    public Owner()
    {

    }

    public static OwnerResponseDTO ConvertDaoToDTO(DAO.Owner OwnerDAO)
    {
        var OwnerDTO = new OwnerResponseDTO();
        OwnerDTO.name = OwnerDAO.name;
        OwnerDTO.date_of_birth = OwnerDAO.date_of_birth;
        OwnerDTO.document = OwnerDAO.document;
        OwnerDTO.email = OwnerDAO.email;
        OwnerDTO.phone = OwnerDAO.phone;
        OwnerDTO.login = OwnerDAO.login;
        OwnerDTO.passwd = OwnerDAO.passwd;
        OwnerDTO.Id = OwnerDAO.id;

        OwnerDTO.address = Address.ConvertDAOToDTO(OwnerDAO.address);

        return OwnerDTO;
    }

    public static OwnerResponseDTO CheckLogin(string login, string passwd)
    {
        using (var context = new DAOContext())
        {
            var ownerDAO = context.owners.Include(i => i.address).FirstOrDefault(q => q.login == login && q.passwd == passwd);
            if (ownerDAO != null)
            {
                var OwnerDTO = Owner.ConvertDaoToDTO(ownerDAO);
                return OwnerDTO;
            }
            else
            {
                return null;
            }

        }

    }

    public static Owner convertDTOToModel(OwnerDTO obj)
    {
        var owner = new Owner();
        if (obj.address != null)
        {
            owner.address = Address.convertDTOToModel(obj.address);
        }
        else
        {
            owner.address = null;
        }
        owner.name = obj.name;
        owner.date_of_birth = obj.date_of_birth;
        owner.document = obj.document;
        owner.email = obj.email;
        owner.phone = obj.phone;
        owner.login = obj.login;
        owner.passwd = obj.passwd;

        return owner;
    }

    public static Owner find(int id)
    {
        using (var context = new DAOContext())
        {

            var ownerDAO = context.owners.Include(i => i.address).FirstOrDefault(o => o.id == id);
            var address = Address.convertDTOToModel(Address.ConvertDAOToDTO(ownerDAO.address));

            return new Owner
            {
                name = ownerDAO.name,
                email = ownerDAO.email,
                date_of_birth = ownerDAO.date_of_birth,
                document = ownerDAO.document,
                phone = ownerDAO.phone,
                login = ownerDAO.login,
                address = address,
                passwd = ownerDAO.passwd
            };
        }
    }

    public void delete()
    {

    }
    public string save()
    {
        var document = "";

        using (var context = new DAOContext())
        {
            var ExistOwner = context.owners.FirstOrDefault(q => q.document == this.document);
            if (ExistOwner == null)
            {
                var address = new DAO.Address
                {
                    street = this.address.getStreet(),
                    city = this.address.getCity(),
                    state = this.address.getState(),
                    country = this.address.getCountry(),
                    postal_code = this.address.getPostalCode()
                };

                context.addresses.Add(address);

                var owner = new DAO.Owner
                {
                    name = this.name,
                    date_of_birth = this.date_of_birth,
                    document = this.document,
                    email = this.email,
                    phone = this.phone,
                    passwd = this.passwd,
                    login = this.login,
                    address = address
                };

                context.owners.Add(owner);

                context.SaveChanges();


                document = owner.document;

                return document;
            }
            else{
                Console.WriteLine(this.document);
                throw  new System.Exception("Document already register");
            }
        }


    }

    public void update(OwnerDTO obj)
    {

    }

    public OwnerDTO findById(int id)
    {

        return new OwnerDTO();
    }

    public static object FindByDocument(String documentDTO)
    {
        using (var context = new DAOContext())
        {

            var ownerDAO = context.owners.Include(i => i.address).FirstOrDefault(o => o.document == documentDTO);

            return new
            {
                id = ownerDAO.id,
                name = ownerDAO.name,
                email = ownerDAO.email,
                date_of_birth = ownerDAO.date_of_birth,
                document = ownerDAO.document,
                phone = ownerDAO.phone,
                login = ownerDAO.login,
                address = ownerDAO.address,
                passwd = ownerDAO.passwd
            };
        }
    }

    public List<OwnerDTO> getAll()
    {
        return this.ownerDTO;
    }


    public OwnerDTO convertModelToDTO()
    {
        var ownerDTO = new OwnerDTO();

        ownerDTO.name = this.name;

        ownerDTO.date_of_birth = this.date_of_birth;

        ownerDTO.document = this.document;

        ownerDTO.email = this.email;

        ownerDTO.phone = this.phone;

        ownerDTO.login = this.login;

        ownerDTO.address = this.address.convertModelToDTO();

        ownerDTO.passwd = this.passwd;



        return ownerDTO;
    }


    public static Owner getInstance(Address address)
    {
        if (Owner.instance == null)
        {
            Owner.instance = new Owner(address);
        }
        return Owner.instance;
    }

    public Boolean validateObject()
    {

        Console.WriteLine("entrei no validade");
        //if (this.getName() == null) return false;
        // if (this.getDateOfBirth() == null) return false;
        // if (this.getDocument() == null) return false;
        Console.WriteLine(this.getName());
        if (this.getEmail() == null) return false;
        if (this.getPhone() == null) return false;
        if (this.getLogin() == null) return false;
        if (this.getAddress().validateObject() == false) return false;
        Console.WriteLine("cheguei ao final");
        return true;
    }
}
