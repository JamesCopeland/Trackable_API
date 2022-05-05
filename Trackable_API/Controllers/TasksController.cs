#nullable disable
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Trackable_API.Models;

namespace Trackable_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TasksController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/Tasks
        [HttpGet]
        public IEnumerable<Models.Task> Get()
        {
            return SqlGetTasks();
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public Models.Task GetTask(long id)
        {
            var task = SqlGetTask(id);

            if (task == null)
            {
                return null;
            }

            return task;
        }

        [HttpPost]
        public async void PostTask()
        {

           var task = await HttpContext.Request.ReadFromJsonAsync<Models.Task>();

           SqlCreateTask(task);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(long id) 
        {
            SqlDeleteTask(id);

            return NoContent();
        }

        private void SqlCreateTask(Models.Task task)
        {
            string name = task.Name;
            string message = task.Message;
            string status = task.Status;
            string type = task.Type;
            string trace = task.Trace;

            using (var connection = new SqlConnection(_configuration.GetConnectionString("TaskDatabase")))
            {
                var sql = "INSERT INTO task (name, message, status, type, trace) VALUES (@name, @message, @status, @type, @trace)";
                using (SqlCommand command = new SqlCommand(sql))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@message", message);
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@type", type);
                    command.Parameters.AddWithValue("@trace", trace);
                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        private void SqlDeleteTask(long id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("TaskDatabase")))
            {
                Models.Task task = null;

                var sql = "DELETE task WHERE id = @id";
                using (SqlCommand command = new SqlCommand(sql))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        private IEnumerable<Models.Task> SqlGetTasks()
        {
            var tasks = new List<Models.Task>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("TaskDatabase")))
            {
                var sql = "SELECT id, name, message, status, type, trace FROM task";
                connection.Open();
                using SqlCommand command = new SqlCommand(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var task = new Models.Task()
                    {
                        Id = (long)reader["id"],
                        Name = reader["name"].ToString(),
                        Message = reader["message"].ToString(),
                        Status = reader["status"].ToString(),
                        Type = reader["type"].ToString(),
                        Trace = reader["trace"].ToString(),
                    };
                    tasks.Add(task);
                }
            }
            return tasks;
        }

        private Models.Task SqlGetTask(long id)
        {
            Models.Task task = null;

            using (var connection = new SqlConnection(_configuration.GetConnectionString("TaskDatabase")))
            {
                var sql = "SELECT id, name, message, status, type, trace FROM task WHERE id = " + id;
                connection.Open();
                using SqlCommand command = new SqlCommand(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    task = new Models.Task()
                    {
                        Id = (long)reader["id"],
                        Name = reader["name"].ToString(),
                        Message = reader["message"].ToString(),
                        Status = reader["status"].ToString(),
                        Type = reader["type"].ToString(),
                        Trace = reader["trace"].ToString(),
                    };
                }
            }

            return task;
        }

    }
}
