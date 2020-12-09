using System;
using System.Collections.Generic;
using Bogus;
using E_Commerce.Data.Entities;

namespace E_Commerce.Data.Fakers
{
    public class UserFaker 
    {
        private readonly Faker<User> _faker;

        public UserFaker()
        {
            _faker = new Faker<User>();
        }

        private Faker<User> SetUpRules() =>
            _faker.RuleFor(u => u.Email , faker => faker.Person.Email.ToLower())
                .RuleFor(u => u.Name , faker => faker.Person.FullName)
                .RuleFor(u => u.Role , f => f.PickRandom<User.Roles>())
                .RuleFor(u => u.Password , "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6") // value is password
                .RuleFor(u => u.Id , f => f.Random.Guid())
                .RuleFor(u => u.CreatedAt , DateTime.Now);

        public User Generate()
        {
            return SetUpRules().Generate();
        }

        public IEnumerable<User> Generate(int count)
        {
            return SetUpRules().Generate(count);
        }
        
    }
}