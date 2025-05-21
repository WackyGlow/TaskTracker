using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Domain.ValueObjects
{
    public class Category
    {
        public string Name { get; }

        private Category(string name) 
        {
            Name = name;        
        }

        public static readonly Category Chores = new("Chores");
        public static readonly Category Work = new("Work");
        public static readonly Category Study = new("Study");
        public static readonly Category Errands = new("Errands");
        public static readonly Category Personal = new("Personal");
        public static readonly Category Other = new("Other");

        public static IEnumerable<Category> List() => new[] { Chores, Work, Study, Errands, Personal, Other };

        public static Category FromName(string name)
        {
            var match = List().FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return match ?? throw new ArgumentException($"Invalid category: {name}");
        }

        public override string ToString() => Name;

        public override bool Equals(object? obj) => obj is Category other && Name == other.Name;
        public override int GetHashCode() => Name.GetHashCode();
    }
}