using System;

namespace WebApplication.Options
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public double TokenLifeTime { get; set; }

    }
}
