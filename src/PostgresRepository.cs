using Npgsql;

namespace Lab
{
    public class PostgresRepository
    {
        private static string Host = "postgres";
        private static string User = "postgres";
        private static string DBname = "postgres";
        private static string Password = "\"postgres\"";
        private static string Port = "5432";

        private string _connectionString;

        public PostgresRepository()
        {
            _connectionString = $"Server={Host};Username={User};Database={DBname};Port={Port};Password={Password};SSLMode=Prefer";
        }

        public void Add(Person person)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            using var command = new NpgsqlCommand("insert into person (name, age, address, work) values (@n1, @n2, @n3, @n4)", conn);

            command.Parameters.AddWithValue("n1", $"'{person.Name}'");
            command.Parameters.AddWithValue("n2", person.Age);
            command.Parameters.AddWithValue("n3", $"'{person.Address}'");
            command.Parameters.AddWithValue("n4", $"'{person.Work}'");
            command.ExecuteNonQuery();
        }

        public Person[] GetAll()
        {
            var persons = new List<Person>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return [];
                }

                using var command = new NpgsqlCommand("select * from person", conn);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var person = new Person()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Age = reader.GetInt32(2),
                        Address = reader.GetString(3),
                        Work = reader.GetString(4),
                    };

                    persons.Add(person);
                }

                reader.Close();
            }

            return persons.ToArray();
        }
    }
}
