using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {   
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment commenModel)
        {
            await _context.Comments.AddAsync(commenModel);
            await _context.SaveChangesAsync();
            return commenModel;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var exisitingComment = await _context.Comments.FindAsync(id);
            if (exisitingComment == null)
            {
                return null;
            }

            _context.Comments.Remove(exisitingComment);
            await _context.SaveChangesAsync();
            return exisitingComment;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.Include(c => c.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.Include(c => c.AppUser).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var exisitingComment = await _context.Comments.FindAsync(id);
            if (exisitingComment == null)
            {
                return null;
            }

            exisitingComment.Title = commentModel.Title;
            exisitingComment.Content = commentModel.Content;

            await _context.SaveChangesAsync();
            return exisitingComment;
        }
    }
}