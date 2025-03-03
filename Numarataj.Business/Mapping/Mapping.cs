
using AutoMapper;
using Numarataj.DTO.DTOs.AdresTespitDTOs;
using Numarataj.DTO.DTOs.BasePdfDTOs;
using Numarataj.DTO.DTOs.OzelIsyeriDTOs;
using Numarataj.DTO.DTOs.ResmiKurumDTOs;
using Numarataj.DTO.DTOs.SahaCalismasiDTOs;
using Numarataj.DTO.DTOs.YeniBinaDtos;
using Numarataj.Entity.Entities;

namespace Numarataj.Business.Mapping
{
    public class Mapping: Profile
    {
        public Mapping()
        {
            CreateMap<CreateAdresTespitDto, AdresTespit>().ReverseMap();
            CreateMap<UpdateAdresTespitDto, AdresTespit>().ReverseMap();
            CreateMap<ResultAdresTespitDto, AdresTespit>().ReverseMap();
            CreateMap<ResultPdfDto, AdresTespit>().ReverseMap();

            CreateMap<CreateOzelIsyeriDto, OzelIsyeri>().ReverseMap();
            CreateMap<UpdateOzelIsyeriDto, OzelIsyeri>().ReverseMap();
            CreateMap<ResultOzelIsyeriDto, OzelIsyeri>().ReverseMap();
            CreateMap<ResultPdfDto, OzelIsyeri>().ReverseMap();

            CreateMap<CreateResmiKurumDto, ResmiKurum>().ReverseMap();
            CreateMap<UpdateResmiKurumDto, ResmiKurum>().ReverseMap();
            CreateMap<ResultResmiKurumDto, ResmiKurum>().ReverseMap();
            CreateMap<ResultPdfDto, ResmiKurum>().ReverseMap();

            CreateMap<CreateSahaCalismasiDto, SahaCalismasi>().ReverseMap();
            CreateMap<UpdateSahaCalismasiDto, SahaCalismasi>().ReverseMap();
            CreateMap<ResultSahaCalismasiDto, SahaCalismasi>().ReverseMap();
            CreateMap<ResultPdfDto, SahaCalismasi>().ReverseMap();

            CreateMap<CreateYeniBinaDto, YeniBina>().ReverseMap();
            CreateMap<UpdateYeniBinaDto, YeniBina>().ReverseMap();
            CreateMap<ResultYeniBinaDto, YeniBina>().ReverseMap();
            CreateMap<ResultPdfDto, YeniBina>().ReverseMap();



        }
    }
}
