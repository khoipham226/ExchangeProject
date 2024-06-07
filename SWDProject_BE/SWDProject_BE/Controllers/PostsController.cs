﻿using BusinessLayer.RequestModels;
using BusinessLayer.Services;
using DataLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace SWDProject_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            try
            {
                var posts = await _postService.GetAllPostsAsync();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error. Please try again later.");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            try
            {
                var post = await _postService.GetPostByIdAsync(id);
                if (post == null)
                {
                    return NotFound();
                }
                return Ok(post);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error. Please try again later.");
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin,user")]
        public async Task<ActionResult> CreatePost([FromBody] PostRequestModel createPostRequest)
        {
            try
            {
                // Take the user id from JWT
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized();
                }
                var userId = int.Parse(userIdClaim.Value);

                // Create a new post
                var post = new Post
                {
                    UserId = userId,
                    TransactionTypeId = createPostRequest.TransactionTypeId,
                    ProductId = (int)createPostRequest.ProductId,
                    Title = createPostRequest.Title,
                    Description = createPostRequest.Description,
                    Img = createPostRequest.Img,
                    Price = createPostRequest.Price,
                    Date = createPostRequest.Date,
                    Status = createPostRequest.Status
                };

                // Add the post using the post service
                await _postService.AddPostAsync(post);

                // Return the created post
                return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error. Please try again later.");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<ActionResult> UpdatePost(int id, PostRequestModel updatePostRequest)
        {
            try
            {
                var existingPost = await _postService.GetPostByIdAsync(id);
                if (existingPost == null)
                {
                    return NotFound();
                }

                // Take the user id from JWT
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized();
                }
                var userId = int.Parse(userIdClaim.Value);

                // Ensure that only the owner or an admin
                if (existingPost.UserId != userId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                // Update the post properties
                existingPost.TransactionTypeId = updatePostRequest.TransactionTypeId;
                existingPost.ProductId = (int)updatePostRequest.ProductId;
                existingPost.Title = updatePostRequest.Title;
                existingPost.Description = updatePostRequest.Description;
                existingPost.Img = updatePostRequest.Img;
                existingPost.Price = updatePostRequest.Price;
                existingPost.Date = updatePostRequest.Date;
                existingPost.Status = updatePostRequest.Status;

                await _postService.UpdatePostAsync(existingPost);
                return Ok("Post updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error. Please try again later.");
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<ActionResult> DeletePost(int id)
        {
            try
            {
                var existingPost = await _postService.GetPostByIdAsync(id);
                if (existingPost == null)
                {
                    return NotFound();
                }

                // Take the user id from JWT
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized();
                }
                var userId = int.Parse(userIdClaim.Value);

                // Ensure that only the owner or an admin
                if (existingPost.UserId != userId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                await _postService.DeletePostAsync(id);
                return Ok("Post deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error. Please try again later.");
            }
        }

    }
}