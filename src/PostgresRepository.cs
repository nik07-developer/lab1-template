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

        public int Add(PersonRequestDto person)
        {
            person.Age ??= 18;
            person.Address ??= string.Empty;
            person.Work ??= string.Empty;

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var command = new NpgsqlCommand("insert into person (name, age, address, work) values (@n1, @n2, @n3, @n4) returning id", conn);

            command.Parameters.AddWithValue("n1", $"{person.Name}");
            command.Parameters.AddWithValue("n2", person.Age);
            command.Parameters.AddWithValue("n3", $"{person.Address}");
            command.Parameters.AddWithValue("n4", $"{person.Work}");

            int id = (int)command.ExecuteScalar();

            conn.Close();

            return id;
        }

        public List<Person> GetAll()
        {
            var persons = new List<Person>();
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

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
            conn.Close();

            return persons;
        }

        public bool TryGet(int id, out Person person)
        {
            person = null;
            bool isFound = false;

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var command = new NpgsqlCommand($"select * from person where id = {id}", conn);
            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                person = new Person()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Age = reader.GetInt32(2),
                    Address = reader.GetString(3),
                    Work = reader.GetString(4),
                };

                isFound = true;
            }

            reader.Close();
            return isFound;
        }

        public void Delete(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var command = new NpgsqlCommand($"delete from person where id = {id}", conn);
            command.ExecuteNonQuery();

            conn.Close();
        }

        public bool Update(int id, PersonRequestDto person, out Person updated)
        {
            bool isFound = false;
            updated = null;

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string update = $"id={id}";

            if (person.Name != null) update += $", name='{person.Name}'";
            if (person.Age != null) update += $", age={person.Age}";
            if (person.Address != null) update += $", address='{person.Address}'";
            if (person.Work != null) update += $", work='{person.Work}'";

            var cmdStr = $"update person set {update} where id={id} returning *";

            using var command = new NpgsqlCommand(cmdStr, conn);

            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                updated = new Person()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Age = reader.GetInt32(2),
                    Address = reader.GetString(3),
                    Work = reader.GetString(4),
                };

                isFound = true;
            }

            reader.Close();
            conn.Close();
            return isFound;
        }
    }
}
