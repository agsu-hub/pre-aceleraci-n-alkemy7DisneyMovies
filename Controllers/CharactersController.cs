using ChallengeBackendDisney.Data;
using ChallengeBackendDisney.DTOs;
using ChallengeBackendDisney.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChallengeBackendDisney.Controllers
{

    [ApiController]
    [Route ("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly MovieContext _context;

        public CharacterController(MovieContext context)
        {
            this._context = context;
        }


        [HttpGet]
        [Route("/characters")]
        public IActionResult GetCharacterList()
        {
            var characterList = _context.characters.ToList();
            var dtoList = new List<CharacterDto>();
            foreach (var l in characterList)
            {
                var dto = new CharacterDto
                {
                    Image = l.Image,
                    Name = l.Name
                };
                dtoList.Add(dto);



            }
            return Ok(dtoList);
        }


        [HttpGet]
        [Route("/characters/Details")]
        public IActionResult GetCharacterDetail()
        {
            var characterDetail = _context.characters.ToList();
           
            return Ok(characterDetail);
        }


        [HttpGet]
    
        [Route("/characters/byName")]
        public IActionResult GetCharacterByName(string name)
        {
            var charactersByName = _context.characters.ToList();

            if (name == null)
            {

                charactersByName = _context.characters.ToList();


            }
            else
            {
                charactersByName = _context.characters.Where(x => x.Name == name).ToList();
            }
            return Ok(charactersByName);
        }


        [HttpGet]

        [Route("/characters/byMovieId")]
        public IActionResult GetCharacterByMovieId(int movieId)
        {
            var charactersList = _context.characters.ToList();
            var moviesList = _context.movies.ToList();

            if (movieId == 0)
            {

                charactersList = _context.characters.ToList();


            }
            else
            {

                foreach (var mov in moviesList)
                {
                    if(movieId == mov.MovieId)
                    {
                        mov.Characters = charactersList;
                    }
                }
                
            }
            return Ok(charactersList);
        }





        //[HttpGet]

        //[Route("/characters/byAge")]
        //public IActionResult GetCharacterByAge(int age)
        //{
        //    var charactersByAge = _context.characters.ToList();

        //    if (age == 0)
        //    {

        //        charactersByAge = _context.characters.ToList();


        //    }
        //    else
        //    {
        //        age = _context.characters.Where(x => x.Age == age).ToList();
        //    }
        //    return Ok(charactersByAge);
        //}


        [HttpPost]
        [Route("/characters/createCharacter")]

        public async Task<IActionResult> Post(CharacterRequestDto dto)
        {
            var previus = await _context.characters.FirstOrDefaultAsync(x => x.Name == dto.Name);

            if(previus != null)
            {
                return BadRequest($"Character {dto.Name} already exists");
            }

            var charEntity = new Character
            {
                Name = dto.Name,
                Age = dto.Age,
                Weight = dto.Weight,
                History = dto.History,

            };

            if(dto.MovieId.GetValueOrDefault() !=0)
            {
                var movie = _context.movies.FirstOrDefaultAsync(x => x.MovieId == dto.MovieId.Value);

                if(movie != null)
                {

                    charEntity.Movies.Add(await movie);

                }
            }

            await _context.characters.AddAsync(charEntity);

            await _context.SaveChangesAsync();

            return Ok();



        }


        [HttpPut("/charactersUpdate/{id}")]
        public async Task<IActionResult> updateCharacter(long id, Character character)
        {
            if (id != character.CharacterId)
            {
                return BadRequest();
            }

            _context.Entry(character).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CharacterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpDelete("/charactersDelete/{id}")]
        public async Task<IActionResult> DeleteCharacters(int id)
        {
            var characters = await _context.characters.FindAsync(id);

            if (characters == null)
            {
                return NotFound();
            }

            _context.characters.Remove(characters);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CharacterExists(long id) =>
     _context.characters.Any(e => e.CharacterId == id);










    }




}