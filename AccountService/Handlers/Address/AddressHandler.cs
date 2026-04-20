using AccountService.Data;
using AccountService.DTO.Address;
using AccountService.Mappers;
using AccountService.Middleware;
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
                throw new NotFoundException("Address not found");
                
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

            var existsAddress = await _context.Addresses.FirstOrDefaultAsync(u =>
                   u.City == updateAddressDTO.City &&
                   u.Country == updateAddressDTO.Country &&
                   u.Street == updateAddressDTO.Street &&
                   u.StreetNumber == updateAddressDTO.StreetNumber &&
                   u.PostalCode == updateAddressDTO.PostalCode
           );

            if (existsAddress == null)
            {
                if (updateAddressDTO.City != null)
                    address.City = updateAddressDTO.City;

                if (updateAddressDTO.Country != null)
                    address.Country = updateAddressDTO.Country;

                if (updateAddressDTO.Street != null)
                    address.Street = updateAddressDTO.Street;

                if (updateAddressDTO.StreetNumber != null)
                    address.StreetNumber = updateAddressDTO.StreetNumber.Value;

                if (updateAddressDTO.PostalCode != null)
                    address.PostalCode = updateAddressDTO.PostalCode.Value;

                _context.Addresses.Update(address);
                await _context.SaveChangesAsync();
                AddressDTO addressDTO = AddressMapper.ToAddressDTO(address);

                return addressDTO;
            }

            return AddressMapper.ToAddressDTO(existsAddress);
        }

        public async Task<bool> DeleteAddress(int idAddress)
        {
            Entities.Address? address = await _context.Addresses.FindAsync(idAddress);

            if (address != null)
            {
                try
                {
                    _context.Addresses.Remove(address);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch(DbUpdateException e)
                {
                    throw new DbUpdateException("Address is in use.");
                }
            }

            return false;
        }
    }
}
