using System;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Entities
{
    public class IdUserRole : IdentityUserRole<string>
    {
        [BsonId]
        public Guid Id { get; set; }
    }

    public class IdRole : IdentityRole<string>
    {
        [BsonId]
        public Guid IdNosql { get; set; }
    }

    public class IdUser : IdentityUser<string>
    {
        [BsonId]
        public Guid IdNosql { get; set; }
    }
}