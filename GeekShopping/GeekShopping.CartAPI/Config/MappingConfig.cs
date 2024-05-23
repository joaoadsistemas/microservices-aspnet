﻿using AutoMapper;
using GeekShopping.CartAPI.DTOs;
using GeekShopping.CartAPI.Entities;

namespace GeekShopping.CartAPI.Config
{
    public class MappingConfig
    {

        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config => {
                config.CreateMap<ProductDTO, Product>().ReverseMap();
                config.CreateMap<CartHeaderDTO, CartHeader>().ReverseMap();
                config.CreateMap<CartDetailDTO, CartDetail>().ReverseMap();
                config.CreateMap<CartDTO, Cart>().ReverseMap();
            });
            return mappingConfig;
        }

    }
}
