using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using NewsStacks.Database.Models;
using NewsStacks.DTOs;
using NewsStacks.DTOs.Enum;
using NewsStacks.Repositories;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace NewsStacks.BusinessService
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _repository;

        public ArticleService(IArticleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Article> Create(Article article, string role, string userId)
        {
            if (Convert.ToInt32(role) == (int)RoleType.Writer)
            {
                article.ReviewerDone = false;
                article.PublishDone = false;
                article.Active = true;
                article.WriteDone = true;
                if (article.IsDraft)
                {
                    article.WriteDone = false;
                }
                article.CreatedDate = DateTime.UtcNow;
                article.UpdateDate = DateTime.UtcNow;
                article.PublishedDate = null;

                article.Id = await _repository.Create(article, role);
                await _repository.CreateArticleUser(role, userId, article.Id);

                if (!article.IsDraft)
                {
                    var message = new ArticleMessage { Id = article.Id, Title = article.Title, MessageType = MessageType.WriterDone };
                    this.CreateArticleNotification(message);
                }
            }
            else
            {
                article = null;
            }

            return article;
        }

        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<IEnumerable<Article>> GetAll(string role, bool published)
        {
            return await _repository.GetAll(role, published);
        }

        public async Task<Article> GetById(int id, string role)
        {
            return await _repository.GetById(id, role);
        }

        public bool CreateArticleNotification(ArticleMessage message)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                CloudQueue queue = queueClient.GetQueueReference("mail-notification");
                queue.CreateIfNotExistsAsync();
                string messsage = JsonSerializer.Serialize(message);
                queue.AddMessageAsync(new CloudQueueMessage(messsage));

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Article> Update(int id, Article article, string role, string userId)
        {
            var model = await _repository.GetById(id, role);

            if (Convert.ToInt32(role) == (int)RoleType.Writer)
            {
                if (model.WriteDone)
                {
                    //Article Write completed already
                    throw new InvalidOperationException("Article Write completed already!!");
                }

                model.Title = article.Title;
                model.Description = article.Description;
                model.Topics = article.Topics;
                model.Tags = article.Tags;
                model.UpdateDate = DateTime.UtcNow;
                model.WriteDone = false;

                await _repository.CreateArticleUser(role, userId, article.Id);

                await _repository.Update(id, model, role, userId);

                if (!article.IsDraft)
                {
                    model.WriteDone = true;
                    //Article message Queue 
                    var message = new ArticleMessage { Id = model.Id, Title = model.Title, MessageType = MessageType.WriterDone };
                    this.CreateArticleNotification(message);
                }

            }
            else if (Convert.ToInt32(role) == (int)RoleType.Reviewer)
            {
                if (model.ReviewerDone)
                {
                    throw new InvalidOperationException("Article Reviewe completed already.!!");
                }

                model.ReviewerDone = true;
                model.UpdateDate = DateTime.UtcNow;
                model.ReviewerComments = article.ReviewerComments;

                await _repository.CreateArticleUser(role, userId, article.Id);

                await _repository.Update(id, model, role, userId);

                //Article message Queue 
                var message = new ArticleMessage { Id = article.Id, Title = article.Title, MessageType = MessageType.ReviewerDone };
                this.CreateArticleNotification(message);

            }
            else if (Convert.ToInt32(role) == (int)RoleType.Editor)
            {
                if (model.EditorDone)
                {
                    throw new InvalidOperationException("Article Editor completed already.!!");
                }

                model.EditorDone = true;
                model.UpdateDate = DateTime.UtcNow;
                model.EditorComments = article.EditorComments;
                await _repository.CreateArticleUser(role, userId, article.Id);

                await _repository.Update(id, model, role, userId);

                //Article message Queue 
                var message = new ArticleMessage { Id = article.Id, Title = article.Title, MessageType = MessageType.EditorDone };
                this.CreateArticleNotification(message);

            }
            else if (Convert.ToInt32(role) == (int)RoleType.Publisher)
            {
                if (model.PublishDone)
                {
                    throw new InvalidOperationException("Article Publish completed already.!!");
                }

                model.PublishDone = true;
                model.UpdateDate = DateTime.UtcNow;
                model.PublishedDate = DateTime.UtcNow;
                model.Tags = article.Tags;

                await _repository.CreateArticleUser(role, userId, article.Id);

                await _repository.Update(id, model, role, userId);

                //Article message Queue 
                var message = new ArticleMessage { Id = article.Id, Title = article.Title, MessageType = MessageType.PublisherDone };
                this.CreateArticleNotification(message);
            }

            return model;
        }

    }
}
