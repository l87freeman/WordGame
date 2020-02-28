namespace WordGame.Game
{
    using AutoMapper;
    using WordGame.Game.Domain.Models;
    using WordGame.Game.Dtos;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            this.CreateMap<Game, GameDto>().ReverseMap();
        }
    }
}