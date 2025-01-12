namespace Lab
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public string? Address { get; set; }
        public string? Work { get; set; }
    }

    public class PersonRequestDto
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public string? Address { get; set; }
        public string? Work { get; set; }
    }
}
