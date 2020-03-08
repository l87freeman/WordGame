namespace WordGame.Game
{
    using AutoMapper;
    using Domain.Models.Players;
    using Domain.Models.Challenges;

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