﻿using AutoMapper;
using NewsStacks.Database.Models;
using NewsStacks.DTOs;

namespace NewsStacks.API.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ArticleDTO, Article>();
            CreateMap<ArticleUpdateDTO, Article>();
            CreateMap<Article, ArticleDisplayDTO>();
            CreateMap<RegistrationDTO, User>();
            CreateMap<User, RegistrationDTO>();

        }
    }
}
