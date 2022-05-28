﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using People.Models;

namespace People
{
    public class PersonRepository
    {
        string dbPath;     

        public string StatusMessage { get; set; }

        private SQLiteAsyncConnection conn;

        private async Task Init()
        {      
            if (conn !=null) 
                return;

            conn = new SQLiteAsyncConnection(dbPath);

            await conn.CreateTableAsync<Person>();
        }

        public PersonRepository(string _dbPath)
        {
            dbPath = _dbPath;                        
        }

        public async Task AddNewPerson(string name)
        {            
            int result = 0;
            try
            {
                await Init();

                //basic validation to ensure a name was entered
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Valid name required");

                result = await conn.InsertAsync(new Person {  Name = name });

                StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, name);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
            }

        }

        public async Task<List<Person>> GetAllPeople()
        {            
            try
            {
                await Init();
                return await conn.Table<Person>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<Person>();
        }
    }
}
