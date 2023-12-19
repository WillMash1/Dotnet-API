using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {

        // private static List<Character> characters = new List<Character> {
        //     new Character(),
        //     new Character { Id = 1, Name = "Sam"}
        // };
        private readonly IMapper _mapper;
        public DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var ServiceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);
            // character.Id = characters.Max(c => c.Id) + 1;
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            ServiceResponse.Data =
            await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return ServiceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try {
            
                var character = 
                await _context.Characters.FirstAsync(C => C.Id == id);
                if(character is null) {
                    throw new Exception($"Chracter with Id '{id}' not found" );
            }

            _context.Characters.Remove(character);

            await _context.SaveChangesAsync();
            
            serviceResponse.Data = await  _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            } catch (Exception e ) {
                serviceResponse.Success = false;
                serviceResponse.message = e.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> getAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters.ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(C => C.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
           var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try {

            var character = 
            await _context.Characters.FirstOrDefaultAsync(C => C.Id == updateCharacter.Id);

            if(character is null) {
                throw new Exception($"Chracter with Id '{updateCharacter.Id}' not found" );
            }

            character.Name = updateCharacter.Name;
            character.Class = updateCharacter.Class;
            character.Defense = updateCharacter.Defense;
            character.HitPoints = updateCharacter.HitPoints;
            character.Intelligence = updateCharacter.Intelligence;
            character.Strength = updateCharacter.Strength;
            
            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            } catch (Exception e ) {
                serviceResponse.Success = false;
                serviceResponse.message = e.Message;
            }
            return serviceResponse;
        }
        
    }
}