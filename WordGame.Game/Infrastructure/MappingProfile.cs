namespace WordGame.Game.Infrastructure
{
    using AutoMapper;
    using Domain.Models.Challenges;
    using Domain.Models.Players;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Challenge, Dto.Challenge>();
            this.CreateMap<Suggestion, Dto.Suggestion>().ReverseMap();
            this.CreateMap<Player, Dto.PlayerInfo>();
        }
    }
}