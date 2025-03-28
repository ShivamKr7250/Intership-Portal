﻿using Internship_Portal.Model;

namespace Internship_Portal.Controllers.Service
{
    public class BlogPostService
    {
        public List<string> GetTags(BlogPost blogPost)
        {
            if (string.IsNullOrEmpty(blogPost.Tags))
            {
                return new List<string>();
            }
            return blogPost.Tags.Split(',').Select(tag => tag.Trim()).ToList();
        }

        public void SetTags(BlogPost blogPost, List<string> tags)
        {
            blogPost.Tags = string.Join(",", tags);
        }
    }
}
