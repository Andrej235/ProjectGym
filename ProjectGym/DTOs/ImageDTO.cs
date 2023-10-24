﻿namespace ProjectGym.DTOs
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public string ImageURL { get; set; } = null!;
        public bool IsMain { get; set; }
        public string Style { get; set; } = null!;
    }
}
