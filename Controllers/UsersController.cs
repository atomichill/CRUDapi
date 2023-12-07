using callapptask.Entitys;
using callapptask.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using testtask.Models;

namespace testtask.Controllers
{
    [Authorize]
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly IUserInfo _rep;
        public UsersController(IUserInfo rep)
        {
            _rep = rep ?? throw new ArgumentNullException(nameof(rep));
        }

        private HttpClient createClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
        private async Task<bool> ValidateUserId(int userId)
        {
            var user = await _rep.GetUserByIdAsync(userId);
            if(user != null) 
            {
                return true;
            }
            return false;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers() 
        {
            var users = await _rep.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet]
        [Route("Todos")]
        public async Task<ActionResult> GetUserTodos(int userId) 
        {
            bool userExists = await ValidateUserId(userId);
            if(userExists)
            {
                var client = createClient();
                HttpResponseMessage response = client.GetAsync($"/users/{userId}/todos").Result;

                return Ok(response.Content.ReadAsStringAsync().Result);
            } else
            {
                return NotFound("User not found.");
            }
        }
        [HttpGet]
        [Route("Photos")]
        public async Task<ActionResult> GetUserPhotos(int userId)
        {
            bool userExists = await ValidateUserId(userId);

            if (!userExists)
            {
                return NotFound("User not found.");
            }

            var client = createClient();

            HttpResponseMessage albumsResponse = await client.GetAsync($"/users/{userId}/albums");

            if (albumsResponse.IsSuccessStatusCode)
            {
                var albumsJson = await albumsResponse.Content.ReadAsStringAsync();
                var albums = JsonConvert.DeserializeObject<List<AlbumDto>>(albumsJson);

                List<PhotoDto> userPhotos = new List<PhotoDto>();

                foreach (var album in albums)
                {
                    HttpResponseMessage photosResponse = await client.GetAsync($"/albums/{album.id}/photos");

                    if (photosResponse.IsSuccessStatusCode)
                    {
                        var photosJson = await photosResponse.Content.ReadAsStringAsync();
                        var photos = JsonConvert.DeserializeObject<List<PhotoDto>>(photosJson);

                        userPhotos.AddRange(photos);
                    }
                    else
                    {
                        return StatusCode(500);
                    }
                }

                return Ok(userPhotos);
            }
            else
            {
                return StatusCode((int)albumsResponse.StatusCode, "Error fetching albums.");
            }
        }
        [HttpGet]
        [Route("Posts")]
        public async Task<ActionResult> GetUserAlbums(int userId)
        {
            bool userExists = await ValidateUserId(userId);

            if (!userExists)
            {
                return NotFound("User not found.");
            }

            var client = createClient();

            HttpResponseMessage postsResponse = await client.GetAsync($"/users/{userId}/posts");

            if (postsResponse.IsSuccessStatusCode)
            {
                var postsJson = await postsResponse.Content.ReadAsStringAsync();
                var posts = JsonConvert.DeserializeObject<List<PostDto>>(postsJson);

                foreach (var post in posts)
                {
                    HttpResponseMessage commentsResponse = await client.GetAsync($"/albums/{post.id}/photos");

                    if (commentsResponse.IsSuccessStatusCode)
                    {
                        var commentsJson = await commentsResponse.Content.ReadAsStringAsync();
                        var comments = JsonConvert.DeserializeObject<List<CommentDto>>(commentsJson);

                        post.comments = comments;
                    }
                    else
                    {
                        return StatusCode(500);
                    }
                }

                return Ok(posts);
            }
            else
            {
                return StatusCode((int)postsResponse.StatusCode, "Error fetching Posts.");
            }
        }

    }

}
