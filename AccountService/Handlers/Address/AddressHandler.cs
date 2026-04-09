using AccountService.Data;
using AccountService.DTO.Address;
using AccountService.Mappers;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Handlers.Address
{
    public class AddressHandler : IAddressHandler
    {
        private readonly AccountDbContext _context;

        public AddressHandler(AccountDbContext context)
        {
            _context = context;
        }
        public async Task<AddressDTO> CreateAddress(CreateAddressDTO createAddressDTO)
        {
            var existsAddress = await _context.Addresses.FirstOrDefaultAsync(u =>
               u.City == createAddressDTO.City &&
               u.Country == createAddressDTO.Country &&
               u.Street == createAddressDTO.Street &&
               u.StreetNumber == createAddressDTO.StreetNumber &&
               u.PostalCode == createAddressDTO.PostalCode
               );

            if (existsAddress == null)
            {
                Entities.Address address = AddressMapper.ToAddress(createAddressDTO);
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();
                AddressDTO addressDTO = AddressMapper.ToAddressDTO(address);
                return addressDTO;
            }

            return AddressMapper.ToAddressDTO(existsAddress);
        }

        public async Task<List<AddressDTO>> GetAddresses()
        {
            return _context.Addresses
                .Select(AddressMapper.ToAddressDTO)
                .ToList();
        }

        public async Task<AddressDTO?> GetAddressById(int idAddress)
        {
            Entities.Address? address = await _context.Addresses.FindAsync(idAddress);
            
            if (address == null)
            {
                throw new Exception("Address not found");
                
            }
            
            return AddressMapper.ToAddressDTO(address);
        }

        public async Task<AddressDTO> UpdateAddress(UpdateAddressDTO updateAddressDTO, int idAddress)
        {
            Entities.Address? address = await _context.Addresses.FindAsync(idAddress);

            if (address == null)
            {
                throw new Exception("Address not found");
            }

            address.City = updateAddressDTO.City;
            address.Country = updateAddressDTO.Country;
            address.Street = updateAddressDTO.Street;
            address.StreetNumber = updateAddressDTO.StreetNumber;
            address.PostalCode = updateAddressDTO.PostalCode;

            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();

            AddressDTO addressDTO = AddressMapper.ToAddressDTO(address);

            return addressDTO;
        }

        public async Task<bool> DeleteAddress(int idAddress)
        {
            Entities.Address? address = await _context.Addresses.FindAsync(idAddress);

            if (address != null)
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
